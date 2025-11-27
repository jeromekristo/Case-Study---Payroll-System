using System;
using System.Windows.Forms;

namespace PayrollSample
{
    public partial class AdminDashboard : Form
    {
        private string username;

        public AdminDashboard(string username)
        {
            InitializeComponent();
            this.username = username;
            lblWelcome.Text = "Welcome, " + username + "!";
        }

        private void AdminDashboard_Load(object sender, EventArgs e)
        {

        }

        private void btnManageEmployees_Click(object sender, EventArgs e)
        {
            using (var manageForm = new ManageEmployeeRecords())
            {
                manageForm.StartPosition = FormStartPosition.CenterParent;
                manageForm.ShowDialog(this);
            }
        }

        private void btnAttendanceManagement_Click(object sender, EventArgs e)
        {
            using (var attendanceForm = new FrmAttendanceManagement())
            {
                attendanceForm.StartPosition = FormStartPosition.CenterParent;
                attendanceForm.ShowDialog(this);
            }
        }

        private void btnPayrollProcessing_Click(object sender, EventArgs e)
        {
            using (var payrollForm = new FrmPayrollProcessing())
            {
                payrollForm.StartPosition = FormStartPosition.CenterParent;
                payrollForm.ShowDialog(this);
            }
        }

        private void btnGenerateReports_Click(object sender, EventArgs e)
        {
            using (var reportsForm = new FrmReports())
            {
                reportsForm.StartPosition = FormStartPosition.CenterParent;
                reportsForm.ShowDialog(this);
            }
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            using (var registerForm = new RegisterForm())
            {
                registerForm.StartPosition = FormStartPosition.CenterParent;
                if (registerForm.ShowDialog(this) == DialogResult.OK)
                {
                    MessageBox.Show("Account created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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
            
            // Show the login form and bring it to front
            loginForm.Show();
            loginForm.BringToFront();
            
            // Close the dashboard
            this.Close();
        }
    }
}

