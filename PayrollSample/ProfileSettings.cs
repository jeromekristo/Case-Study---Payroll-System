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
            string firstName = tbFirstName.Text.Trim();
            string lastName = tbLastName.Text.Trim();
            string username = tbUsername.Text.Trim();
            string password = tbPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    // Check if username is being changed and if it already exists
                    if (username != currentUsername)
                    {
                        using (var checkCmd = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username=@username", conn))
                        {
                            checkCmd.Parameters.AddWithValue("@username", username);
                            conn.Open();
                            int count = (int)checkCmd.ExecuteScalar();
                            conn.Close();

                            if (count > 0)
                            {
                                MessageBox.Show("Username already exists. Please choose a different username.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }

                
                    using (var updateCmd = new SqlCommand("UPDATE Users SET FirstName=@fn, LastName=@ln, Username=@u, Password=@p WHERE Username=@oldUsername", conn))
                    {
                        updateCmd.Parameters.AddWithValue("@fn", firstName);
                        updateCmd.Parameters.AddWithValue("@ln", lastName);
                        updateCmd.Parameters.AddWithValue("@u", username);
                        updateCmd.Parameters.AddWithValue("@p", password);
                        updateCmd.Parameters.AddWithValue("@oldUsername", currentUsername);

                        conn.Open();
                        int rows = updateCmd.ExecuteNonQuery();
                        conn.Close();

                        if (rows > 0)
                        {
                            MessageBox.Show("Profile updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.currentUsername = username; 
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update profile.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating profile: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

