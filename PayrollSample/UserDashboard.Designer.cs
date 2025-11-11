namespace PayrollSample
{
    partial class UserDashboard
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
            this.groupBoxSummary = new System.Windows.Forms.GroupBox();
            this.lblSummaryPlaceholder = new System.Windows.Forms.Label();
            this.btnViewAttendance = new System.Windows.Forms.Button();
            this.btnViewPayslip = new System.Windows.Forms.Button();
            this.btnProfileSettings = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.groupBoxSummary.SuspendLayout();
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
            // groupBoxSummary
            // 
            this.groupBoxSummary.Controls.Add(this.lblSummaryPlaceholder);
            this.groupBoxSummary.Location = new System.Drawing.Point(30, 70);
            this.groupBoxSummary.Name = "groupBoxSummary";
            this.groupBoxSummary.Size = new System.Drawing.Size(500, 150);
            this.groupBoxSummary.TabIndex = 1;
            this.groupBoxSummary.TabStop = false;
            this.groupBoxSummary.Text = "Summary";
            // 
            // lblSummaryPlaceholder
            // 
            this.lblSummaryPlaceholder.AutoSize = true;
            this.lblSummaryPlaceholder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSummaryPlaceholder.ForeColor = System.Drawing.Color.Gray;
            this.lblSummaryPlaceholder.Location = new System.Drawing.Point(20, 60);
            this.lblSummaryPlaceholder.Name = "lblSummaryPlaceholder";
            this.lblSummaryPlaceholder.Size = new System.Drawing.Size(419, 17);
            this.lblSummaryPlaceholder.TabIndex = 0;
            this.lblSummaryPlaceholder.Text = "Recent payroll and attendance information will be displayed here.";
            // 
            // btnViewAttendance
            // 
            this.btnViewAttendance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewAttendance.Location = new System.Drawing.Point(30, 250);
            this.btnViewAttendance.Name = "btnViewAttendance";
            this.btnViewAttendance.Size = new System.Drawing.Size(150, 40);
            this.btnViewAttendance.TabIndex = 2;
            this.btnViewAttendance.Text = "View Attendance";
            this.btnViewAttendance.UseVisualStyleBackColor = true;
            this.btnViewAttendance.Click += new System.EventHandler(this.btnViewAttendance_Click);
            // 
            // btnViewPayslip
            // 
            this.btnViewPayslip.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewPayslip.Location = new System.Drawing.Point(200, 250);
            this.btnViewPayslip.Name = "btnViewPayslip";
            this.btnViewPayslip.Size = new System.Drawing.Size(150, 40);
            this.btnViewPayslip.TabIndex = 3;
            this.btnViewPayslip.Text = "View Payslip";
            this.btnViewPayslip.UseVisualStyleBackColor = true;
            this.btnViewPayslip.Click += new System.EventHandler(this.btnViewPayslip_Click);
            // 
            // btnProfileSettings
            // 
            this.btnProfileSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProfileSettings.Location = new System.Drawing.Point(370, 250);
            this.btnProfileSettings.Name = "btnProfileSettings";
            this.btnProfileSettings.Size = new System.Drawing.Size(150, 40);
            this.btnProfileSettings.TabIndex = 4;
            this.btnProfileSettings.Text = "Profile Settings";
            this.btnProfileSettings.UseVisualStyleBackColor = true;
            this.btnProfileSettings.Click += new System.EventHandler(this.btnProfileSettings_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogout.Location = new System.Drawing.Point(450, 25);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(80, 30);
            this.btnLogout.TabIndex = 5;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // UserDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 320);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnProfileSettings);
            this.Controls.Add(this.btnViewPayslip);
            this.Controls.Add(this.btnViewAttendance);
            this.Controls.Add(this.groupBoxSummary);
            this.Controls.Add(this.lblWelcome);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "UserDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "User Dashboard";
            this.Load += new System.EventHandler(this.UserDashboard_Load);
            this.groupBoxSummary.ResumeLayout(false);
            this.groupBoxSummary.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.GroupBox groupBoxSummary;
        private System.Windows.Forms.Label lblSummaryPlaceholder;
        private System.Windows.Forms.Button btnViewAttendance;
        private System.Windows.Forms.Button btnViewPayslip;
        private System.Windows.Forms.Button btnProfileSettings;
        private System.Windows.Forms.Button btnLogout;
    }
}

