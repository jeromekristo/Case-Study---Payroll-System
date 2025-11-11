using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PayrollSample
{
	public partial class RegisterForm : Form
	{
		private readonly string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=PayrollDB;Integrated Security=True";

		public RegisterForm()
		{
			InitializeComponent();
		}

		private void btnRegister_Click(object sender, EventArgs e)
		{
			string firstName = tbFirstName.Text.Trim();
			string lastName = tbLastName.Text.Trim();
			string username = tbUsername.Text.Trim();
			string password = tbPassword.Text.Trim();

			if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || 
			    string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
			{
				MessageBox.Show("Please fill in all fields.");
				return;
			}

			try
			{
				using (var conn = new SqlConnection(connectionString))
				using (var cmd = new SqlCommand("INSERT INTO Users (FirstName, LastName, Username, Password, Role) VALUES (@fn, @ln, @u, @p, @r)", conn))
				{
					cmd.Parameters.AddWithValue("@fn", firstName);
					cmd.Parameters.AddWithValue("@ln", lastName);
					cmd.Parameters.AddWithValue("@u", username);
					cmd.Parameters.AddWithValue("@p", password);
					cmd.Parameters.AddWithValue("@r", "Employee");

					conn.Open();
					int rows = cmd.ExecuteNonQuery();
					if (rows > 0)
					{
						MessageBox.Show("Registration successful.");
						this.DialogResult = DialogResult.OK;
						this.Close();
					}
					else
					{
						MessageBox.Show("Registration failed.");
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error: " + ex.Message);
			}
		}
	}
}


