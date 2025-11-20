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
			conn.Open();

			// Check if username already exists
			using (SqlCommand cmd = new SqlCommand("SELECT * FROM [PayrollDB].dbo.Users WHERE Username = @username", conn))
			{
   			 	cmd.Parameters.AddWithValue("@username", tbUsername.Text);

   	    	using (SqlDataReader reader = cmd.ExecuteReader())
   	    	{
        	if (reader.Read())
        	{
            	MessageBox.Show("Username already exists. Please choose a different username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            	return;
        }
    }
}

			// Insert new user
			using (SqlCommand insertCmd = new SqlCommand("INSERT INTO [PayrollDB].dbo.Users(FirstName, LastName,Username,Role,Password) " +
                                            "VALUES (@firstName, @lastName, @username, @role, @password); ", conn))
			{
	    		insertCmd.Parameters.AddWithValue("@firstName", tbFirstName.Text);
    			insertCmd.Parameters.AddWithValue("@lastName", tbLastName.Text);
    			insertCmd.Parameters.AddWithValue("@username", tbUsername.Text);
    			insertCmd.Parameters.AddWithValue("@password", tbPassword.Text);
   		    	insertCmd.Parameters.AddWithValue("@role", "Employee");


    			insertCmd.ExecuteNonQuery();
			}

				MessageBox.Show("Account created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
	}
}



