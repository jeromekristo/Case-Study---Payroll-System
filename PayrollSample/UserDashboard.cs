using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PayrollSample
{
    public partial class UserDashboard : Form
    {
        private string username;
        private int userId;
        private readonly string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=PayrollDB;Integrated Security=True";

        public UserDashboard(string username)
        {
            InitializeComponent();
            this.username = username;
            lblWelcome.Text = "Welcome, " + username + "!";
        }

        private void btnViewAttendance_Click(object sender, EventArgs e)
        {
            LoadAttendanceHistory();
        }

        private void btnViewPayslip_Click(object sender, EventArgs e)
        {
            // Placeholder - will be implemented later
            MessageBox.Show("View Payslip feature coming soon!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnProfileSettings_Click(object sender, EventArgs e)
        {
            using (var profileForm = new ProfileSettings(username))
            {
                profileForm.StartPosition = FormStartPosition.CenterParent;
                profileForm.ShowDialog(this);
            }
        }

        private void UserDashboard_Load(object sender, EventArgs e)
        {
            try
            {
                userId = GetUserIdByUsername(username);

                if (userId == 0)
                {
                    MessageBox.Show("Unable to retrieve user information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnTimeIn.Enabled = false;
                    btnTimeOut.Enabled = false;
                    return;
                }

                RefreshTodayAttendance();
                LoadAttendanceHistory();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load attendance information: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult != DialogResult.Yes)
            {
                return;
            }

            // Find and show the login form
            Form1 loginForm = null;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Form1)
                {
                    loginForm = form as Form1;
                    break;
                }
            }
            
            // If login form not found, create a new one
            if (loginForm == null)
            {
                loginForm = new Form1();
            }
            
            // Show the login form and clear the password field
            loginForm.Show();
            loginForm.BringToFront();
            
            // Close the dashboard
            this.Close();
        }

        private void lblWelcome_Click(object sender, EventArgs e)
        {

        }

        private int GetUserIdByUsername(string username)
        {
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("SELECT UserID FROM Users WHERE Username = @Username", conn))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                conn.Open();
                var result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        private void RefreshTodayAttendance()
        {
            if (userId == 0)
            {
                lblTodayStatus.Text = "User information unavailable.";
                return;
            }

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(@"SELECT TOP 1 attendance_id,
                                                     time_in,
                                                     time_out,
                                                     CASE 
                                                         WHEN time_in IS NOT NULL AND time_out IS NOT NULL 
                                                             THEN CAST(DATEDIFF(SECOND, time_in, time_out) / 3600.0 AS DECIMAL(10,2))
                                                         ELSE NULL
                                                     END AS HoursWorked
                                              FROM Attendance 
                                              WHERE UserID = @UserID AND [date] = @Date
                                              ORDER BY time_in DESC", conn))
            {
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@Date", DateTime.Today);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        lblTodayStatus.Text = "Latest attendance entry for today:";
                        lblTimeInValue.Text = FormatTime(reader["time_in"]);
                        lblTimeOutValue.Text = FormatTime(reader["time_out"]);
                        lblHoursWorkedValue.Text = FormatHoursWorked(reader["HoursWorked"]);
                    }
                    else
                    {
                        lblTodayStatus.Text = "No attendance record yet for today.";
                        lblTimeInValue.Text = "--:--";
                        lblTimeOutValue.Text = "--:--";
                        lblHoursWorkedValue.Text = "0.00 hrs";
                    }
                }
            }
        }

        private void LoadAttendanceHistory()
        {
            if (userId == 0)
            {
                MessageBox.Show("User information unavailable. Cannot load attendance history.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(@"SELECT [date] AS AttendanceDate,
                                                         time_in AS TimeIn,
                                                         time_out AS TimeOut,
                                                         CASE 
                                                             WHEN time_in IS NOT NULL AND time_out IS NOT NULL 
                                                                 THEN CAST(DATEDIFF(SECOND, time_in, time_out) / 3600.0 AS DECIMAL(10,2))
                                                             ELSE NULL
                                                         END AS HoursWorked
                                                  FROM Attendance
                                                  WHERE UserID = @UserID
                                                  ORDER BY [date] DESC, time_in DESC", conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        var table = new DataTable();
                        adapter.Fill(table);

                        var displayTable = new DataTable();
                        displayTable.Columns.Add("Date", typeof(string));
                        displayTable.Columns.Add("Time In", typeof(string));
                        displayTable.Columns.Add("Time Out", typeof(string));
                        displayTable.Columns.Add("Hours Worked", typeof(string));

                        foreach (DataRow row in table.Rows)
                        {
                            displayTable.Rows.Add(
                                FormatDate(row["AttendanceDate"]),
                                FormatTime(row["TimeIn"]),
                                FormatTime(row["TimeOut"]),
                                FormatHoursWorked(row["HoursWorked"]));
                        }

                        dgvAttendanceHistory.AutoGenerateColumns = true;
                        dgvAttendanceHistory.DataSource = displayTable;

                        if (table.Rows.Count == 0)
                        {
                            lblHistoryStatus.Text = "No attendance history yet.";
                        }
                        else
                        {
                            lblHistoryStatus.Text = $"Showing {table.Rows.Count} attendance record(s).";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblHistoryStatus.Text = "Failed to load attendance history.";
                MessageBox.Show("Failed to load attendance history: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string FormatHoursWorked(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return "0.00 hrs";
            }

            decimal hours;

            if (value is decimal decimalValue)
            {
                hours = decimalValue;
            }
            else if (value is double doubleValue)
            {
                hours = Convert.ToDecimal(doubleValue);
            }
            else if (value is float floatValue)
            {
                hours = Convert.ToDecimal(floatValue);
            }
            else if (value is TimeSpan timeSpanValue)
            {
                hours = Convert.ToDecimal(Math.Round(timeSpanValue.TotalHours, 2, MidpointRounding.AwayFromZero));
            }
            else if (!decimal.TryParse(value.ToString(), out hours))
            {
                return value.ToString();
            }

            return hours.ToString("0.00") + " hrs";
        }

        private string FormatTime(object value)
        {
            var dateTimeValue = ConvertSqlTimeToDateTime(value);
            return dateTimeValue.HasValue ? dateTimeValue.Value.ToString("hh:mm tt") : "--:--";
        }

        private string FormatDate(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return "--";
            }

            if (value is DateTime dateValue)
            {
                return dateValue.ToString("MMM dd, yyyy");
            }

            if (DateTime.TryParse(value.ToString(), out DateTime parsedDate))
            {
                return parsedDate.ToString("MMM dd, yyyy");
            }

            return value.ToString();
        }

        private DateTime? ConvertSqlTimeToDateTime(object value, DateTime? dateForTime = null)
        {
            if (value == null || value == DBNull.Value)
            {
                return null;
            }

            if (value is DateTime dateTimeValue)
            {
                return dateTimeValue;
            }

            if (value is TimeSpan timeSpanValue)
            {
                return (dateForTime ?? DateTime.Today).Date.Add(timeSpanValue);
            }

            if (DateTime.TryParse(value.ToString(), out DateTime parsedDateTime))
            {
                return parsedDateTime;
            }

            return null;
        }

        private void btnTimeIn_Click(object sender, EventArgs e)
        {
            if (userId == 0)
            {
                MessageBox.Show("User information unavailable. Cannot time in.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    int? openAttendanceId = null;
                    DateTime? openTimeIn = null;

                    using (var checkCmd = new SqlCommand(@"SELECT TOP 1 attendance_id, time_in 
                                                           FROM Attendance 
                                                           WHERE UserID = @UserID AND [date] = @Date AND time_out IS NULL
                                                           ORDER BY time_in DESC", conn))
                    {
                        checkCmd.Parameters.AddWithValue("@UserID", userId);
                        checkCmd.Parameters.AddWithValue("@Date", DateTime.Today);

                        using (var reader = checkCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                openAttendanceId = Convert.ToInt32(reader["attendance_id"]);
                                openTimeIn = ConvertSqlTimeToDateTime(reader["time_in"], DateTime.Today);
                            }
                        }
                    }

                    if (openAttendanceId.HasValue)
                    {
                        var openSinceText = openTimeIn.HasValue ? openTimeIn.Value.ToString("hh:mm tt") : "earlier";
                        MessageBox.Show("You already have an active attendance entry since " + openSinceText + ". Please time out first.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    DateTime now = DateTime.Now;

                    using (var insertCmd = new SqlCommand(@"INSERT INTO Attendance (UserID, [date], time_in) 
                                                            VALUES (@UserID, @Date, @TimeIn)", conn))
                    {
                        insertCmd.Parameters.AddWithValue("@UserID", userId);
                        insertCmd.Parameters.AddWithValue("@Date", DateTime.Today);
                        var timeParam = insertCmd.Parameters.Add("@TimeIn", SqlDbType.Time);
                        timeParam.Value = now.TimeOfDay;
                        insertCmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Time in recorded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshTodayAttendance();
                LoadAttendanceHistory();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to record time in: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTimeOut_Click(object sender, EventArgs e)
        {
            if (userId == 0)
            {
                MessageBox.Show("User information unavailable. Cannot time out.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    int? attendanceId = null;
                    DateTime? timeIn = null;

                    using (var cmd = new SqlCommand(@"SELECT TOP 1 attendance_id, time_in 
                                                      FROM Attendance 
                                                      WHERE UserID = @UserID AND [date] = @Date AND time_out IS NULL
                                                      ORDER BY time_in DESC", conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.Parameters.AddWithValue("@Date", DateTime.Today);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                attendanceId = Convert.ToInt32(reader["attendance_id"]);
                                timeIn = ConvertSqlTimeToDateTime(reader["time_in"], DateTime.Today);
                            }
                        }
                    }

                    if (!attendanceId.HasValue || !timeIn.HasValue)
                    {
                        MessageBox.Show("No active attendance entry found. Please time in first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    DateTime timeOut = DateTime.Now;
                    if (timeOut < timeIn.Value)
                    {
                        MessageBox.Show("Time out cannot be earlier than time in.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    using (var updateCmd = new SqlCommand(@"UPDATE Attendance 
                                                            SET time_out = @TimeOut 
                                                            WHERE attendance_id = @AttendanceID", conn))
                    {
                        var timeParam = updateCmd.Parameters.Add("@TimeOut", SqlDbType.Time);
                        timeParam.Value = timeOut.TimeOfDay;
                        updateCmd.Parameters.AddWithValue("@AttendanceID", attendanceId.Value);

                        updateCmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Time out recorded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshTodayAttendance();
                LoadAttendanceHistory();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to record time out: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

