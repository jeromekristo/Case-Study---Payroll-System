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
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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
    }
}
