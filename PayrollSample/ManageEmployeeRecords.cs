using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PayrollSample
{
    public partial class ManageEmployeeRecords : Form
    {
        private readonly string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=PayrollDB;Integrated Security=True";
        private int? selectedUserId = null;

        public ManageEmployeeRecords()
        {
            InitializeComponent();
        }

        private void ManageEmployeeRecords_Load(object sender, EventArgs e)
        {
            RemoveObsoleteColumns();
            EnsureUserColumns();

            cmbSalaryType.Items.AddRange(new object[] { "Hourly", "Daily", "Monthly", "Annual" });
            cmbStatus.Items.AddRange(new object[] { "Active", "Inactive" });

            if (cmbSalaryType.Items.Count > 0)
            {
                cmbSalaryType.SelectedIndex = 0;
            }

            if (cmbStatus.Items.Count > 0)
            {
                cmbStatus.SelectedIndex = 0;
            }

            LoadEmployees();
        }

        private void LoadEmployees(string searchTerm = "")
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    string query = @"SELECT UserID,
                                            FirstName,
                                            LastName,
                                            Username,
                                            Role,
                                            salary_rate AS SalaryRate,
                                            salary_type AS SalaryType,
                                            Status
                                     FROM Users
                                     WHERE Role IN ('Employee', 'Part-Time')";
                    
                    if (!string.IsNullOrWhiteSpace(searchTerm))
                    {
                        query += @" AND (FirstName LIKE @SearchTerm 
                                         OR LastName LIKE @SearchTerm 
                                         OR Username LIKE @SearchTerm)";
                    }
                    
                    query += " ORDER BY LastName, FirstName";
                    
                    using (var adapter = new SqlDataAdapter(query, conn))
                    {
                        if (!string.IsNullOrWhiteSpace(searchTerm))
                        {
                            adapter.SelectCommand.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm.Trim()}%");
                        }
                        
                        var table = new DataTable();
                        adapter.Fill(table);
                        dgvEmployees.DataSource = table;
                        dgvEmployees.ClearSelection();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load employees: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInputs(out decimal salaryRate)
        {
            salaryRate = 0m;

            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("First name, last name, and username are required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(txtSalaryRate.Text, out salaryRate) || salaryRate < 0)
            {
                MessageBox.Show("Salary rate must be a positive number.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbSalaryType.SelectedItem == null)
            {
                MessageBox.Show("Select a salary type.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbStatus.SelectedItem == null)
            {
                MessageBox.Show("Select a status.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs(out decimal salaryRate))
            {
                return;
            }

            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(
                    @"INSERT INTO Users 
                        (FirstName, LastName, Username, Password, Role, salary_rate, salary_type, Status) 
                      VALUES 
                        (@FirstName, @LastName, @Username, @Password, 'Employee', @SalaryRate, @SalaryType, @Status)", conn))
                {
                    cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text.Trim());
                    cmd.Parameters.AddWithValue("@LastName", txtLastName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("@Password", "Password123!");
                    cmd.Parameters.AddWithValue("@SalaryRate", salaryRate);
                    cmd.Parameters.AddWithValue("@SalaryType", cmbSalaryType.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@Status", cmbStatus.SelectedItem.ToString());

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Employee added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadEmployees();
            }
            catch (SqlException sqlEx) when (sqlEx.Number == 2627 || sqlEx.Number == 2601)
            {
                MessageBox.Show("Username already exists. Please choose another.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add employee: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedUserId == null)
            {
                MessageBox.Show("Select an employee to update.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInputs(out decimal salaryRate))
            {
                return;
            }

            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(
                    @"UPDATE Users
                      SET FirstName = @FirstName,
                          LastName = @LastName,
                          Username = @Username,
                          salary_rate = @SalaryRate,
                          salary_type = @SalaryType,
                          Status = @Status
                      WHERE UserID = @UserID", conn))
                {
                    cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text.Trim());
                    cmd.Parameters.AddWithValue("@LastName", txtLastName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("@SalaryRate", salaryRate);
                    cmd.Parameters.AddWithValue("@SalaryType", cmbSalaryType.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@Status", cmbStatus.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@UserID", selectedUserId.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Employee updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadEmployees();
            }
            catch (SqlException sqlEx) when (sqlEx.Number == 2627 || sqlEx.Number == 2601)
            {
                MessageBox.Show("Username already exists. Please choose another.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update employee: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedUserId == null)
            {
                MessageBox.Show("Select an employee to delete or disable.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var dialog = new DeleteDisableDialog())
            {
                if (dialog.ShowDialog(this) == DialogResult.Cancel)
                {
                    return;
                }

                try
                {
                    using (var conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        
                        using (var transaction = conn.BeginTransaction())
                        {
                            try
                            {
                                if (dialog.UserChoice == DialogResult.Yes) // Delete
                                {
                                    // Delete related records first to avoid foreign key constraint violations
                                    // Delete from Attendance table
                                    using (var deleteAttendanceCmd = new SqlCommand("DELETE FROM Attendance WHERE UserID = @UserID", conn, transaction))
                                    {
                                        deleteAttendanceCmd.Parameters.AddWithValue("@UserID", selectedUserId.Value);
                                        deleteAttendanceCmd.ExecuteNonQuery();
                                    }

                                    // Delete from Payslips table (if it exists)
                                    try
                                    {
                                        using (var deletePayslipsCmd = new SqlCommand("DELETE FROM Payslips WHERE UserID = @UserID", conn, transaction))
                                        {
                                            deletePayslipsCmd.Parameters.AddWithValue("@UserID", selectedUserId.Value);
                                            deletePayslipsCmd.ExecuteNonQuery();
                                        }
                                    }
                                    catch (SqlException)
                                    {
                                        // Payslips table might not exist or use different column name, try alternative
                                        try
                                        {
                                            using (var deletePayslipsCmd = new SqlCommand("DELETE FROM Payslips WHERE user_id = @UserID", conn, transaction))
                                            {
                                                deletePayslipsCmd.Parameters.AddWithValue("@UserID", selectedUserId.Value);
                                                deletePayslipsCmd.ExecuteNonQuery();
                                            }
                                        }
                                        catch (SqlException)
                                        {
                                            // Table might not exist, continue
                                        }
                                    }

                                    // Delete from Payroll table (if it exists)
                                    try
                                    {
                                        using (var deletePayrollCmd = new SqlCommand("DELETE FROM Payroll WHERE UserID = @UserID", conn, transaction))
                                        {
                                            deletePayrollCmd.Parameters.AddWithValue("@UserID", selectedUserId.Value);
                                            deletePayrollCmd.ExecuteNonQuery();
                                        }
                                    }
                                    catch (SqlException)
                                    {
                                        // Payroll table might not exist or use different column name, try alternative
                                        try
                                        {
                                            using (var deletePayrollCmd = new SqlCommand("DELETE FROM Payroll WHERE user_id = @UserID", conn, transaction))
                                            {
                                                deletePayrollCmd.Parameters.AddWithValue("@UserID", selectedUserId.Value);
                                                deletePayrollCmd.ExecuteNonQuery();
                                            }
                                        }
                                        catch (SqlException)
                                        {
                                            // Table might not exist, continue
                                        }
                                    }

                                    // Finally, delete the employee from Users table
                                    using (var deleteUserCmd = new SqlCommand("DELETE FROM Users WHERE UserID = @UserID", conn, transaction))
                                    {
                                        deleteUserCmd.Parameters.AddWithValue("@UserID", selectedUserId.Value);
                                        deleteUserCmd.ExecuteNonQuery();
                                    }

                                    transaction.Commit();
                                    MessageBox.Show("Employee removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else if (dialog.UserChoice == DialogResult.No) // Disable
                                {
                                    using (var disableCmd = new SqlCommand("UPDATE Users SET Status = 'Inactive' WHERE UserID = @UserID", conn, transaction))
                                    {
                                        disableCmd.Parameters.AddWithValue("@UserID", selectedUserId.Value);
                                        disableCmd.ExecuteNonQuery();
                                    }

                                    transaction.Commit();
                                    MessageBox.Show("Employee disabled successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    transaction.Rollback();
                                    return; // Cancel
                                }

                                ClearForm();
                                LoadEmployees();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                throw; // Re-throw to be caught by outer catch block
                            }
                        }
                    }
                }
                catch (SqlException sqlEx)
                {
                    string errorMessage = "Failed to modify employee: " + sqlEx.Message;
                    if (sqlEx.Number == 547) // Foreign key constraint violation
                    {
                        errorMessage = "Cannot delete employee. There are still related records in the database. Please contact the administrator.";
                    }
                    MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to modify employee: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvEmployees_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvEmployees.SelectedRows.Count == 0)
            {
                selectedUserId = null;
                return;
            }

            var row = dgvEmployees.SelectedRows[0];

            if (row == null || row.Cells["UserID"].Value == null)
            {
                selectedUserId = null;
                return;
            }

            selectedUserId = Convert.ToInt32(row.Cells["UserID"].Value);
            txtFirstName.Text = Convert.ToString(row.Cells["FirstName"].Value);
            txtLastName.Text = Convert.ToString(row.Cells["LastName"].Value);
            txtUsername.Text = Convert.ToString(row.Cells["Username"].Value);
            txtSalaryRate.Text = Convert.ToString(row.Cells["SalaryRate"].Value);

            var salaryType = Convert.ToString(row.Cells["SalaryType"].Value);
            if (!string.IsNullOrEmpty(salaryType) && cmbSalaryType.Items.Contains(salaryType))
            {
                cmbSalaryType.SelectedItem = salaryType;
            }

            var status = Convert.ToString(row.Cells["Status"].Value);
            if (!string.IsNullOrEmpty(status) && cmbStatus.Items.Contains(status))
            {
                cmbStatus.SelectedItem = status;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadEmployees();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadEmployees(txtSearch.Text);
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            selectedUserId = null;
            txtFirstName.Clear();
            txtLastName.Clear();
            txtUsername.Clear();
            txtSalaryRate.Clear();

            if (cmbSalaryType.Items.Count > 0)
            {
                cmbSalaryType.SelectedIndex = 0;
            }

            if (cmbStatus.Items.Count > 0)
            {
                cmbStatus.SelectedIndex = 0;
            }

            dgvEmployees.ClearSelection();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EnsureUserColumns()
        {
            var requiredColumns = new[]
            {
                new ColumnRequirement("Status", "NVARCHAR(20) NULL", "Active"),
            };

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    foreach (var column in requiredColumns)
                    {
                        if (!ColumnExists(conn, column.Name))
                        {
                            using (var addCmd = new SqlCommand($"ALTER TABLE Users ADD [{column.Name}] {column.Definition}", conn))
                            {
                                addCmd.ExecuteNonQuery();
                            }

                            if (column.DefaultValue != null)
                            {
                                using (var updateCmd = new SqlCommand($"UPDATE Users SET [{column.Name}] = @Default WHERE [{column.Name}] IS NULL", conn))
                                {
                                    updateCmd.Parameters.AddWithValue("@Default", column.DefaultValue);
                                    updateCmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to verify Users table columns: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RemoveObsoleteColumns()
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    DropColumnIfExists(conn, "ContactNumber");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update Users table schema: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DropColumnIfExists(SqlConnection conn, string columnName)
        {
            if (!ColumnExists(conn, columnName))
            {
                return;
            }

            using (var cmd = new SqlCommand($"ALTER TABLE Users DROP COLUMN [{columnName}]", conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private bool ColumnExists(SqlConnection conn, string columnName)
        {
            using (var cmd = new SqlCommand(
                @"SELECT 1 
                  FROM INFORMATION_SCHEMA.COLUMNS 
                  WHERE TABLE_NAME = 'Users' AND COLUMN_NAME = @ColumnName", conn))
            {
                cmd.Parameters.AddWithValue("@ColumnName", columnName);
                var result = cmd.ExecuteScalar();
                return result != null;
            }
        }

        private sealed class ColumnRequirement
        {
            public string Name { get; }
            public string Definition { get; }
            public object DefaultValue { get; }

            public ColumnRequirement(string name, string definition, object defaultValue)
            {
                Name = name;
                Definition = definition;
                DefaultValue = defaultValue;
            }
        }
    }
}

