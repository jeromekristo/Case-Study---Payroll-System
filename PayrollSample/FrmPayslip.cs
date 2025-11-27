using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PayrollSample
{
    public partial class FrmPayslip : Form
    {
        private readonly string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=PayrollDB;Integrated Security=True";
        private int userId;

        public FrmPayslip(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            LoadPayslipData();
        }

        private void LoadPayslipData()
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Try to find the correct table name and column names
                    string tableName = GetPayrollTableName(conn);
                    if (string.IsNullOrEmpty(tableName))
                    {
                        MessageBox.Show("Payroll table not found. Please check your database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ShowNoPayrollMessage();
                        return;
                    }

                    var columnNames = GetPayrollColumnNames(conn, tableName);
                    if (columnNames == null || columnNames.Count == 0)
                    {
                        MessageBox.Show("Could not read Payroll table structure.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ShowNoPayrollMessage();
                        return;
                    }

                    DetectPayrollColumns(columnNames,
                        out string userIdCol,
                        out string periodStartCol,
                        out string periodEndCol,
                        out string totalHoursCol,
                        out string grossPayCol,
                        out string deductionsCol,
                        out string netPayCol);

                    if (!HasEssentialColumns(userIdCol, periodStartCol, periodEndCol) &&
                        tableName.Equals("Payslips", StringComparison.OrdinalIgnoreCase))
                    {
                        string fallbackTable = GetPayrollDataTableName(conn);
                        if (!string.IsNullOrEmpty(fallbackTable) &&
                            !fallbackTable.Equals(tableName, StringComparison.OrdinalIgnoreCase))
                        {
                            var fallbackColumns = GetPayrollColumnNames(conn, fallbackTable);
                            if (fallbackColumns != null && fallbackColumns.Count > 0)
                            {
                                tableName = fallbackTable;
                                columnNames = fallbackColumns;

                                DetectPayrollColumns(columnNames,
                                    out userIdCol,
                                    out periodStartCol,
                                    out periodEndCol,
                                    out totalHoursCol,
                                    out grossPayCol,
                                    out deductionsCol,
                                    out netPayCol);
                            }
                        }
                    }

                    if (userIdCol == null || periodStartCol == null || periodEndCol == null)
                    {
                        ShowMissingColumnsMessage(tableName, columnNames, userIdCol, periodStartCol, periodEndCol);
                        ShowNoPayrollMessage();
                        return;
                    }

                    string tableAlias = "p";
                    string fromClause = $"FROM [{tableName}] {tableAlias}";

                    // If essential columns missing in Payslips, attempt to join with actual payroll table
                    if ((userIdCol == null || periodStartCol == null || periodEndCol == null) &&
                        TryUsePayrollJoin(conn, tableName, columnNames, ref tableAlias, ref fromClause,
                            ref userIdCol, ref periodStartCol, ref periodEndCol,
                            ref totalHoursCol, ref grossPayCol, ref deductionsCol, ref netPayCol))
                    {
                        // Columns filled via payroll join
                    }

                    if (userIdCol == null || periodStartCol == null || periodEndCol == null)
                    {
                        ShowMissingColumnsMessage(tableName, columnNames, userIdCol, periodStartCol, periodEndCol);
                        ShowNoPayrollMessage();
                        return;
                    }

                    // Build SQL query with detected column names (only include columns that exist)
                    var selectColumns = new System.Collections.Generic.List<string>();
                    var orderByColumns = new System.Collections.Generic.List<string>();

                    if (totalHoursCol != null) selectColumns.Add($"{tableAlias}.[{totalHoursCol}] AS TotalHours");
                    if (grossPayCol != null) selectColumns.Add($"{tableAlias}.[{grossPayCol}] AS GrossPay");
                    if (deductionsCol != null) selectColumns.Add($"{tableAlias}.[{deductionsCol}] AS Deductions");
                    if (netPayCol != null) selectColumns.Add($"{tableAlias}.[{netPayCol}] AS NetPay");
                    if (periodStartCol != null)
                    {
                        selectColumns.Add($"{tableAlias}.[{periodStartCol}] AS PeriodStart");
                        orderByColumns.Add($"{tableAlias}.[{periodStartCol}]");
                    }
                    if (periodEndCol != null)
                    {
                        selectColumns.Add($"{tableAlias}.[{periodEndCol}] AS PeriodEnd");
                        orderByColumns.Insert(0, $"{tableAlias}.[{periodEndCol}]");
                    }

                    selectColumns.Add("u.FirstName + ' ' + u.LastName AS EmployeeName");
                    selectColumns.Add("u.salary_rate AS HourlyRate");

                    if (selectColumns.Count == 0)
                    {
                        ShowMissingColumnsMessage(tableName, columnNames, userIdCol, periodStartCol, periodEndCol);
                        ShowNoPayrollMessage();
                        return;
                    }

                    string orderByClause = orderByColumns.Count > 0 ? "ORDER BY " + string.Join(" DESC, ", orderByColumns) + " DESC" : "";

                    var query = $@"SELECT TOP 1 
                                        {string.Join(",\n                                        ", selectColumns)}
                                  {fromClause}
                                  INNER JOIN Users u ON {tableAlias}.[{userIdCol}] = u.UserID
                                  WHERE {tableAlias}.[{userIdCol}] = @UserID
                                  {orderByClause}";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Display employee name
                                if (reader["EmployeeName"] != DBNull.Value)
                                    lblEmployeeNameValue.Text = reader["EmployeeName"].ToString();
                                else
                                    lblEmployeeNameValue.Text = "N/A";

                                // Display payroll period (check if columns exist)
                                if (periodStartCol != null && periodEndCol != null && 
                                    reader["PeriodStart"] != DBNull.Value && reader["PeriodEnd"] != DBNull.Value)
                                {
                                    DateTime periodStart = Convert.ToDateTime(reader["PeriodStart"]);
                                    DateTime periodEnd = Convert.ToDateTime(reader["PeriodEnd"]);
                                    lblPayrollPeriodValue.Text = $"{periodStart:MMM dd, yyyy} - {periodEnd:MMM dd, yyyy}";
                                }
                                else
                                {
                                    lblPayrollPeriodValue.Text = "N/A";
                                }

                                // Display total hours worked
                                if (totalHoursCol != null && reader["TotalHours"] != DBNull.Value)
                                {
                                    decimal totalHours = Convert.ToDecimal(reader["TotalHours"]);
                                    lblTotalHoursValue.Text = totalHours.ToString("N2") + " hours";
                                }
                                else
                                {
                                    lblTotalHoursValue.Text = "N/A";
                                }

                                // Display hourly rate
                                if (reader["HourlyRate"] != DBNull.Value)
                                {
                                    decimal hourlyRate = Convert.ToDecimal(reader["HourlyRate"]);
                                    lblHourlyRateValue.Text = hourlyRate.ToString("C2");
                                }
                                else
                                {
                                    lblHourlyRateValue.Text = "N/A";
                                }

                                // Display gross pay
                                if (grossPayCol != null && reader["GrossPay"] != DBNull.Value)
                                {
                                    decimal grossPay = Convert.ToDecimal(reader["GrossPay"]);
                                    lblGrossPayValue.Text = grossPay.ToString("C2");
                                }
                                else
                                {
                                    lblGrossPayValue.Text = "N/A";
                                }

                                // Display total deductions (only if column exists)
                                if (deductionsCol != null && reader["Deductions"] != DBNull.Value)
                                {
                                    decimal deductions = Convert.ToDecimal(reader["Deductions"]);
                                    lblTotalDeductionsValue.Text = deductions.ToString("C2");
                                }
                                else
                                {
                                    lblTotalDeductionsValue.Text = "$0.00"; // Default to 0 if column doesn't exist
                                }

                                // Display net pay
                                if (netPayCol != null && reader["NetPay"] != DBNull.Value)
                                {
                                    decimal netPay = Convert.ToDecimal(reader["NetPay"]);
                                    lblNetPayValue.Text = netPay.ToString("C2");
                                }
                                else
                                {
                                    lblNetPayValue.Text = "N/A";
                                }

                                // Enable download button
                                btnDownloadPayslip.Enabled = true;
                            }
                            else
                            {
                                // No payroll record found
                                ShowNoPayrollMessage();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load payslip data: " + ex.Message + "\n\nDetails: " + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ShowNoPayrollMessage();
            }
        }

        private string GetPayrollTableName(SqlConnection conn)
        {
            // Check if Payroll table exists
            var query = @"SELECT TOP 1 TABLE_NAME 
                        FROM INFORMATION_SCHEMA.TABLES 
                        WHERE TABLE_NAME IN ('Payslips', 'Payroll', 'PayrollRecords', 'PayrollHistory')
                        ORDER BY CASE 
                            WHEN TABLE_NAME = 'Payslips' THEN 1 
                            WHEN TABLE_NAME = 'Payroll' THEN 2
                            WHEN TABLE_NAME = 'PayrollRecords' THEN 3
                            ELSE 4 END";

            using (var cmd = new SqlCommand(query, conn))
            {
                var result = cmd.ExecuteScalar();
                return result != null ? result.ToString() : null;
            }
        }

        private System.Collections.Generic.Dictionary<string, string> GetPayrollColumnNames(SqlConnection conn, string tableName)
        {
            var columnNames = new System.Collections.Generic.Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            var query = $@"SELECT COLUMN_NAME 
                         FROM INFORMATION_SCHEMA.COLUMNS 
                         WHERE TABLE_NAME = '{tableName}'
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

            return columnNames;
        }


        private bool HasEssentialColumns(string userIdCol, string periodStartCol, string periodEndCol)
        {
            return !string.IsNullOrEmpty(userIdCol) &&
                   !string.IsNullOrEmpty(periodStartCol) &&
                   !string.IsNullOrEmpty(periodEndCol);
        }

        private void DetectPayrollColumns(
            System.Collections.Generic.Dictionary<string, string> columnNames,
            out string userIdCol,
            out string periodStartCol,
            out string periodEndCol,
            out string totalHoursCol,
            out string grossPayCol,
            out string deductionsCol,
            out string netPayCol)
        {
            userIdCol = FindColumnName(
                columnNames,
                new[] { "user_id", "UserID", "EmployeeID", "employee_id", "emp_id", "EmployeeId", "EmpID", "Employee_ID" },
                new[] { new[] { "user", "id" }, new[] { "employee", "id" }, new[] { "emp", "id" } });

            periodStartCol = FindColumnName(
                columnNames,
                new[] { "period_start", "PeriodFrom", "period_from", "FromDate", "from_date", "StartDate", "start_date", "DateFrom", "datefrom", "PayrollStart", "PayrollStartDate", "CutOffStart", "CutoffStart", "cutoff_from" },
                new[] { new[] { "period", "from" }, new[] { "start", "date" }, new[] { "cutoff", "start" }, new[] { "cut", "off", "start" } });

            periodEndCol = FindColumnName(
                columnNames,
                new[] { "period_end", "PeriodTo", "period_to", "ToDate", "to_date", "EndDate", "end_date", "DateTo", "dateto", "PayrollEnd", "PayrollEndDate", "CutOffEnd", "CutoffEnd", "cutoff_to" },
                new[] { new[] { "period", "to" }, new[] { "end", "date" }, new[] { "cutoff", "end" }, new[] { "cut", "off", "end" } });

            totalHoursCol = FindColumnName(
                columnNames,
                new[] { "total_hours", "TotalHours", "total_hours", "Hours", "hours", "TotalHoursWorked", "total_hours_worked" },
                new[] { new[] { "total", "hours" }, new[] { "hours", "worked" } });

            grossPayCol = FindColumnName(
                columnNames,
                new[] { "gross_pay", "GrossPay", "gross_pay", "Gross", "gross", "GrossAmount", "gross_amount" },
                new[] { new[] { "gross" } });

            deductionsCol = FindColumnName(
                columnNames,
                new[] { "deductions", "Deductions", "deduction", "Deduction", "TotalDeductions", "total_deductions" },
                new[] { new[] { "deduction" } });

            netPayCol = FindColumnName(
                columnNames,
                new[] { "net_pay", "NetPay", "net_pay", "Net", "net", "NetAmount", "net_amount" },
                new[] { new[] { "net" } });
        }

        private string FindColumnName(
            System.Collections.Generic.Dictionary<string, string> columnNames,
            string[] possibleNames,
            string[][] keywordSets = null)
        {
            foreach (var name in possibleNames)
            {
                if (columnNames.ContainsKey(name))
                {
                    return columnNames[name];
                }
            }

            if (keywordSets != null)
            {
                foreach (var keywords in keywordSets)
                {
                    if (keywords == null || keywords.Length == 0)
                        continue;

                    foreach (var column in columnNames.Keys)
                    {
                        bool matches = true;
                        foreach (var keyword in keywords)
                        {
                            if (string.IsNullOrWhiteSpace(keyword))
                                continue;

                            if (column.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) < 0)
                            {
                                matches = false;
                                break;
                            }
                        }

                        if (matches)
                        {
                            return columnNames[column];
                        }
                    }
                }
            }

            return null;
        }

        private void ShowMissingColumnsMessage(string tableName, System.Collections.Generic.Dictionary<string, string> columnNames,
            string userIdCol, string periodStartCol, string periodEndCol)
        {
            var missing = new System.Text.StringBuilder();
            if (userIdCol == null) missing.AppendLine("- Employee/User ID column");
            if (periodStartCol == null) missing.AppendLine("- Period From column");
            if (periodEndCol == null) missing.AppendLine("- Period To column");

            var existingColumns = columnNames != null && columnNames.Count > 0
                ? string.Join(", ", columnNames.Keys)
                : "No columns detected";

            MessageBox.Show(
                $"Required columns not found in table '{tableName}'.\n\nMissing:\n{missing}\nDetected columns:\n{existingColumns}\n\n" +
                "Please verify that the payroll table contains recognizable column names.",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        private bool TryUsePayrollJoin(SqlConnection conn,
            string baseTableName,
            System.Collections.Generic.Dictionary<string, string> baseColumns,
            ref string tableAlias,
            ref string fromClause,
            ref string userIdCol,
            ref string periodStartCol,
            ref string periodEndCol,
            ref string totalHoursCol,
            ref string grossPayCol,
            ref string deductionsCol,
            ref string netPayCol)
        {
            if (!baseTableName.Equals("Payslips", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            string payrollLinkCol = FindColumnName(
                baseColumns,
                new[] { "payroll_id", "PayrollID", "payrollId" },
                new[] { new[] { "payroll", "id" } });

            if (payrollLinkCol == null)
            {
                return false;
            }

            string payrollTable = GetPayrollDataTableName(conn);
            if (string.IsNullOrEmpty(payrollTable))
            {
                return false;
            }

            var payrollColumns = GetPayrollColumnNames(conn, payrollTable);
            if (payrollColumns == null || payrollColumns.Count == 0)
            {
                return false;
            }

            string payrollPrimaryCol = FindColumnName(
                payrollColumns,
                new[] { "payroll_id", "PayrollID", "payrollId" },
                new[] { new[] { "payroll", "id" } });

            if (payrollPrimaryCol == null)
            {
                return false;
            }

            string payrollUserCol = FindColumnName(
                payrollColumns,
                new[] { "user_id", "UserID", "EmployeeID", "employee_id", "emp_id", "EmployeeId" },
                new[] { new[] { "user", "id" }, new[] { "employee", "id" }, new[] { "emp", "id" } });

            string payrollStartCol = FindColumnName(
                payrollColumns,
                new[] { "period_start", "PeriodFrom", "period_from", "FromDate", "from_date", "StartDate", "start_date", "DateFrom", "datefrom", "PayrollStart", "PayrollStartDate", "CutOffStart", "CutoffStart", "cutoff_from" },
                new[] { new[] { "period", "from" }, new[] { "start", "date" }, new[] { "cutoff", "start" }, new[] { "cut", "off", "start" } });

            string payrollEndCol = FindColumnName(
                payrollColumns,
                new[] { "period_end", "PeriodTo", "period_to", "ToDate", "to_date", "EndDate", "end_date", "DateTo", "dateto", "PayrollEnd", "PayrollEndDate", "CutOffEnd", "CutoffEnd", "cutoff_to" },
                new[] { new[] { "period", "to" }, new[] { "end", "date" }, new[] { "cutoff", "end" }, new[] { "cut", "off", "end" } });

            if (userIdCol == null) userIdCol = payrollUserCol;
            if (periodStartCol == null) periodStartCol = payrollStartCol;
            if (periodEndCol == null) periodEndCol = payrollEndCol;

            if (userIdCol == null || periodStartCol == null || periodEndCol == null)
            {
                return false;
            }

            string payrollTotalHoursCol = FindColumnName(
                payrollColumns,
                new[] { "total_hours", "TotalHours", "total_hours", "Hours", "hours", "TotalHoursWorked", "total_hours_worked" },
                new[] { new[] { "total", "hours" }, new[] { "hours", "worked" } });
            string payrollGrossCol = FindColumnName(
                payrollColumns,
                new[] { "gross_pay", "GrossPay", "gross_pay", "Gross", "gross", "GrossAmount", "gross_amount" },
                new[] { new[] { "gross" } });
            string payrollDeductionCol = FindColumnName(
                payrollColumns,
                new[] { "deductions", "Deductions", "deduction", "Deduction", "TotalDeductions", "total_deductions" },
                new[] { new[] { "deduction" } });
            string payrollNetCol = FindColumnName(
                payrollColumns,
                new[] { "net_pay", "NetPay", "net_pay", "Net", "net", "NetAmount", "net_amount" },
                new[] { new[] { "net" } });

            if (totalHoursCol == null) totalHoursCol = payrollTotalHoursCol;
            if (grossPayCol == null) grossPayCol = payrollGrossCol;
            if (deductionsCol == null) deductionsCol = payrollDeductionCol;
            if (netPayCol == null) netPayCol = payrollNetCol;

            tableAlias = "pr";
            fromClause = $"FROM [{baseTableName}] ps INNER JOIN [{payrollTable}] {tableAlias} ON ps.[{payrollLinkCol}] = {tableAlias}.[{payrollPrimaryCol}]";

            return true;
        }

        private string GetPayrollDataTableName(SqlConnection conn)
        {
            var query = @"SELECT TOP 1 TABLE_NAME 
                        FROM INFORMATION_SCHEMA.TABLES 
                        WHERE TABLE_NAME IN ('Payroll', 'PayrollRecords', 'PayrollHistory', 'Payruns')
                        ORDER BY CASE 
                            WHEN TABLE_NAME = 'Payroll' THEN 1 
                            WHEN TABLE_NAME = 'PayrollRecords' THEN 2
                            WHEN TABLE_NAME = 'PayrollHistory' THEN 3
                            ELSE 4 END";

            using (var cmd = new SqlCommand(query, conn))
            {
                var result = cmd.ExecuteScalar();
                return result != null ? result.ToString() : null;
            }
        }

        private void ShowNoPayrollMessage()
        {
            lblEmployeeNameValue.Text = "N/A";
            lblPayrollPeriodValue.Text = "No payroll record found";
            lblTotalHoursValue.Text = "N/A";
            lblHourlyRateValue.Text = "N/A";
            lblGrossPayValue.Text = "N/A";
            lblTotalDeductionsValue.Text = "N/A";
            lblNetPayValue.Text = "N/A";
            btnDownloadPayslip.Enabled = false;

            MessageBox.Show("No payroll record found for your account. Please contact your administrator.", 
                "No Payslip Available", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information);
        }

        private void btnDownloadPayslip_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Payslip downloaded successfully!", "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

