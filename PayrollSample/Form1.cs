using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PayrollSample

{
    public partial class Form1: Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=PayrollDB;Integrated Security=True");
        public Form1()
        {
            InitializeComponent();
            SetupPlaceholders();
            SetupHoverEffects();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Set focus to username field
            userTB.Focus();
            
            // If Image is null or empty but InitialImage is set, use InitialImage
            // InitialImage is only shown during async loading, so we need to manually use it as fallback
            if ((pictureBox1.Image == null || pictureBox1.Image.Size.IsEmpty) && pictureBox1.InitialImage != null)
            {
                pictureBox1.Image = pictureBox1.InitialImage;
            }
        }

        private void SetupPlaceholders()
        {
            // Initially hide placeholders if textboxes have text (shouldn't happen, but just in case)
            labelUser.Visible = string.IsNullOrEmpty(userTB.Text);
            labelPass.Visible = string.IsNullOrEmpty(passTB.Text);

            // Username placeholder
            userTB.Enter += (s, e) => {
                labelUser.Visible = false;
            };
            userTB.Leave += (s, e) => {
                labelUser.Visible = string.IsNullOrEmpty(userTB.Text);
            };
            userTB.TextChanged += (s, e) => {
                if (userTB.Focused)
                {
                    labelUser.Visible = false;
                }
                else
                {
                    labelUser.Visible = string.IsNullOrEmpty(userTB.Text);
                }
            };

            // Password placeholder
            passTB.Enter += (s, e) => {
                labelPass.Visible = false;
            };
            passTB.Leave += (s, e) => {
                labelPass.Visible = string.IsNullOrEmpty(passTB.Text);
            };
            passTB.TextChanged += (s, e) => {
                if (passTB.Focused)
                {
                    labelPass.Visible = false;
                }
                else
                {
                    labelPass.Visible = string.IsNullOrEmpty(passTB.Text);
                }
            };

            // Enter key support
            passTB.KeyDown += (s, e) => {
                if (e.KeyCode == Keys.Enter)
                {
                    login.PerformClick();
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            };
        }

        private void SetupHoverEffects()
        {
            // Username panel hover
            panelUsername.MouseEnter += (s, e) => {
                panelUsername.BorderStyle = BorderStyle.FixedSingle;
                panelUsername.BackColor = Color.FromArgb(240, 240, 240);
            };
            panelUsername.MouseLeave += (s, e) => {
                panelUsername.BorderStyle = BorderStyle.FixedSingle;
                panelUsername.BackColor = Color.White;
            };

            // Password panel hover
            panelPassword.MouseEnter += (s, e) => {
                panelPassword.BorderStyle = BorderStyle.FixedSingle;
                panelPassword.BackColor = Color.FromArgb(240, 240, 240);
            };
            panelPassword.MouseLeave += (s, e) => {
                panelPassword.BorderStyle = BorderStyle.FixedSingle;
                panelPassword.BackColor = Color.White;
            };
        }

        public void ClearCredentials()
        {
            userTB.Clear();
            passTB.Clear();
        }

        private void login_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT * FROM Users WHERE Username=@Username AND Password=@Password";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", userTB.Text);
                cmd.Parameters.AddWithValue("@Password", passTB.Text);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    string role = reader["Role"].ToString();
                    string username = userTB.Text;
                    reader.Close();
                    conn.Close();
                    
                    this.Hide();
                    
                    if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase) || 
                        role.Equals("Employer", StringComparison.OrdinalIgnoreCase))
                    {
                        AdminDashboard adminDashboard = new AdminDashboard(username);
                        adminDashboard.Show();
                    }
                    else
                    {
                        UserDashboard dashboard = new UserDashboard(username);
                        dashboard.Show();
                    }
                }
                else
                {
                    reader.Close();
                    conn.Close();
                    MessageBox.Show("Invalid username or password.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void labelUser_MouseDown(object sender, MouseEventArgs e)
        {
            // When clicking on placeholder label, focus the textbox
            userTB.Focus();
        }

        private void labelPass_MouseDown(object sender, MouseEventArgs e)
        {
            // When clicking on placeholder label, focus the textbox
            passTB.Focus();
        }
    }
}
