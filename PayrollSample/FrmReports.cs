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
                    @"SELECT user_id, first_name, last_name
                      FROM users
                      WHERE role = 'Employee'
                      ORDER BY last_name, first_name;", conn))
                {
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        int userIdOrdinal = reader.GetOrdinal("user_id");
                        int firstNameOrdinal = reader.GetOrdinal("first_name");
                        int lastNameOrdinal = reader.GetOrdinal("last_name");

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
                            u.first_name,
                            u.last_name,
                            a.date,
                            a.time_in,
                            a.time_out,
                            a.hours_worked
                          FROM attendance a
                          JOIN users u ON a.user_id = u.user_id
                          WHERE a.date BETWEEN @startDate AND @endDate
                            AND (u.user_id = @userId OR @userId = 0);";
                    cmd.Parameters.Add("@startDate", SqlDbType.Date).Value = dtFrom.Value.Date;
                    cmd.Parameters.Add("@endDate", SqlDbType.Date).Value = dtTo.Value.Date;
                    cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
                    break;

                case "Payroll Summary Report":
                    cmd.CommandText =
                        @"SELECT
                            p.payroll_id,
                            u.first_name,
                            u.last_name,
                            p.cutoff_start,
                            p.cutoff_end,
                            p.total_hours,
                            p.gross_pay,
                            p.total_deductions,
                            p.net_pay
                          FROM payroll p
                          JOIN users u ON p.user_id = u.user_id
                          WHERE p.cutoff_start >= @startDate
                            AND p.cutoff_end <= @endDate
                            AND (u.user_id = @userId OR @userId = 0);";
                    cmd.Parameters.Add("@startDate", SqlDbType.Date).Value = dtFrom.Value.Date;
                    cmd.Parameters.Add("@endDate", SqlDbType.Date).Value = dtTo.Value.Date;
                    cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
                    break;

                case "Employee Report":
                    cmd.CommandText =
                        @"SELECT 
                            user_id,
                            first_name,
                            last_name,
                            role,
                            salary_type,
                            salary_rate,
                            status
                          FROM users;";
                    break;

                case "Deduction Report":
                    cmd.CommandText =
                        @"SELECT 
                            d.deduction_name,
                            d.amount,
                            d.is_percentage,
                            COUNT(pd.payroll_id) AS times_applied,
                            SUM(pd.deduction_amount) AS total_deducted
                          FROM deductions d
                          LEFT JOIN payroll_deductions pd 
                                 ON d.deduction_id = pd.deduction_id
                          GROUP BY d.deduction_id;";
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
            MessageBox.Show("PDF export feature coming soon.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Excel export feature coming soon.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private sealed class EmployeeItem
        {
            public int UserId { get; set; }
            public string DisplayName { get; set; }
        }
    }
}


