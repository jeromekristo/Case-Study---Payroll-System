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
            this.lblHoursWorkedValue = new System.Windows.Forms.Label();
            this.lblHoursWorkedLabel = new System.Windows.Forms.Label();
            this.lblTimeOutValue = new System.Windows.Forms.Label();
            this.lblTimeOutLabel = new System.Windows.Forms.Label();
            this.lblTimeInValue = new System.Windows.Forms.Label();
            this.lblTimeInLabel = new System.Windows.Forms.Label();
            this.lblTodayStatus = new System.Windows.Forms.Label();
            this.btnTimeIn = new System.Windows.Forms.Button();
            this.btnTimeOut = new System.Windows.Forms.Button();
            this.btnViewAttendance = new System.Windows.Forms.Button();
            this.btnViewPayslip = new System.Windows.Forms.Button();
            this.btnProfileSettings = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.groupBoxHistory = new System.Windows.Forms.GroupBox();
            this.dgvAttendanceHistory = new System.Windows.Forms.DataGridView();
            this.lblHistoryStatus = new System.Windows.Forms.Label();
            this.groupBoxSummary.SuspendLayout();
            this.groupBoxHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttendanceHistory)).BeginInit();
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
            this.lblWelcome.Click += new System.EventHandler(this.lblWelcome_Click);
            // 
            // groupBoxSummary
            // 
            this.groupBoxSummary.Controls.Add(this.lblHoursWorkedValue);
            this.groupBoxSummary.Controls.Add(this.lblHoursWorkedLabel);
            this.groupBoxSummary.Controls.Add(this.lblTimeOutValue);
            this.groupBoxSummary.Controls.Add(this.lblTimeOutLabel);
            this.groupBoxSummary.Controls.Add(this.lblTimeInValue);
            this.groupBoxSummary.Controls.Add(this.lblTimeInLabel);
            this.groupBoxSummary.Controls.Add(this.lblTodayStatus);
            this.groupBoxSummary.Location = new System.Drawing.Point(30, 70);
            this.groupBoxSummary.Name = "groupBoxSummary";
            this.groupBoxSummary.Size = new System.Drawing.Size(500, 170);
            this.groupBoxSummary.TabIndex = 1;
            this.groupBoxSummary.TabStop = false;
            this.groupBoxSummary.Text = "Summary";
            // 
            // lblHoursWorkedValue
            // 
            this.lblHoursWorkedValue.AutoSize = true;
            this.lblHoursWorkedValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHoursWorkedValue.Location = new System.Drawing.Point(133, 130);
            this.lblHoursWorkedValue.Name = "lblHoursWorkedValue";
            this.lblHoursWorkedValue.Size = new System.Drawing.Size(51, 15);
            this.lblHoursWorkedValue.TabIndex = 6;
            this.lblHoursWorkedValue.Text = "0.00 hrs";
            // 
            // lblHoursWorkedLabel
            // 
            this.lblHoursWorkedLabel.AutoSize = true;
            this.lblHoursWorkedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHoursWorkedLabel.Location = new System.Drawing.Point(20, 130);
            this.lblHoursWorkedLabel.Name = "lblHoursWorkedLabel";
            this.lblHoursWorkedLabel.Size = new System.Drawing.Size(101, 15);
            this.lblHoursWorkedLabel.TabIndex = 5;
            this.lblHoursWorkedLabel.Text = "Hours Worked:";
            // 
            // lblTimeOutValue
            // 
            this.lblTimeOutValue.AutoSize = true;
            this.lblTimeOutValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeOutValue.Location = new System.Drawing.Point(120, 100);
            this.lblTimeOutValue.Name = "lblTimeOutValue";
            this.lblTimeOutValue.Size = new System.Drawing.Size(26, 15);
            this.lblTimeOutValue.TabIndex = 4;
            this.lblTimeOutValue.Text = "--:--";
            // 
            // lblTimeOutLabel
            // 
            this.lblTimeOutLabel.AutoSize = true;
            this.lblTimeOutLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeOutLabel.Location = new System.Drawing.Point(20, 100);
            this.lblTimeOutLabel.Name = "lblTimeOutLabel";
            this.lblTimeOutLabel.Size = new System.Drawing.Size(69, 15);
            this.lblTimeOutLabel.TabIndex = 3;
            this.lblTimeOutLabel.Text = "Time Out:";
            // 
            // lblTimeInValue
            // 
            this.lblTimeInValue.AutoSize = true;
            this.lblTimeInValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeInValue.Location = new System.Drawing.Point(120, 70);
            this.lblTimeInValue.Name = "lblTimeInValue";
            this.lblTimeInValue.Size = new System.Drawing.Size(26, 15);
            this.lblTimeInValue.TabIndex = 2;
            this.lblTimeInValue.Text = "--:--";
            // 
            // lblTimeInLabel
            // 
            this.lblTimeInLabel.AutoSize = true;
            this.lblTimeInLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeInLabel.Location = new System.Drawing.Point(20, 70);
            this.lblTimeInLabel.Name = "lblTimeInLabel";
            this.lblTimeInLabel.Size = new System.Drawing.Size(59, 15);
            this.lblTimeInLabel.TabIndex = 1;
            this.lblTimeInLabel.Text = "Time In:";
            // 
            // lblTodayStatus
            // 
            this.lblTodayStatus.AutoSize = true;
            this.lblTodayStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTodayStatus.Location = new System.Drawing.Point(20, 30);
            this.lblTodayStatus.Name = "lblTodayStatus";
            this.lblTodayStatus.Size = new System.Drawing.Size(198, 16);
            this.lblTodayStatus.TabIndex = 0;
            this.lblTodayStatus.Text = "No attendance record yet today.";
            // 
            // btnTimeIn
            // 
            this.btnTimeIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTimeIn.Location = new System.Drawing.Point(30, 260);
            this.btnTimeIn.Name = "btnTimeIn";
            this.btnTimeIn.Size = new System.Drawing.Size(150, 40);
            this.btnTimeIn.TabIndex = 2;
            this.btnTimeIn.Text = "Time In";
            this.btnTimeIn.UseVisualStyleBackColor = true;
            this.btnTimeIn.Click += new System.EventHandler(this.btnTimeIn_Click);
            // 
            // btnTimeOut
            // 
            this.btnTimeOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTimeOut.Location = new System.Drawing.Point(200, 260);
            this.btnTimeOut.Name = "btnTimeOut";
            this.btnTimeOut.Size = new System.Drawing.Size(150, 40);
            this.btnTimeOut.TabIndex = 3;
            this.btnTimeOut.Text = "Time Out";
            this.btnTimeOut.UseVisualStyleBackColor = true;
            this.btnTimeOut.Click += new System.EventHandler(this.btnTimeOut_Click);
            // 
            // btnViewAttendance
            // 
            this.btnViewAttendance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewAttendance.Location = new System.Drawing.Point(30, 320);
            this.btnViewAttendance.Name = "btnViewAttendance";
            this.btnViewAttendance.Size = new System.Drawing.Size(150, 40);
            this.btnViewAttendance.TabIndex = 4;
            this.btnViewAttendance.Text = "View Attendance";
            this.btnViewAttendance.UseVisualStyleBackColor = true;
            this.btnViewAttendance.Click += new System.EventHandler(this.btnViewAttendance_Click);
            // 
            // btnViewPayslip
            // 
            this.btnViewPayslip.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewPayslip.Location = new System.Drawing.Point(200, 320);
            this.btnViewPayslip.Name = "btnViewPayslip";
            this.btnViewPayslip.Size = new System.Drawing.Size(150, 40);
            this.btnViewPayslip.TabIndex = 5;
            this.btnViewPayslip.Text = "View Payslip";
            this.btnViewPayslip.UseVisualStyleBackColor = true;
            this.btnViewPayslip.Click += new System.EventHandler(this.btnViewPayslip_Click);
            // 
            // btnProfileSettings
            // 
            this.btnProfileSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProfileSettings.Location = new System.Drawing.Point(294, 21);
            this.btnProfileSettings.Name = "btnProfileSettings";
            this.btnProfileSettings.Size = new System.Drawing.Size(150, 40);
            this.btnProfileSettings.TabIndex = 6;
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
            // groupBoxHistory
            // 
            this.groupBoxHistory.Controls.Add(this.dgvAttendanceHistory);
            this.groupBoxHistory.Controls.Add(this.lblHistoryStatus);
            this.groupBoxHistory.Location = new System.Drawing.Point(30, 370);
            this.groupBoxHistory.Name = "groupBoxHistory";
            this.groupBoxHistory.Size = new System.Drawing.Size(500, 230);
            this.groupBoxHistory.TabIndex = 7;
            this.groupBoxHistory.TabStop = false;
            this.groupBoxHistory.Text = "Attendance History";
            // 
            // dgvAttendanceHistory
            // 
            this.dgvAttendanceHistory.AllowUserToAddRows = false;
            this.dgvAttendanceHistory.AllowUserToDeleteRows = false;
            this.dgvAttendanceHistory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAttendanceHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAttendanceHistory.Location = new System.Drawing.Point(20, 60);
            this.dgvAttendanceHistory.MultiSelect = false;
            this.dgvAttendanceHistory.Name = "dgvAttendanceHistory";
            this.dgvAttendanceHistory.ReadOnly = true;
            this.dgvAttendanceHistory.RowHeadersVisible = false;
            this.dgvAttendanceHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAttendanceHistory.Size = new System.Drawing.Size(460, 150);
            this.dgvAttendanceHistory.TabIndex = 1;
            // 
            // lblHistoryStatus
            // 
            this.lblHistoryStatus.AutoSize = true;
            this.lblHistoryStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHistoryStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblHistoryStatus.Location = new System.Drawing.Point(20, 30);
            this.lblHistoryStatus.Name = "lblHistoryStatus";
            this.lblHistoryStatus.Size = new System.Drawing.Size(215, 15);
            this.lblHistoryStatus.TabIndex = 0;
            this.lblHistoryStatus.Text = "Click \"View Attendance\" to load history.";
            // 
            // UserDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 630);
            this.Controls.Add(this.groupBoxHistory);
            this.Controls.Add(this.btnTimeOut);
            this.Controls.Add(this.btnTimeIn);
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
            this.groupBoxHistory.ResumeLayout(false);
            this.groupBoxHistory.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttendanceHistory)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.GroupBox groupBoxSummary;
        private System.Windows.Forms.Label lblTodayStatus;
        private System.Windows.Forms.Label lblTimeInLabel;
        private System.Windows.Forms.Label lblTimeInValue;
        private System.Windows.Forms.Label lblTimeOutLabel;
        private System.Windows.Forms.Label lblTimeOutValue;
        private System.Windows.Forms.Label lblHoursWorkedLabel;
        private System.Windows.Forms.Label lblHoursWorkedValue;
        private System.Windows.Forms.Button btnTimeIn;
        private System.Windows.Forms.Button btnTimeOut;
        private System.Windows.Forms.Button btnViewAttendance;
        private System.Windows.Forms.Button btnViewPayslip;
        private System.Windows.Forms.Button btnProfileSettings;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.GroupBox groupBoxHistory;
        private System.Windows.Forms.DataGridView dgvAttendanceHistory;
        private System.Windows.Forms.Label lblHistoryStatus;
    }
}

