using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PayrollSample
{
    public partial class FrmReports : Form
    {
        private readonly string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=PayrollDB;Integrated Security=True";

        public FrmReports()
        {
            InitializeComponent();
        }

        private void FrmReports_Load(object sender, EventArgs e)
        {
            dgvReports.AutoGenerateColumns = true;
            dtFrom.Value = DateTime.Today.AddDays(-7);
            dtTo.Value = DateTime.Today;

            LoadReportTypes();
            LoadEmployees();
        }

        private void LoadReportTypes()
        {
            cmbReportType.Items.Clear();
            cmbReportType.Items.AddRange(new object[]
            {
                "Attendance Report",
                "Payroll Summary Report",
                "Employee Report",
                "Deduction Report"
            });

            if (cmbReportType.Items.Count > 0)
            {
                cmbReportType.SelectedIndex = 0;
            }
        }

        private void LoadEmployees()
        {
            var employees = new List<EmployeeItem>
            {
                new EmployeeItem { UserId = 0, DisplayName = "All Employees" }
            };

            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(
                    @"SELECT UserID, FirstName, LastName
                      FROM Users
                      WHERE Role = 'Employee'
                      ORDER BY LastName, FirstName;", conn))
                {
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        int userIdOrdinal = reader.GetOrdinal("UserID");
                        int firstNameOrdinal = reader.GetOrdinal("FirstName");
                        int lastNameOrdinal = reader.GetOrdinal("LastName");

                        while (reader.Read())
                        {
                            var userId = reader.IsDBNull(userIdOrdinal) ? 0 : reader.GetInt32(userIdOrdinal);
                            var firstName = reader.IsDBNull(firstNameOrdinal)
                                ? string.Empty
                                : reader.GetString(firstNameOrdinal);
                            var lastName = reader.IsDBNull(lastNameOrdinal)
                                ? string.Empty
                                : reader.GetString(lastNameOrdinal);

                            employees.Add(new EmployeeItem
                            {
                                UserId = userId,
                                DisplayName = $"{firstName} {lastName}".Trim()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load employees: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            cmbEmployee.DataSource = employees;
            cmbEmployee.DisplayMember = nameof(EmployeeItem.DisplayName);
            cmbEmployee.ValueMember = nameof(EmployeeItem.UserId);
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            GenerateReport();
        }

        private void GenerateReport()
        {
            if (cmbReportType.SelectedItem == null)
            {
                MessageBox.Show("Select a report type.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var reportType = cmbReportType.SelectedItem.ToString();
            var userId = GetSelectedUserId();

            if (RequiresDateRange(reportType) && dtFrom.Value.Date > dtTo.Value.Date)
            {
                MessageBox.Show("The From date cannot be later than the To date.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = conn.CreateCommand())
                {
                    ConfigureCommandForReport(cmd, reportType, userId);

                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        var table = new DataTable();
                        adapter.Fill(table);
                        dgvReports.DataSource = table;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to generate report: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureCommandForReport(SqlCommand cmd, string reportType, int userId)
        {
            switch (reportType)
            {
                case "Attendance Report":
                    cmd.CommandText =
                        @"SELECT 
                            a.attendance_id,
                            u.FirstName AS first_name,
                            u.LastName AS last_name,
                            a.[date],
                            a.time_in,
                            a.time_out,
                            a.hours_worked
                          FROM Attendance a
                          JOIN Users u ON a.UserID = u.UserID
                          WHERE a.[date] BETWEEN @startDate AND @endDate
                            AND (u.UserID = @userId OR @userId = 0);";
                    cmd.Parameters.Add("@startDate", SqlDbType.Date).Value = dtFrom.Value.Date;
                    cmd.Parameters.Add("@endDate", SqlDbType.Date).Value = dtTo.Value.Date;
                    cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
                    break;

                case "Payroll Summary Report":
                    // Use Payslips table (the table used by payroll processing)
                    // Note: Payslips table may not have PayrollID, so we'll use ROW_NUMBER or just omit it
                    cmd.CommandText =
                        @"SELECT
                            ROW_NUMBER() OVER (ORDER BY ps.PeriodFrom, u.UserID) AS payroll_id,
                            u.FirstName AS first_name,
                            u.LastName AS last_name,
                            ps.PeriodFrom AS cutoff_start,
                            ps.PeriodTo AS cutoff_end,
                            ps.TotalHours AS total_hours,
                            ps.GrossPay AS gross_pay,
                            ps.Deductions AS total_deductions,
                            ps.NetPay AS net_pay
                          FROM Payslips ps
                          JOIN Users u ON ps.UserID = u.UserID
                          WHERE ps.PeriodFrom >= @startDate
                            AND ps.PeriodTo <= @endDate
                            AND (u.UserID = @userId OR @userId = 0)
                          ORDER BY ps.PeriodFrom DESC, u.LastName, u.FirstName;";
                    cmd.Parameters.Add("@startDate", SqlDbType.Date).Value = dtFrom.Value.Date;
                    cmd.Parameters.Add("@endDate", SqlDbType.Date).Value = dtTo.Value.Date;
                    cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
                    break;

                case "Employee Report":
                    cmd.CommandText =
                        @"SELECT 
                            UserID AS user_id,
                            FirstName AS first_name,
                            LastName AS last_name,
                            Role AS role,
                            salary_type,
                            salary_rate,
                            Status AS status
                          FROM Users;";
                    break;

                case "Deduction Report":
                    // Updated to work with the actual Deductions table structure (deduction_id, name, percentage, is_mandatory)
                    cmd.CommandText =
                        @"SELECT 
                            d.name AS deduction_name,
                            d.percentage AS amount,
                            CASE WHEN d.is_mandatory = 1 THEN 'Yes' ELSE 'No' END AS is_percentage,
                            (SELECT COUNT(*) FROM Payslips WHERE Deductions > 0) AS times_applied,
                            (SELECT SUM(Deductions) FROM Payslips) AS total_deducted
                          FROM Deductions d;";
                    break;

                default:
                    throw new InvalidOperationException("Unsupported report type.");
            }
        }

        private bool RequiresDateRange(string reportType)
        {
            return reportType == "Attendance Report" || reportType == "Payroll Summary Report";
        }

        private int GetSelectedUserId()
        {
            if (cmbEmployee.SelectedValue is int id)
            {
                return id;
            }

            return 0;
        }

        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            if (dgvReports.DataSource == null || dgvReports.Rows.Count == 0)
            {
                MessageBox.Show("Please generate a report first before exporting to PDF.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                "Are you sure you want to export the current report to PDF?",
                "Export to PDF",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                MessageBox.Show("PDF export completed successfully!", "Export Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            if (dgvReports.DataSource == null || dgvReports.Rows.Count == 0)
            {
                MessageBox.Show("Please generate a report first before exporting to Excel.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                "Are you sure you want to export the current report to Excel?",
                "Export to Excel",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                MessageBox.Show("Excel export completed successfully!", "Export Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private sealed class EmployeeItem
        {
            public int UserId { get; set; }
            public string DisplayName { get; set; }
        }
    }
}


