using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PayrollSample
{
    public partial class ProfileSettings : Form
    {
        private readonly string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=PayrollDB;Integrated Security=True";
        private string currentUsername;

        public ProfileSettings(string username)
        {
            InitializeComponent();
            this.currentUsername = username;
            LoadUserData();

            // Users can only change their password; lock name and username fields
            tbFirstName.ReadOnly = true;
            tbLastName.ReadOnly = true;
            tbUsername.ReadOnly = true;
        }

        private void LoadUserData()
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("SELECT FirstName, LastName, Username, Password FROM Users WHERE Username=@username", conn))
                {
                    cmd.Parameters.AddWithValue("@username", currentUsername);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        tbFirstName.Text = reader["FirstName"].ToString();
                        tbLastName.Text = reader["LastName"].ToString();
                        tbUsername.Text = reader["Username"].ToString();
                        tbPassword.Text = reader["Password"].ToString();
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading user data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Users are only allowed to change their password here
            string password = tbPassword.Text.Trim();
            string confirmPassword = tbConfirmPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Please enter and confirm your new password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!string.Equals(password, confirmPassword))
            {
                MessageBox.Show("Password and Confirm Password do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var updateCmd = new SqlCommand("UPDATE Users SET Password=@p WHERE Username=@username", conn))
                {
                    updateCmd.Parameters.AddWithValue("@p", password);
                    updateCmd.Parameters.AddWithValue("@username", currentUsername);

                    conn.Open();
                    int rows = updateCmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        MessageBox.Show("Password updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating password: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void groupBoxProfile_Enter(object sender, EventArgs e)
        {

        }
    }
}

