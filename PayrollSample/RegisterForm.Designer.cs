namespace PayrollSample
{
	partial class RegisterForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tbFirstName = new System.Windows.Forms.TextBox();
			this.tbLastName = new System.Windows.Forms.TextBox();
			this.tbUsername = new System.Windows.Forms.TextBox();
			this.tbPassword = new System.Windows.Forms.TextBox();
			this.tbConfirmPassword = new System.Windows.Forms.TextBox();
			this.btnRegister = new System.Windows.Forms.Button();
			this.lblFirstName = new System.Windows.Forms.Label();
			this.lblLastName = new System.Windows.Forms.Label();
			this.lblUser = new System.Windows.Forms.Label();
			this.lblPass = new System.Windows.Forms.Label();
			this.lblConfirmPass = new System.Windows.Forms.Label();
			this.lblRole = new System.Windows.Forms.Label();
			this.cmbRole = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// tbFirstName
			// 
			this.tbFirstName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbFirstName.Location = new System.Drawing.Point(154, 32);
			this.tbFirstName.Name = "tbFirstName";
			this.tbFirstName.Size = new System.Drawing.Size(250, 26);
			this.tbFirstName.TabIndex = 0;
			// 
			// tbLastName
			// 
			this.tbLastName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbLastName.Location = new System.Drawing.Point(154, 72);
			this.tbLastName.Name = "tbLastName";
			this.tbLastName.Size = new System.Drawing.Size(250, 26);
			this.tbLastName.TabIndex = 1;
			// 
			// tbUsername
			// 
			this.tbUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbUsername.Location = new System.Drawing.Point(154, 112);
			this.tbUsername.Name = "tbUsername";
			this.tbUsername.Size = new System.Drawing.Size(250, 26);
			this.tbUsername.TabIndex = 2;
			// 
			// tbPassword
			// 
			this.tbPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbPassword.Location = new System.Drawing.Point(154, 152);
			this.tbPassword.Name = "tbPassword";
			this.tbPassword.Size = new System.Drawing.Size(250, 26);
			this.tbPassword.TabIndex = 3;
			this.tbPassword.UseSystemPasswordChar = true;
			// 
			// btnRegister
			// 
			this.btnRegister.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnRegister.Location = new System.Drawing.Point(154, 268);
			this.btnRegister.Name = "btnRegister";
			this.btnRegister.Size = new System.Drawing.Size(180, 40);
			this.btnRegister.TabIndex = 4;
			this.btnRegister.Text = "Create Account";
			this.btnRegister.UseVisualStyleBackColor = true;
			this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
			// 
			// lblFirstName
			// 
			this.lblFirstName.AutoSize = true;
			this.lblFirstName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFirstName.Location = new System.Drawing.Point(58, 35);
			this.lblFirstName.Name = "lblFirstName";
			this.lblFirstName.Size = new System.Drawing.Size(90, 20);
			this.lblFirstName.TabIndex = 5;
			this.lblFirstName.Text = "First Name:";
			// 
			// lblLastName
			// 
			this.lblLastName.AutoSize = true;
			this.lblLastName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblLastName.Location = new System.Drawing.Point(58, 75);
			this.lblLastName.Name = "lblLastName";
			this.lblLastName.Size = new System.Drawing.Size(90, 20);
			this.lblLastName.TabIndex = 6;
			this.lblLastName.Text = "Last Name:";
			// 
			// lblUser
			// 
			this.lblUser.AutoSize = true;
			this.lblUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblUser.Location = new System.Drawing.Point(58, 115);
			this.lblUser.Name = "lblUser";
			this.lblUser.Size = new System.Drawing.Size(87, 20);
			this.lblUser.TabIndex = 7;
			this.lblUser.Text = "Username:";
			// 
			// lblPass
			// 
			this.lblPass.AutoSize = true;
			this.lblPass.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPass.Location = new System.Drawing.Point(60, 155);
			this.lblPass.Name = "lblPass";
			this.lblPass.Size = new System.Drawing.Size(82, 20);
			this.lblPass.TabIndex = 8;
			this.lblPass.Text = "Password:";
			// 
			// tbConfirmPassword
			// 
			this.tbConfirmPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbConfirmPassword.Location = new System.Drawing.Point(154, 192);
			this.tbConfirmPassword.Name = "tbConfirmPassword";
			this.tbConfirmPassword.Size = new System.Drawing.Size(250, 26);
			this.tbConfirmPassword.TabIndex = 4;
			this.tbConfirmPassword.UseSystemPasswordChar = true;
			// 
			// lblConfirmPass
			// 
			this.lblConfirmPass.AutoSize = true;
			this.lblConfirmPass.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblConfirmPass.Location = new System.Drawing.Point(10, 195);
			this.lblConfirmPass.Name = "lblConfirmPass";
			this.lblConfirmPass.Size = new System.Drawing.Size(132, 20);
			this.lblConfirmPass.TabIndex = 9;
			this.lblConfirmPass.Text = "Confirm Password:";
			// 
			// lblRole
			// 
			this.lblRole.AutoSize = true;
			this.lblRole.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblRole.Location = new System.Drawing.Point(60, 231);
			this.lblRole.Name = "lblRole";
			this.lblRole.Size = new System.Drawing.Size(47, 20);
			this.lblRole.TabIndex = 9;
			this.lblRole.Text = "Role:";
			// 
			// cmbRole
			// 
			this.cmbRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbRole.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmbRole.FormattingEnabled = true;
			this.cmbRole.Items.AddRange(new object[] {
            "Employee (Full-Time)",
            "Part-Time"});
			this.cmbRole.Location = new System.Drawing.Point(154, 228);
			this.cmbRole.Name = "cmbRole";
			this.cmbRole.Size = new System.Drawing.Size(250, 28);
			this.cmbRole.TabIndex = 5;
			// 
			// RegisterForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(450, 330);
			this.Controls.Add(this.tbConfirmPassword);
			this.Controls.Add(this.lblConfirmPass);
			this.Controls.Add(this.cmbRole);
			this.Controls.Add(this.lblRole);
			this.Controls.Add(this.lblPass);
			this.Controls.Add(this.lblUser);
			this.Controls.Add(this.lblLastName);
			this.Controls.Add(this.lblFirstName);
			this.Controls.Add(this.btnRegister);
			this.Controls.Add(this.tbPassword);
			this.Controls.Add(this.tbUsername);
			this.Controls.Add(this.tbLastName);
			this.Controls.Add(this.tbFirstName);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RegisterForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Register User";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox tbFirstName;
		private System.Windows.Forms.TextBox tbLastName;
		private System.Windows.Forms.TextBox tbUsername;
		private System.Windows.Forms.TextBox tbPassword;
		private System.Windows.Forms.Button btnRegister;
		private System.Windows.Forms.Label lblFirstName;
		private System.Windows.Forms.Label lblLastName;
		private System.Windows.Forms.Label lblUser;
		private System.Windows.Forms.Label lblPass;
		private System.Windows.Forms.TextBox tbConfirmPassword;
		private System.Windows.Forms.Label lblConfirmPass;
		private System.Windows.Forms.Label lblRole;
		private System.Windows.Forms.ComboBox cmbRole;
	}
}


