namespace PayrollSample
{
    partial class AdminDashboard
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
            this.lblWelcome = new System.Windows.Forms.Label();
            this.btnManageEmployees = new System.Windows.Forms.Button();
            this.btnAttendanceManagement = new System.Windows.Forms.Button();
            this.btnPayrollProcessing = new System.Windows.Forms.Button();
            this.btnGenerateReports = new System.Windows.Forms.Button();
            this.btnAddAccount = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWelcome.Location = new System.Drawing.Point(30, 25);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(223, 24);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Welcome, [Username]!";
            // 
            // btnManageEmployees
            // 
            this.btnManageEmployees.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnManageEmployees.Location = new System.Drawing.Point(30, 100);
            this.btnManageEmployees.Name = "btnManageEmployees";
            this.btnManageEmployees.Size = new System.Drawing.Size(200, 50);
            this.btnManageEmployees.TabIndex = 1;
            this.btnManageEmployees.Text = "Manage Employee Records";
            this.btnManageEmployees.UseVisualStyleBackColor = true;
            this.btnManageEmployees.Click += new System.EventHandler(this.btnManageEmployees_Click);
            // 
            // btnAttendanceManagement
            // 
            this.btnAttendanceManagement.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAttendanceManagement.Location = new System.Drawing.Point(250, 100);
            this.btnAttendanceManagement.Name = "btnAttendanceManagement";
            this.btnAttendanceManagement.Size = new System.Drawing.Size(200, 50);
            this.btnAttendanceManagement.TabIndex = 2;
            this.btnAttendanceManagement.Text = "Attendance Management";
            this.btnAttendanceManagement.UseVisualStyleBackColor = true;
            this.btnAttendanceManagement.Click += new System.EventHandler(this.btnAttendanceManagement_Click);
            // 
            // btnPayrollProcessing
            // 
            this.btnPayrollProcessing.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPayrollProcessing.Location = new System.Drawing.Point(30, 170);
            this.btnPayrollProcessing.Name = "btnPayrollProcessing";
            this.btnPayrollProcessing.Size = new System.Drawing.Size(200, 50);
            this.btnPayrollProcessing.TabIndex = 3;
            this.btnPayrollProcessing.Text = "Payroll Processing";
            this.btnPayrollProcessing.UseVisualStyleBackColor = true;
            this.btnPayrollProcessing.Click += new System.EventHandler(this.btnPayrollProcessing_Click);
            // 
            // btnGenerateReports
            // 
            this.btnGenerateReports.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerateReports.Location = new System.Drawing.Point(250, 170);
            this.btnGenerateReports.Name = "btnGenerateReports";
            this.btnGenerateReports.Size = new System.Drawing.Size(200, 50);
            this.btnGenerateReports.TabIndex = 4;
            this.btnGenerateReports.Text = "Generate Reports";
            this.btnGenerateReports.UseVisualStyleBackColor = true;
            this.btnGenerateReports.Click += new System.EventHandler(this.btnGenerateReports_Click);
            // 
            // btnAddAccount
            // 
            this.btnAddAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddAccount.Location = new System.Drawing.Point(30, 240);
            this.btnAddAccount.Name = "btnAddAccount";
            this.btnAddAccount.Size = new System.Drawing.Size(200, 50);
            this.btnAddAccount.TabIndex = 5;
            this.btnAddAccount.Text = "Add Account";
            this.btnAddAccount.UseVisualStyleBackColor = true;
            this.btnAddAccount.Click += new System.EventHandler(this.btnAddAccount_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogout.Location = new System.Drawing.Point(450, 25);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(80, 30);
            this.btnLogout.TabIndex = 6;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // AdminDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 320);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnAddAccount);
            this.Controls.Add(this.btnGenerateReports);
            this.Controls.Add(this.btnPayrollProcessing);
            this.Controls.Add(this.btnAttendanceManagement);
            this.Controls.Add(this.btnManageEmployees);
            this.Controls.Add(this.lblWelcome);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "AdminDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Admin Dashboard";
            this.Load += new System.EventHandler(this.AdminDashboard_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Button btnManageEmployees;
        private System.Windows.Forms.Button btnAttendanceManagement;
        private System.Windows.Forms.Button btnPayrollProcessing;
        private System.Windows.Forms.Button btnGenerateReports;
        private System.Windows.Forms.Button btnAddAccount;
        private System.Windows.Forms.Button btnLogout;
    }
}

