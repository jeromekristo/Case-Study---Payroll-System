namespace PayrollSample
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.userTB = new System.Windows.Forms.TextBox();
            this.passTB = new System.Windows.Forms.TextBox();
            this.login = new System.Windows.Forms.Button();
            this.labelUser = new System.Windows.Forms.Label();
            this.labelPass = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // userTB
            // 
            this.userTB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.userTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userTB.Location = new System.Drawing.Point(445, 171);
            this.userTB.Name = "userTB";
            this.userTB.Size = new System.Drawing.Size(180, 26);
            this.userTB.TabIndex = 0;
            // 
            // passTB
            // 
            this.passTB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.passTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passTB.Location = new System.Drawing.Point(445, 210);
            this.passTB.Name = "passTB";
            this.passTB.Size = new System.Drawing.Size(180, 26);
            this.passTB.TabIndex = 1;
            this.passTB.UseSystemPasswordChar = true;
            // 
            // login
            // 
            this.login.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.login.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.login.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.login.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.login.Location = new System.Drawing.Point(445, 252);
            this.login.Name = "login";
            this.login.Size = new System.Drawing.Size(180, 37);
            this.login.TabIndex = 2;
            this.login.Text = "Login";
            this.login.UseVisualStyleBackColor = false;
            this.login.Click += new System.EventHandler(this.login_Click);
            // 
            // labelUser
            // 
            this.labelUser.AutoSize = true;
            this.labelUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUser.Location = new System.Drawing.Point(352, 171);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(87, 20);
            this.labelUser.TabIndex = 4;
            this.labelUser.Text = "Username:";
            // 
            // labelPass
            // 
            this.labelPass.AutoSize = true;
            this.labelPass.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPass.Location = new System.Drawing.Point(352, 216);
            this.labelPass.Name = "labelPass";
            this.labelPass.Size = new System.Drawing.Size(82, 20);
            this.labelPass.TabIndex = 5;
            this.labelPass.Text = "Password:";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(428, -16);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(180, 164);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 7;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(2, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(344, 342);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(352, 128);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(314, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "Please enter your username and password.";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(676, 343);
            this.Controls.Add(this.passTB);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelPass);
            this.Controls.Add(this.labelUser);
            this.Controls.Add(this.login);
            this.Controls.Add(this.userTB);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pictureBox2);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Payroll - Login";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.TextBox userTB;
		private System.Windows.Forms.TextBox passTB;
		private System.Windows.Forms.Button login;
		private System.Windows.Forms.Label labelUser;
		private System.Windows.Forms.Label labelPass;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label1;
    }
}

