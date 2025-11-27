using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PayrollSample
{
    public partial class FrmPayrollProcessing : Form
    {
        private readonly string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=PayrollDB;Integrated Security=True";
        private DataTable payrollData;

        public FrmPayrollProcessing()
        {
            InitializeComponent();
            payrollData = new DataTable();
            InitializeDataTable();
        }

        private void InitializeDataTable()
        {
            payrollData.Columns.Add("UserID", typeof(int));
            payrollData.Columns.Add("EmployeeName", typeof(string));
            payrollData.Columns.Add("TotalHours", typeof(decimal));
            payrollData.Columns.Add("GrossPay", typeof(decimal));
            payrollData.Columns.Add("Deductions", typeof(decimal));
            payrollData.Columns.Add("NetPay", typeof(decimal));
            payrollData.Columns.Add("Status", typeof(string));

            dataGridViewPayroll.DataSource = payrollData;
            dataGridViewPayroll.AutoGenerateColumns = true;
            dataGridViewPayroll.ReadOnly = true;
            dataGridViewPayroll.AllowUserToAddRows = false;
            dataGridViewPayroll.AllowUserToDeleteRows = false;

            // Hide UserID column and format numeric columns
            if (dataGridViewPayroll.Columns["UserID"] != null)
            {
                dataGridViewPayroll.Columns["UserID"].Visible = false;
            }

            FormatDataGridViewColumns();
        }

        private void FormatDataGridViewColumns()
        {
            if (dataGridViewPayroll.Columns["TotalHours"] != null)
            {
                dataGridViewPayroll.Columns["TotalHours"].DefaultCellStyle.Format = "N2";
            }

            if (dataGridViewPayroll.Columns["GrossPay"] != null)
            {
                dataGridViewPayroll.Columns["GrossPay"].DefaultCellStyle.Format = "C2";
            }

            if (dataGridViewPayroll.Columns["Deductions"] != null)
            {
                dataGridViewPayroll.Columns["Deductions"].DefaultCellStyle.Format = "C2";
            }

            if (dataGridViewPayroll.Columns["NetPay"] != null)
            {
                dataGridViewPayroll.Columns["NetPay"].DefaultCellStyle.Format = "C2";
            }
        }

        private void btnLoadAttendance_Click(object sender, EventArgs e)
        {
            if (dtpFrom.Value > dtpTo.Value)
            {
                MessageBox.Show("From date cannot be later than To date.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                payrollData.Rows.Clear();
                Cursor = Cursors.WaitCursor;
                btnLoadAttendance.Enabled = false;

                // First, load all employees into a list (close reader before processing)
                var employees = new System.Collections.Generic.List<EmployeeInfo>();

                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Get all employees
                    var employeesQuery = @"SELECT UserID, FirstName, LastName, salary_rate, salary_type 
                                          FROM Users 
                                          WHERE Role = 'Employee' AND Status = 'Active'";

                    using (var employeesCmd = new SqlCommand(employeesQuery, conn))
                    using (var employeesReader = employeesCmd.ExecuteReader())
                    {
                        while (employeesReader.Read())
                        {
                            employees.Add(new EmployeeInfo
                            {
                                UserID = Convert.ToInt32(employeesReader["UserID"]),
                                FirstName = employeesReader["FirstName"].ToString(),
                                LastName = employeesReader["LastName"].ToString(),
                                SalaryRate = Convert.ToDecimal(employeesReader["salary_rate"]),
                                SalaryType = employeesReader["salary_type"].ToString()
                            });
                        }
                    }

                    // Now process each employee (reader is closed)
                    foreach (var emp in employees)
                    {
                        string employeeName = $"{emp.FirstName} {emp.LastName}";

                        // Calculate total hours for this employee in the date range
                        decimal totalHours = CalculateTotalHours(conn, emp.UserID, dtpFrom.Value, dtpTo.Value);

                        // Calculate gross pay based on salary type
                        decimal grossPay = CalculateGrossPay(totalHours, emp.SalaryRate, emp.SalaryType);

                        // Calculate total deductions from Deductions table
                        decimal deductions = CalculateTotalDeductions(conn, emp.UserID);
                        decimal netPay = grossPay - deductions;

                        // Check if payslip already exists for this period
                        string status = CheckPayslipStatus(conn, emp.UserID, dtpFrom.Value, dtpTo.Value) ? "Generated" : "Pending";

                        payrollData.Rows.Add(emp.UserID, employeeName, totalHours, grossPay, deductions, netPay, status);
                    }
                }

                FormatDataGridViewColumns();
                MessageBox.Show($"Loaded payroll data for {payrollData.Rows.Count} employee(s).", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load attendance data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                btnLoadAttendance.Enabled = true;
            }
        }

        private decimal CalculateTotalHours(SqlConnection conn, int userId, DateTime fromDate, DateTime toDate)
        {
            // Calculate total hours worked from attendance records
            // Only count records where both time_in and time_out are present
            var query = @"SELECT SUM(CAST(DATEDIFF(SECOND, time_in, time_out) AS DECIMAL(10,2)) / 3600.0) AS TotalHours
                         FROM Attendance
                         WHERE UserID = @UserID 
                           AND [date] >= @FromDate 
                           AND [date] <= @ToDate
                           AND time_in IS NOT NULL 
                           AND time_out IS NOT NULL";

            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@FromDate", fromDate.Date);
                cmd.Parameters.AddWithValue("@ToDate", toDate.Date);

                var result = cmd.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                {
                    return 0m;
                }

                return Convert.ToDecimal(result);
            }
        }

        private decimal CalculateGrossPay(decimal totalHours, decimal salaryRate, string salaryType)
        {
            if (totalHours <= 0)
            {
                return 0m;
            }

            if (salaryType == "Hourly")
            {
                return totalHours * salaryRate;
            }
            else if (salaryType == "Monthly")
            {
                // Convert monthly salary to hourly rate: monthly / 22 working days / 8 hours per day
                decimal hourlyRate = salaryRate / 22m / 8m;
                return totalHours * hourlyRate;
            }
            else
            {
                // For other types (Daily, Annual), default to hourly calculation
                // You can extend this logic as needed
                return totalHours * salaryRate;
            }
        }

        private decimal CalculateTotalDeductions(SqlConnection conn, int userId)
        {
            try
            {
                // First, check if Deductions table exists and get its column names
                var tableExistsQuery = @"SELECT COUNT(*) 
                                       FROM INFORMATION_SCHEMA.TABLES 
                                       WHERE TABLE_NAME = 'Deductions'";

                using (var checkCmd = new SqlCommand(tableExistsQuery, conn))
                {
                    int tableCount = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (tableCount == 0)
                    {
                        return 0m; // Table doesn't exist
                    }
                }

                // Get column names from Deductions table
                var columnQuery = @"SELECT COLUMN_NAME 
                                  FROM INFORMATION_SCHEMA.COLUMNS 
                                  WHERE TABLE_NAME = 'Deductions'";

                var columnNames = new System.Collections.Generic.Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                using (var colCmd = new SqlCommand(columnQuery, conn))
                using (var reader = colCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string colName = reader["COLUMN_NAME"].ToString();
                        columnNames[colName] = colName;
                    }
                }

                // Find column names (case-insensitive)
                string userIdCol = FindColumnName(columnNames, new[] { "user_id", "UserID", "EmployeeID", "employee_id" });
                string amountCol = FindColumnName(columnNames, new[] { "amount", "Amount", "deduction_amount", "DeductionAmount" });
                string statusCol = FindColumnName(columnNames, new[] { "status", "Status", "active", "Active", "is_active", "IsActive" });

                if (userIdCol == null || amountCol == null)
                {
                    return 0m; // Required columns not found
                }

                // Build query dynamically
                string whereClause = $"[{userIdCol}] = @UserID";
                if (statusCol != null)
                {
                    // Check if status column exists and add condition
                    whereClause += $" AND ([{statusCol}] = 'Active' OR [{statusCol}] = 1 OR [{statusCol}] IS NULL)";
                }

                var query = $@"SELECT SUM([{amountCol}]) AS TotalDeductions
                             FROM Deductions
                             WHERE {whereClause}";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    var result = cmd.ExecuteScalar();
                    if (result == null || result == DBNull.Value)
                    {
                        return 0m;
                    }

                    return Convert.ToDecimal(result);
                }
            }
            catch
            {
                // If Deductions table doesn't exist or has different structure, return 0
                return 0m;
            }
        }

        private bool CheckPayslipStatus(SqlConnection conn, int userId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                // Get actual column names from the Payslips table
                var columnNames = GetPayslipsColumnNames(conn);
                
                if (columnNames == null || columnNames.Count == 0)
                {
                    return false; // Table might not have expected structure
                }

                // Build query using actual column names
                string userIdCol = columnNames.ContainsKey("UserID") ? columnNames["UserID"] : 
                                   columnNames.ContainsKey("user_id") ? columnNames["user_id"] : 
                                   columnNames.ContainsKey("EmployeeID") ? columnNames["EmployeeID"] : null;
                
                string periodFromCol = columnNames.ContainsKey("PeriodFrom") ? columnNames["PeriodFrom"] : 
                                      columnNames.ContainsKey("period_from") ? columnNames["period_from"] : 
                                      columnNames.ContainsKey("FromDate") ? columnNames["FromDate"] : null;
                
                string periodToCol = columnNames.ContainsKey("PeriodTo") ? columnNames["PeriodTo"] : 
                                    columnNames.ContainsKey("period_to") ? columnNames["period_to"] : 
                                    columnNames.ContainsKey("ToDate") ? columnNames["ToDate"] : null;
                
                string statusCol = columnNames.ContainsKey("Status") ? columnNames["Status"] : 
                                  columnNames.ContainsKey("status") ? columnNames["status"] : null;

                if (userIdCol == null || periodFromCol == null || periodToCol == null || statusCol == null)
                {
                    return false; // Required columns not found
                }

                // Check if a payslip already exists for this employee and period
                var query = $@"SELECT COUNT(*) 
                             FROM Payslips
                             WHERE [{userIdCol}] = @UserID 
                               AND [{periodFromCol}] = @FromDate 
                               AND [{periodToCol}] = @ToDate
                               AND [{statusCol}] = 'Generated'";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@FromDate", fromDate.Date);
                    cmd.Parameters.AddWithValue("@ToDate", toDate.Date);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch
            {
                // If error occurs, return false (not generated)
                return false;
            }
        }

        private System.Collections.Generic.Dictionary<string, string> GetPayslipsColumnNames(SqlConnection conn)
        {
            var columnNames = new System.Collections.Generic.Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            
            try
            {
                // First, get all columns
                var query = @"SELECT COLUMN_NAME 
                            FROM INFORMATION_SCHEMA.COLUMNS 
                            WHERE TABLE_NAME = 'Payslips'
                            ORDER BY ORDINAL_POSITION";

                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string colName = reader["COLUMN_NAME"].ToString();
                        columnNames[colName] = colName;
                    }
                }

                // Then check for identity columns separately (more reliable)
                var identityQuery = @"SELECT c.name AS COLUMN_NAME
                                    FROM sys.columns c
                                    INNER JOIN sys.tables t ON c.object_id = t.object_id
                                    WHERE t.name = 'Payslips' AND c.is_identity = 1";

                using (var cmd = new SqlCommand(identityQuery, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string colName = reader["COLUMN_NAME"].ToString();
                        columnNames[$"{colName}_IS_IDENTITY"] = "1";
                    }
                }
            }
            catch
            {
                return null;
            }

            return columnNames;
        }

        private void btnGeneratePayroll_Click(object sender, EventArgs e)
        {
            if (payrollData.Rows.Count == 0)
            {
                MessageBox.Show("Please load attendance data first.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show(
                $"Are you sure you want to generate payslips for {payrollData.Rows.Count} employee(s)?",
                "Confirm Generation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult != DialogResult.Yes)
            {
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                btnGeneratePayroll.Enabled = false;

                int successCount = 0;
                int skipCount = 0;

                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    EnsurePayslipColumnsExist(conn);

                    // Get actual column names from Payslips table
                    var columnNames = GetPayslipsColumnNames(conn);
                    if (columnNames == null || columnNames.Count == 0)
                    {
                        MessageBox.Show("Unable to read Payslips table structure. Please check the table exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    foreach (DataRow row in payrollData.Rows)
                    {
                        int userId = Convert.ToInt32(row["UserID"]);
                        decimal totalHours = Convert.ToDecimal(row["TotalHours"]);
                        decimal grossPay = Convert.ToDecimal(row["GrossPay"]);
                        decimal deductions = Convert.ToDecimal(row["Deductions"]);
                        decimal netPay = Convert.ToDecimal(row["NetPay"]);
                        string currentStatus = row["Status"].ToString();

                        // Skip if already generated
                        if (currentStatus == "Generated")
                        {
                            skipCount++;
                            continue;
                        }

                        // Check again to prevent duplicates
                        if (CheckPayslipStatus(conn, userId, dtpFrom.Value, dtpTo.Value))
                        {
                            row["Status"] = "Generated";
                            skipCount++;
                            continue;
                        }

                        // Build INSERT query using actual column names (exclude identity columns)
                        string insertQuery = BuildInsertQuery(columnNames);

                        if (string.IsNullOrEmpty(insertQuery))
                        {
                            throw new Exception("Could not build INSERT query. Check Payslips table structure.");
                        }

                        using (var cmd = new SqlCommand(insertQuery, conn))
                        {
                            AddInsertParameters(cmd, columnNames, userId, dtpFrom.Value.Date, dtpTo.Value.Date, 
                                totalHours, grossPay, deductions, netPay);

                            cmd.ExecuteNonQuery();
                        }

                        row["Status"] = "Generated";
                        successCount++;
                    }
                }

                string message = $"Payslips generated successfully!\n\nGenerated: {successCount}\nSkipped (already generated): {skipCount}";
                MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to generate payslips: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                btnGeneratePayroll.Enabled = true;
            }
        }

        private string BuildInsertQuery(System.Collections.Generic.Dictionary<string, string> columnNames)
        {
            // Find column names (case-insensitive) - exclude identity columns and ID columns
            string userIdCol = FindColumnName(columnNames, new[] { "UserID", "user_id", "EmployeeID", "employee_id" });
            string periodFromCol = FindColumnName(columnNames, new[] { "PeriodFrom", "period_from", "FromDate", "from_date", "StartDate", "start_date" });
            string periodToCol = FindColumnName(columnNames, new[] { "PeriodTo", "period_to", "ToDate", "to_date", "EndDate", "end_date" });
            string totalHoursCol = FindColumnName(columnNames, new[] { "TotalHours", "total_hours", "Hours", "hours" });
            string grossPayCol = FindColumnName(columnNames, new[] { "GrossPay", "gross_pay", "Gross", "gross" });
            string deductionsCol = FindColumnName(columnNames, new[] { "Deductions", "deductions", "Deduction", "deduction" });
            string netPayCol = FindColumnName(columnNames, new[] { "NetPay", "net_pay", "Net", "net" });
            string statusCol = FindColumnName(columnNames, new[] { "Status", "status" });
            string generatedDateCol = FindColumnName(columnNames, new[] { "GeneratedDate", "generated_date", "CreatedDate", "created_date", "DateGenerated", "date_generated" });

            // Build column list and values list (exclude identity columns and ID columns)
            var columns = new System.Collections.Generic.List<string>();
            var values = new System.Collections.Generic.List<string>();

            // Only add columns that exist, are not identity columns, and are not ID columns
            if (userIdCol != null && !IsIdentityColumn(columnNames, userIdCol) && !IsIdColumn(userIdCol)) 
            { 
                columns.Add($"[{userIdCol}]"); 
                values.Add("@UserID"); 
            }
            if (periodFromCol != null && !IsIdentityColumn(columnNames, periodFromCol) && !IsIdColumn(periodFromCol)) 
            { 
                columns.Add($"[{periodFromCol}]"); 
                values.Add("@PeriodFrom"); 
            }
            if (periodToCol != null && !IsIdentityColumn(columnNames, periodToCol) && !IsIdColumn(periodToCol)) 
            { 
                columns.Add($"[{periodToCol}]"); 
                values.Add("@PeriodTo"); 
            }
            if (totalHoursCol != null && !IsIdentityColumn(columnNames, totalHoursCol) && !IsIdColumn(totalHoursCol)) 
            { 
                columns.Add($"[{totalHoursCol}]"); 
                values.Add("@TotalHours"); 
            }
            if (grossPayCol != null && !IsIdentityColumn(columnNames, grossPayCol) && !IsIdColumn(grossPayCol)) 
            { 
                columns.Add($"[{grossPayCol}]"); 
                values.Add("@GrossPay"); 
            }
            if (deductionsCol != null && !IsIdentityColumn(columnNames, deductionsCol) && !IsIdColumn(deductionsCol)) 
            { 
                columns.Add($"[{deductionsCol}]"); 
                values.Add("@Deductions"); 
            }
            if (netPayCol != null && !IsIdentityColumn(columnNames, netPayCol) && !IsIdColumn(netPayCol)) 
            { 
                columns.Add($"[{netPayCol}]"); 
                values.Add("@NetPay"); 
            }
            if (statusCol != null && !IsIdentityColumn(columnNames, statusCol) && !IsIdColumn(statusCol)) 
            { 
                columns.Add($"[{statusCol}]"); 
                values.Add("@Status"); 
            }
            if (generatedDateCol != null && !IsIdentityColumn(columnNames, generatedDateCol) && !IsIdColumn(generatedDateCol)) 
            { 
                columns.Add($"[{generatedDateCol}]"); 
                values.Add("@GeneratedDate"); 
            }

            if (columns.Count == 0)
            {
                return null; // Return null instead of throwing exception
            }

            return $"INSERT INTO Payslips ({string.Join(", ", columns)}) VALUES ({string.Join(", ", values)})";
        }

        private bool IsIdColumn(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                return false;
                
            string lowerName = columnName.ToLower();
            // Exclude common ID column patterns (payroll_id, PayslipID, id, etc.)
            return lowerName == "payroll_id" || 
                   lowerName == "payslipid" || 
                   lowerName == "id" || 
                   (lowerName.EndsWith("_id") && (lowerName.Contains("payroll") || lowerName.Contains("payslip")));
        }

        private bool IsIdentityColumn(System.Collections.Generic.Dictionary<string, string> columnNames, string columnName)
        {
            // Check if this column is marked as identity
            if (columnNames.ContainsKey($"{columnName}_IS_IDENTITY"))
            {
                return true;
            }
            
            // Also check common identity column patterns (payroll_id, PayslipID, etc.)
            // These are typically identity columns
            if (columnName != null)
            {
                string lowerName = columnName.ToLower();
                if (lowerName == "payroll_id" || lowerName == "payslipid" || 
                    lowerName == "id" || (lowerName.EndsWith("_id") && lowerName.Contains("payroll")))
                {
                    // Check if it's actually marked as identity in the dictionary
                    return columnNames.ContainsKey($"{columnName}_IS_IDENTITY");
                }
            }
            
            return false;
        }

        private void AddInsertParameters(SqlCommand cmd, System.Collections.Generic.Dictionary<string, string> columnNames, 
            int userId, DateTime periodFrom, DateTime periodTo, decimal totalHours, decimal grossPay, decimal deductions, decimal netPay)
        {
            // Find column names (case-insensitive)
            string userIdCol = FindColumnName(columnNames, new[] { "UserID", "user_id", "EmployeeID", "employee_id" });
            string periodFromCol = FindColumnName(columnNames, new[] { "PeriodFrom", "period_from", "FromDate", "from_date", "StartDate", "start_date" });
            string periodToCol = FindColumnName(columnNames, new[] { "PeriodTo", "period_to", "ToDate", "to_date", "EndDate", "end_date" });
            string totalHoursCol = FindColumnName(columnNames, new[] { "TotalHours", "total_hours", "Hours", "hours" });
            string grossPayCol = FindColumnName(columnNames, new[] { "GrossPay", "gross_pay", "Gross", "gross" });
            string deductionsCol = FindColumnName(columnNames, new[] { "Deductions", "deductions", "Deduction", "deduction" });
            string netPayCol = FindColumnName(columnNames, new[] { "NetPay", "net_pay", "Net", "net" });
            string statusCol = FindColumnName(columnNames, new[] { "Status", "status" });
            string generatedDateCol = FindColumnName(columnNames, new[] { "GeneratedDate", "generated_date", "CreatedDate", "created_date", "DateGenerated", "date_generated" });

            if (userIdCol != null) cmd.Parameters.AddWithValue("@UserID", userId);
            if (periodFromCol != null) cmd.Parameters.AddWithValue("@PeriodFrom", periodFrom);
            if (periodToCol != null) cmd.Parameters.AddWithValue("@PeriodTo", periodTo);
            if (totalHoursCol != null) cmd.Parameters.AddWithValue("@TotalHours", totalHours);
            if (grossPayCol != null) cmd.Parameters.AddWithValue("@GrossPay", grossPay);
            if (deductionsCol != null) cmd.Parameters.AddWithValue("@Deductions", deductions);
            if (netPayCol != null) cmd.Parameters.AddWithValue("@NetPay", netPay);
            if (statusCol != null) cmd.Parameters.AddWithValue("@Status", "Generated");
            if (generatedDateCol != null) cmd.Parameters.AddWithValue("@GeneratedDate", DateTime.Now);
        }

        private string FindColumnName(System.Collections.Generic.Dictionary<string, string> columnNames, string[] possibleNames)
        {
            foreach (var name in possibleNames)
            {
                if (columnNames.ContainsKey(name))
                {
                    return columnNames[name];
                }
            }
            return null;
        }

        private void EnsurePayslipColumnsExist(SqlConnection conn)
        {
            var requiredColumns = new System.Collections.Generic.Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "UserID", "INT NULL" },
                { "PeriodFrom", "DATE NULL" },
                { "PeriodTo", "DATE NULL" },
                { "TotalHours", "DECIMAL(18,2) NULL" },
                { "GrossPay", "DECIMAL(18,2) NULL" },
                { "Deductions", "DECIMAL(18,2) NULL" },
                { "NetPay", "DECIMAL(18,2) NULL" },
                { "Status", "NVARCHAR(50) NULL" },
                { "GeneratedDate", "DATETIME NULL" }
            };

            foreach (var column in requiredColumns)
            {
                if (!ColumnExists(conn, "Payslips", column.Key))
                {
                    var alterSql = $"ALTER TABLE Payslips ADD [{column.Key}] {column.Value}";
                    using (var alterCmd = new SqlCommand(alterSql, conn))
                    {
                        alterCmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private bool ColumnExists(SqlConnection conn, string tableName, string columnName)
        {
            var query = @"SELECT COUNT(*) 
                        FROM INFORMATION_SCHEMA.COLUMNS 
                        WHERE TABLE_NAME = @TableName AND COLUMN_NAME = @ColumnName";

            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@TableName", tableName);
                cmd.Parameters.AddWithValue("@ColumnName", columnName);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private class EmployeeInfo
        {
            public int UserID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public decimal SalaryRate { get; set; }
            public string SalaryType { get; set; }
        }
    }
}

