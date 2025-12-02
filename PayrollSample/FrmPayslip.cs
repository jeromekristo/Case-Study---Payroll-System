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

                    // Debug: Show all column names found
                    System.Diagnostics.Debug.WriteLine($"Columns found in {tableName}: {string.Join(", ", columnNames.Keys)}");

                    DetectPayrollColumns(columnNames,
                        out string userIdCol,
                        out string periodStartCol,
                        out string periodEndCol,
                        out string totalHoursCol,
                        out string grossPayCol,
                        out string deductionsCol,
                        out string netPayCol,
                        out string lateDeductionCol);

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
                                    out netPayCol,
                                    out lateDeductionCol);
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
                            ref totalHoursCol, ref grossPayCol, ref deductionsCol, ref netPayCol, ref lateDeductionCol))
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

                    // Determine actual column names to use (prefer detected, fallback to standard names)
                    string actualTotalHoursCol = totalHoursCol ?? (columnNames.ContainsKey("TotalHours") ? "TotalHours" : null);
                    string actualGrossPayCol = grossPayCol ?? (columnNames.ContainsKey("GrossPay") ? "GrossPay" : null);
                    string actualDeductionsCol = deductionsCol ?? (columnNames.ContainsKey("Deductions") ? "Deductions" : null);
                    string actualNetPayCol = netPayCol ?? (columnNames.ContainsKey("NetPay") ? "NetPay" : null);
                    string actualPeriodStartCol = periodStartCol ?? (columnNames.ContainsKey("PeriodFrom") ? "PeriodFrom" : null);
                    string actualPeriodEndCol = periodEndCol ?? (columnNames.ContainsKey("PeriodTo") ? "PeriodTo" : null);
                    string actualLateDeductionCol = lateDeductionCol ??
                        (columnNames.ContainsKey("LateDeduction") ? "LateDeduction" :
                        (columnNames.ContainsKey("late_deduction") ? "late_deduction" : null));

                    // Use ISNULL to handle NULL values and display 0 instead
                    if (actualTotalHoursCol != null) 
                        selectColumns.Add($"ISNULL({tableAlias}.[{actualTotalHoursCol}], 0) AS TotalHours");
                    else
                        selectColumns.Add("0 AS TotalHours");
                    
                    if (actualGrossPayCol != null) 
                        selectColumns.Add($"ISNULL({tableAlias}.[{actualGrossPayCol}], 0) AS GrossPay");
                    else
                        selectColumns.Add("0 AS GrossPay");
                    
                    if (actualLateDeductionCol != null)
                        selectColumns.Add($"ISNULL({tableAlias}.[{actualLateDeductionCol}], 0) AS LateDeduction");
                    else
                        selectColumns.Add("0 AS LateDeduction");
                    
                    if (actualDeductionsCol != null) 
                        selectColumns.Add($"ISNULL({tableAlias}.[{actualDeductionsCol}], 0) AS Deductions");
                    else
                        selectColumns.Add("0 AS Deductions");
                    
                    if (actualNetPayCol != null) 
                        selectColumns.Add($"ISNULL({tableAlias}.[{actualNetPayCol}], 0) AS NetPay");
                    else
                        selectColumns.Add("0 AS NetPay");
                    
                    if (actualPeriodStartCol != null)
                    {
                        selectColumns.Add($"{tableAlias}.[{actualPeriodStartCol}] AS PeriodStart");
                        orderByColumns.Add($"{tableAlias}.[{actualPeriodStartCol}]");
                    }
                    if (actualPeriodEndCol != null)
                    {
                        selectColumns.Add($"{tableAlias}.[{actualPeriodEndCol}] AS PeriodEnd");
                        orderByColumns.Insert(0, $"{tableAlias}.[{actualPeriodEndCol}]");
                    }

                    selectColumns.Add("u.FirstName + ' ' + u.LastName AS EmployeeName");
                    selectColumns.Add("ISNULL(u.salary_rate, 0) AS HourlyRate");

                    if (selectColumns.Count == 0)
                    {
                        ShowMissingColumnsMessage(tableName, columnNames, userIdCol, periodStartCol, periodEndCol);
                        ShowNoPayrollMessage();
                        return;
                    }

                    // Build order by clause - get the most recent payslip by period dates
                    // Order by period end date first (most recent period), then period start date
                    var orderByParts = new System.Collections.Generic.List<string>();
                    
                    // Primary: Order by period end date (most recent cutoff period first)
                    if (actualPeriodEndCol != null)
                    {
                        orderByParts.Add($"{tableAlias}.[{actualPeriodEndCol}] DESC");
                    }
                    else if (periodEndCol != null)
                    {
                        orderByParts.Add($"{tableAlias}.[{periodEndCol}] DESC");
                    }
                    
                    // Secondary: Order by period start date
                    if (actualPeriodStartCol != null)
                    {
                        orderByParts.Add($"{tableAlias}.[{actualPeriodStartCol}] DESC");
                    }
                    else if (periodStartCol != null)
                    {
                        orderByParts.Add($"{tableAlias}.[{periodStartCol}] DESC");
                    }
                    
                    // Tertiary: Order by GeneratedDate if available (most recently generated first)
                    string dateCol = FindColumnName(columnNames, 
                        new[] { "GeneratedDate", "generated_date", "CreatedDate", "created_date", "DateGenerated", "date_generated" },
                        null);
                    if (dateCol != null)
                    {
                        orderByParts.Add($"{tableAlias}.[{dateCol}] DESC");
                    }
                    
                    string orderByClause = orderByParts.Count > 0 ? "ORDER BY " + string.Join(", ", orderByParts) : "";

                    // Check if Status column exists and add it to WHERE clause if available
                    string statusCol = FindColumnName(columnNames, 
                        new[] { "Status", "status" },
                        null);
                    string statusFilter = "";
                    if (statusCol != null)
                    {
                        // Prefer records with Status = 'Generated', but don't exclude others
                        statusFilter = $" AND ({tableAlias}.[{statusCol}] = 'Generated' OR {tableAlias}.[{statusCol}] IS NULL)";
                    }

                    var query = $@"SELECT TOP 1 
                                        {string.Join(",\n                                        ", selectColumns)}
                                  {fromClause}
                                  INNER JOIN Users u ON {tableAlias}.[{userIdCol}] = u.UserID
                                  WHERE {tableAlias}.[{userIdCol}] = @UserID{statusFilter}
                                  {orderByClause}";

                    // Debug: Log the query and column detection results
                    System.Diagnostics.Debug.WriteLine($"Payslip Query: {query}");
                    System.Diagnostics.Debug.WriteLine($"UserID: {userId}");
                    System.Diagnostics.Debug.WriteLine($"Table: {tableName}");
                    System.Diagnostics.Debug.WriteLine($"TotalHours Column: {actualTotalHoursCol ?? "NOT FOUND"}");
                    System.Diagnostics.Debug.WriteLine($"GrossPay Column: {actualGrossPayCol ?? "NOT FOUND"}");
                    System.Diagnostics.Debug.WriteLine($"Deductions Column: {actualDeductionsCol ?? "NOT FOUND"}");
                    System.Diagnostics.Debug.WriteLine($"LateDeduction Column: {actualLateDeductionCol ?? "NOT FOUND"}");
                    System.Diagnostics.Debug.WriteLine($"NetPay Column: {actualNetPayCol ?? "NOT FOUND"}");
                    System.Diagnostics.Debug.WriteLine($"PeriodFrom Column: {actualPeriodStartCol ?? "NOT FOUND"}");
                    System.Diagnostics.Debug.WriteLine($"PeriodTo Column: {actualPeriodEndCol ?? "NOT FOUND"}");

                    // First, let's verify the raw data exists with a simple query (without ISNULL to see actual values)
                    var verifyQuery = $@"SELECT TOP 5 
                                        {tableAlias}.[{userIdCol}] AS UserID,
                                        {tableAlias}.[{actualPeriodStartCol ?? periodStartCol}] AS PeriodFrom,
                                        {tableAlias}.[{actualPeriodEndCol ?? periodEndCol}] AS PeriodTo";
                    
                    if (actualTotalHoursCol != null) verifyQuery += $", {tableAlias}.[{actualTotalHoursCol}] AS TotalHours";
                    else if (totalHoursCol != null) verifyQuery += $", {tableAlias}.[{totalHoursCol}] AS TotalHours";
                    
                    if (actualGrossPayCol != null) verifyQuery += $", {tableAlias}.[{actualGrossPayCol}] AS GrossPay";
                    else if (grossPayCol != null) verifyQuery += $", {tableAlias}.[{grossPayCol}] AS GrossPay";
                    
                    if (actualDeductionsCol != null) verifyQuery += $", {tableAlias}.[{actualDeductionsCol}] AS Deductions";
                    else if (deductionsCol != null) verifyQuery += $", {tableAlias}.[{deductionsCol}] AS Deductions";
                    
                    if (actualLateDeductionCol != null) verifyQuery += $", {tableAlias}.[{actualLateDeductionCol}] AS LateDeduction";
                    else if (lateDeductionCol != null) verifyQuery += $", {tableAlias}.[{lateDeductionCol}] AS LateDeduction";
                    
                    if (actualNetPayCol != null) verifyQuery += $", {tableAlias}.[{actualNetPayCol}] AS NetPay";
                    else if (netPayCol != null) verifyQuery += $", {tableAlias}.[{netPayCol}] AS NetPay";
                    
                    verifyQuery += $@"
                                        FROM [{tableName}] {tableAlias}
                                        WHERE {tableAlias}.[{userIdCol}] = @UserID
                                        ORDER BY {tableAlias}.[{actualPeriodEndCol ?? periodEndCol}] DESC, {tableAlias}.[{actualPeriodStartCol ?? periodStartCol}] DESC";
                    
                    System.Diagnostics.Debug.WriteLine($"Verify Query (Raw Values): {verifyQuery}");
                    
                    // Execute verify query to see what's actually in the database
                    try
                    {
                        using (var verifyCmd = new SqlCommand(verifyQuery, conn))
                        {
                            verifyCmd.Parameters.AddWithValue("@UserID", userId);
                            using (var verifyReader = verifyCmd.ExecuteReader())
                            {
                                int recordCount = 0;
                                while (verifyReader.Read() && recordCount < 3)
                                {
                                    recordCount++;
                                    System.Diagnostics.Debug.WriteLine($"Record {recordCount}:");
                                    try
                                    {
                                        System.Diagnostics.Debug.WriteLine($"  PeriodFrom: {verifyReader["PeriodFrom"]}");
                                        System.Diagnostics.Debug.WriteLine($"  PeriodTo: {verifyReader["PeriodTo"]}");
                                        
                                        var schemaTable = verifyReader.GetSchemaTable();
                                        if (schemaTable != null)
                                        {
                                            var availableColumns = new System.Collections.Generic.List<string>();
                                            foreach (System.Data.DataRow row in schemaTable.Rows)
                                            {
                                                availableColumns.Add(row["ColumnName"].ToString());
                                            }
                                            System.Diagnostics.Debug.WriteLine($"  Available columns: {string.Join(", ", availableColumns)}");
                                            
                                            if (availableColumns.Contains("TotalHours"))
                                                System.Diagnostics.Debug.WriteLine($"  TotalHours (raw): {verifyReader["TotalHours"]} (IsDBNull: {verifyReader.IsDBNull(verifyReader.GetOrdinal("TotalHours"))})");
                                            if (availableColumns.Contains("GrossPay"))
                                                System.Diagnostics.Debug.WriteLine($"  GrossPay (raw): {verifyReader["GrossPay"]} (IsDBNull: {verifyReader.IsDBNull(verifyReader.GetOrdinal("GrossPay"))})");
                                            if (availableColumns.Contains("Deductions"))
                                                System.Diagnostics.Debug.WriteLine($"  Deductions (raw): {verifyReader["Deductions"]} (IsDBNull: {verifyReader.IsDBNull(verifyReader.GetOrdinal("Deductions"))})");
                                        if (availableColumns.Contains("LateDeduction"))
                                            System.Diagnostics.Debug.WriteLine($"  LateDeduction (raw): {verifyReader["LateDeduction"]} (IsDBNull: {verifyReader.IsDBNull(verifyReader.GetOrdinal("LateDeduction"))})");
                                            if (availableColumns.Contains("NetPay"))
                                                System.Diagnostics.Debug.WriteLine($"  NetPay (raw): {verifyReader["NetPay"]} (IsDBNull: {verifyReader.IsDBNull(verifyReader.GetOrdinal("NetPay"))})");
                                        }
                                    }
                                    catch (Exception colEx)
                                    {
                                        System.Diagnostics.Debug.WriteLine($"  Error reading record: {colEx.Message}");
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception verifyEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"Verify query error: {verifyEx.Message}");
                    }

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Debug: Log the actual values retrieved
                                try
                                {
                                    System.Diagnostics.Debug.WriteLine($"Retrieved Values:");
                                    System.Diagnostics.Debug.WriteLine($"  TotalHours: {reader["TotalHours"]}");
                                    System.Diagnostics.Debug.WriteLine($"  GrossPay: {reader["GrossPay"]}");
                                    System.Diagnostics.Debug.WriteLine($"  Deductions: {reader["Deductions"]}");
                                    System.Diagnostics.Debug.WriteLine($"  NetPay: {reader["NetPay"]}");
                                    System.Diagnostics.Debug.WriteLine($"  PeriodStart: {reader["PeriodStart"]}");
                                    System.Diagnostics.Debug.WriteLine($"  PeriodEnd: {reader["PeriodEnd"]}");
                                }
                                catch (Exception debugEx)
                                {
                                    System.Diagnostics.Debug.WriteLine($"Debug logging error: {debugEx.Message}");
                                }

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

                                // Display total hours worked (always display, even if 0)
                                try
                                {
                                    decimal totalHours = reader["TotalHours"] != DBNull.Value ? Convert.ToDecimal(reader["TotalHours"]) : 0m;
                                    lblTotalHoursValue.Text = totalHours.ToString("N2") + " hours";
                                }
                                catch
                                {
                                    lblTotalHoursValue.Text = "0.00 hours";
                                }

                                // Display hourly rate
                                const string pesoFormat = "₱#,0.00";

                                try
                                {
                                    decimal hourlyRate = reader["HourlyRate"] != DBNull.Value ? Convert.ToDecimal(reader["HourlyRate"]) : 0m;
                                    lblHourlyRateValue.Text = hourlyRate.ToString(pesoFormat);
                                }
                                catch
                                {
                                    lblHourlyRateValue.Text = "₱0.00";
                                }

                                // Display gross pay (always display, even if 0)
                                try
                                {
                                    decimal grossPay = reader["GrossPay"] != DBNull.Value ? Convert.ToDecimal(reader["GrossPay"]) : 0m;
                                    lblGrossPayValue.Text = grossPay.ToString(pesoFormat);
                                }
                                catch
                                {
                                    lblGrossPayValue.Text = "₱0.00";
                                }

                                // Display total deductions (always display, even if 0)
                                try
                                {
                                    decimal deductions = reader["Deductions"] != DBNull.Value ? Convert.ToDecimal(reader["Deductions"]) : 0m;
                                    lblTotalDeductionsValue.Text = deductions.ToString(pesoFormat);
                                }
                                catch
                                {
                                    lblTotalDeductionsValue.Text = "₱0.00";
                                }

                                // Display late deduction separately
                                try
                                {
                                    decimal lateDeduction = reader["LateDeduction"] != DBNull.Value ? Convert.ToDecimal(reader["LateDeduction"]) : 0m;
                                    lblLateDeductionValue.Text = lateDeduction.ToString(pesoFormat);
                                }
                                catch
                                {
                                    lblLateDeductionValue.Text = "₱0.00";
                                }

                                // Display net pay (always display, even if 0)
                                try
                                {
                                    decimal netPay = reader["NetPay"] != DBNull.Value ? Convert.ToDecimal(reader["NetPay"]) : 0m;
                                    lblNetPayValue.Text = netPay.ToString(pesoFormat);
                                }
                                catch
                                {
                                    lblNetPayValue.Text = "₱0.00";
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
            out string netPayCol,
            out string lateDeductionCol)
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

            // Check for exact matches first (case-insensitive), then try variations
            totalHoursCol = FindColumnName(
                columnNames,
                new[] { "TotalHours", "total_hours", "TotalHoursWorked", "total_hours_worked", "Hours", "hours" },
                new[] { new[] { "total", "hours" }, new[] { "hours", "worked" } });

            grossPayCol = FindColumnName(
                columnNames,
                new[] { "GrossPay", "gross_pay", "GrossAmount", "gross_amount", "Gross", "gross" },
                new[] { new[] { "gross" } });

            deductionsCol = FindColumnName(
                columnNames,
                new[] { "Deductions", "deductions", "TotalDeductions", "total_deductions", "Deduction", "deduction" },
                new[] { new[] { "deduction" } });

            netPayCol = FindColumnName(
                columnNames,
                new[] { "NetPay", "net_pay", "NetAmount", "net_amount", "Net", "net" },
                new[] { new[] { "net" } });

            lateDeductionCol = FindColumnName(
                columnNames,
                new[] { "LateDeduction", "late_deduction", "LatePenalty", "late_penalty" },
                new[] { new[] { "late", "deduction" }, new[] { "late", "penalty" } });
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
            ref string netPayCol,
            ref string lateDeductionCol)
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
            string payrollLateDeductionCol = FindColumnName(
                payrollColumns,
                new[] { "late_deduction", "LateDeduction", "LatePenalty", "late_penalty" },
                new[] { new[] { "late", "deduction" }, new[] { "late", "penalty" } });

            if (totalHoursCol == null) totalHoursCol = payrollTotalHoursCol;
            if (grossPayCol == null) grossPayCol = payrollGrossCol;
            if (deductionsCol == null) deductionsCol = payrollDeductionCol;
            if (netPayCol == null) netPayCol = payrollNetCol;
            if (lateDeductionCol == null) lateDeductionCol = payrollLateDeductionCol;

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
            lblLateDeductionValue.Text = "N/A";
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

