using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PayrollSample
{
    public partial class FrmAttendanceManagement : Form
    {
        private readonly string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=PayrollDB;Integrated Security=True";
        private bool isEditing;
        private DataGridViewRow editingRow;

        public FrmAttendanceManagement()
        {
            InitializeComponent();
            dgvAttendance.AutoGenerateColumns = false;
        }

        private void FrmAttendanceManagement_Load(object sender, EventArgs e)
        {
            dtFrom.Value = DateTime.Today.AddDays(-7);
            dtTo.Value = DateTime.Today;
            LoadEmployees();
            ToggleEditMode(false);
        }

        private void LoadEmployees()
        {
            try
            {
                var employees = new List<EmployeeItem>
                {
                    new EmployeeItem { UserId = 0, DisplayName = "All Employees" }
                };

                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(
                    @"SELECT UserID, FirstName, LastName
                      FROM Users
                      WHERE Role IN ('Employee', 'Part-Time')
                      ORDER BY LastName, FirstName", conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var userId = reader.GetInt32(0);
                            var firstName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                            var lastName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                            var fullName = $"{firstName} {lastName}".Trim();
                            employees.Add(new EmployeeItem
                            {
                                UserId = userId,
                                DisplayName = fullName
                            });
                        }
                    }
                }

                cmbEmployee.DataSource = employees;
                cmbEmployee.DisplayMember = nameof(EmployeeItem.DisplayName);
                cmbEmployee.ValueMember = nameof(EmployeeItem.UserId);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load employees: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadAttendance();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadAttendance();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadAttendance();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void LoadAttendance()
        {
            if (isEditing)
            {
                MessageBox.Show("Finish editing before reloading data.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (dtFrom.Value.Date > dtTo.Value.Date)
            {
                MessageBox.Show("The From date cannot be later than the To date.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(
                    @"SELECT a.attendance_id,
                             u.UserID AS user_id,
                             u.FirstName AS first_name,
                             u.LastName AS last_name,
                             a.[date],
                             a.time_in,
                             a.time_out,
                             a.hours_worked
                      FROM Attendance a
                      JOIN Users u ON a.UserID = u.UserID
                      WHERE a.[date] BETWEEN @from AND @to
                        AND (@userId = 0 OR u.UserID = @userId)
                        AND (
                            @searchTerm = '' OR 
                            u.FirstName LIKE @likeSearch OR 
                            u.LastName LIKE @likeSearch OR 
                            u.Username LIKE @likeSearch
                        )
                      ORDER BY a.date DESC;", conn))
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.Add("@from", SqlDbType.Date).Value = dtFrom.Value.Date;
                    cmd.Parameters.Add("@to", SqlDbType.Date).Value = dtTo.Value.Date;
                    cmd.Parameters.Add("@userId", SqlDbType.Int).Value = GetSelectedUserId();

                    string searchTerm = (txtSearch.Text ?? string.Empty).Trim();
                    cmd.Parameters.Add("@searchTerm", SqlDbType.NVarChar, 100).Value = searchTerm;
                    cmd.Parameters.Add("@likeSearch", SqlDbType.NVarChar, 100).Value = "%" + searchTerm + "%";

                    var table = new DataTable();
                    adapter.Fill(table);
                    dgvAttendance.DataSource = table;
                    dgvAttendance.ClearSelection();
                    editingRow = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load attendance: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            UpdateButtonStates();
        }

        private int GetSelectedUserId()
        {
            if (cmbEmployee.SelectedValue is int selectedId)
            {
                return selectedId;
            }

            return 0;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvAttendance.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select an attendance entry to edit.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            editingRow = dgvAttendance.SelectedRows[0];
            ToggleEditMode(true);

            if (editingRow != null)
            {
                dgvAttendance.CurrentCell = editingRow.Cells["time_in"];
                dgvAttendance.BeginEdit(true);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (!isEditing)
            {
                return;
            }

            ToggleEditMode(false);
            LoadAttendance();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvAttendance.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select an attendance entry to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var row = dgvAttendance.SelectedRows[0];
            if (row.Cells["attendance_id"].Value == null)
            {
                return;
            }

            var confirm = MessageBox.Show("Delete the selected attendance entry?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes)
            {
                return;
            }

            var attendanceId = Convert.ToInt32(row.Cells["attendance_id"].Value);

            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("DELETE FROM Attendance WHERE attendance_id = @id;", conn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = attendanceId;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Attendance entry deleted.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadAttendance();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete attendance: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!isEditing || editingRow == null)
            {
                return;
            }

            if (editingRow.Cells["attendance_id"].Value == null)
            {
                MessageBox.Show("Invalid attendance entry.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!TryParseTimeValue(editingRow.Cells["time_in"], out var timeIn, out var errorMessage))
            {
                MessageBox.Show("Invalid Time In value. " + errorMessage, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!TryParseTimeValue(editingRow.Cells["time_out"], out var timeOut, out errorMessage))
            {
                MessageBox.Show("Invalid Time Out value. " + errorMessage, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (timeIn.HasValue && timeOut.HasValue && timeOut < timeIn)
            {
                MessageBox.Show("Time Out cannot be earlier than Time In.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var attendanceId = Convert.ToInt32(editingRow.Cells["attendance_id"].Value);

            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(
                    @"UPDATE Attendance
                      SET time_in = @time_in,
                          time_out = @time_out
                      WHERE attendance_id = @id;", conn))
                {
                    var timeInParam = cmd.Parameters.Add("@time_in", SqlDbType.Time);
                    timeInParam.Value = (object)timeIn ?? DBNull.Value;
                    var timeOutParam = cmd.Parameters.Add("@time_out", SqlDbType.Time);
                    timeOutParam.Value = (object)timeOut ?? DBNull.Value;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = attendanceId;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Attendance entry updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ToggleEditMode(false);
                LoadAttendance();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update attendance: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvAttendance_SelectionChanged(object sender, EventArgs e)
        {
            if (isEditing)
            {
                if (editingRow != null)
                {
                    editingRow.Selected = true;
                }

                return;
            }

            UpdateButtonStates();
        }

        private void dgvAttendance_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!isEditing || e.RowIndex < 0)
            {
                return;
            }

            var columnName = dgvAttendance.Columns[e.ColumnIndex].Name;
            if (columnName == "time_in" || columnName == "time_out")
            {
                var row = dgvAttendance.Rows[e.RowIndex];
                RecalculateHours(row);
            }
        }

        private void dgvAttendance_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (!isEditing)
            {
                e.Cancel = true;
                return;
            }

            var columnName = dgvAttendance.Columns[e.ColumnIndex].Name;
            if ((columnName != "time_in" && columnName != "time_out") ||
                editingRow == null ||
                e.RowIndex != editingRow.Index)
            {
                e.Cancel = true;
            }
        }

        private void dgvAttendance_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Invalid value entered. Please check the format.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            e.ThrowException = false;
        }

        private void ToggleEditMode(bool enable)
        {
            isEditing = enable;
            btnSave.Enabled = enable;
            btnCancel.Enabled = enable;
            cmbEmployee.Enabled = !enable;
            dtFrom.Enabled = !enable;
            dtTo.Enabled = !enable;
            btnLoad.Enabled = !enable;
            btnEdit.Enabled = !enable && dgvAttendance.SelectedRows.Count > 0;
            btnDelete.Enabled = !enable && dgvAttendance.SelectedRows.Count > 0;

            dgvAttendance.ReadOnly = !enable;

            foreach (DataGridViewColumn column in dgvAttendance.Columns)
            {
                column.ReadOnly = enable
                    ? (column.Name != "time_in" && column.Name != "time_out")
                    : true;
            }

            if (!enable)
            {
                editingRow = null;
            }
        }

        private void UpdateButtonStates()
        {
            var hasSelection = dgvAttendance.SelectedRows.Count > 0;
            btnEdit.Enabled = hasSelection && !isEditing;
            btnDelete.Enabled = hasSelection && !isEditing;
        }

        private void RecalculateHours(DataGridViewRow row)
        {
            if (!TryParseTimeValue(row.Cells["time_in"], out var timeIn, out _))
            {
                return;
            }

            if (!TryParseTimeValue(row.Cells["time_out"], out var timeOut, out _))
            {
                return;
            }

            if (timeIn.HasValue && timeOut.HasValue && timeOut >= timeIn)
            {
                var hours = (decimal)(timeOut.Value - timeIn.Value).TotalHours;
                row.Cells["hours_worked"].Value = Math.Round(hours, 2);
            }
            else
            {
                row.Cells["hours_worked"].Value = 0m;
            }
        }

        private bool TryParseTimeValue(DataGridViewCell cell, out TimeSpan? value, out string errorMessage)
        {
            value = null;
            errorMessage = string.Empty;

            if (cell == null || cell.Value == null || cell.Value == DBNull.Value)
            {
                return true;
            }

            if (cell.Value is TimeSpan timeSpanValue)
            {
                value = timeSpanValue;
                return true;
            }

            if (cell.Value is DateTime dtValue)
            {
                value = dtValue.TimeOfDay;
                return true;
            }

            var raw = cell.Value.ToString();
            if (string.IsNullOrWhiteSpace(raw))
            {
                cell.Value = DBNull.Value;
                return true;
            }

            if (TimeSpan.TryParse(raw, out timeSpanValue))
            {
                cell.Value = timeSpanValue;
                value = timeSpanValue;
                return true;
            }

            if (DateTime.TryParse(raw, out dtValue))
            {
                value = dtValue.TimeOfDay;
                cell.Value = value;
                return true;
            }

            errorMessage = "Use HH:mm format (e.g., 08:00).";
            return false;
        }

        private sealed class EmployeeItem
        {
            public int UserId { get; set; }
            public string DisplayName { get; set; }
        }
    }
}


