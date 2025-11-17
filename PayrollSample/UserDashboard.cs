using System;
using System.Windows.Forms;

namespace PayrollSample
{
    public partial class UserDashboard : Form
    {
        private string username;

        public UserDashboard(string username)
        {
            InitializeComponent();
            this.username = username;
            lblWelcome.Text = "Welcome, " + username + "!";
        }

        private void btnViewAttendance_Click(object sender, EventArgs e)
        {
            // Placeholder - will be implemented later
            MessageBox.Show("View Attendance feature coming soon!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
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
    }
}

