namespace PayrollSample
{
    partial class FrmPayslip
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
            this.groupBoxPayslip = new System.Windows.Forms.GroupBox();
            this.lblNetPay = new System.Windows.Forms.Label();
            this.lblNetPayValue = new System.Windows.Forms.Label();
            this.lblLateDeduction = new System.Windows.Forms.Label();
            this.lblLateDeductionValue = new System.Windows.Forms.Label();
            this.lblTotalDeductions = new System.Windows.Forms.Label();
            this.lblTotalDeductionsValue = new System.Windows.Forms.Label();
            this.lblGrossPay = new System.Windows.Forms.Label();
            this.lblGrossPayValue = new System.Windows.Forms.Label();
            this.lblHourlyRate = new System.Windows.Forms.Label();
            this.lblHourlyRateValue = new System.Windows.Forms.Label();
            this.lblTotalHours = new System.Windows.Forms.Label();
            this.lblTotalHoursValue = new System.Windows.Forms.Label();
            this.lblPayrollPeriod = new System.Windows.Forms.Label();
            this.lblPayrollPeriodValue = new System.Windows.Forms.Label();
            this.lblEmployeeName = new System.Windows.Forms.Label();
            this.lblEmployeeNameValue = new System.Windows.Forms.Label();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnDownloadPayslip = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBoxPayslip.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxPayslip
            // 
            this.groupBoxPayslip.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxPayslip.Controls.Add(this.lblNetPay);
            this.groupBoxPayslip.Controls.Add(this.lblNetPayValue);
            this.groupBoxPayslip.Controls.Add(this.lblTotalDeductions);
            this.groupBoxPayslip.Controls.Add(this.lblTotalDeductionsValue);
            this.groupBoxPayslip.Controls.Add(this.lblLateDeduction);
            this.groupBoxPayslip.Controls.Add(this.lblLateDeductionValue);
            this.groupBoxPayslip.Controls.Add(this.lblGrossPay);
            this.groupBoxPayslip.Controls.Add(this.lblGrossPayValue);
            this.groupBoxPayslip.Controls.Add(this.lblHourlyRate);
            this.groupBoxPayslip.Controls.Add(this.lblHourlyRateValue);
            this.groupBoxPayslip.Controls.Add(this.lblTotalHours);
            this.groupBoxPayslip.Controls.Add(this.lblTotalHoursValue);
            this.groupBoxPayslip.Controls.Add(this.lblPayrollPeriod);
            this.groupBoxPayslip.Controls.Add(this.lblPayrollPeriodValue);
            this.groupBoxPayslip.Controls.Add(this.lblEmployeeName);
            this.groupBoxPayslip.Controls.Add(this.lblEmployeeNameValue);
            this.groupBoxPayslip.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxPayslip.Location = new System.Drawing.Point(12, 12);
            this.groupBoxPayslip.Name = "groupBoxPayslip";
            this.groupBoxPayslip.Size = new System.Drawing.Size(560, 400);
            this.groupBoxPayslip.TabIndex = 0;
            this.groupBoxPayslip.TabStop = false;
            this.groupBoxPayslip.Text = "Payslip Details";
            // 
            // lblNetPay
            // 
            this.lblNetPay.AutoSize = true;
            this.lblNetPay.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNetPay.Location = new System.Drawing.Point(30, 340);
            this.lblNetPay.Name = "lblNetPay";
            this.lblNetPay.Size = new System.Drawing.Size(75, 17);
            this.lblNetPay.TabIndex = 13;
            this.lblNetPay.Text = "Net Pay:";
            // 
            // lblNetPayValue
            // 
            this.lblNetPayValue.AutoSize = true;
            this.lblNetPayValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNetPayValue.ForeColor = System.Drawing.Color.Green;
            this.lblNetPayValue.Location = new System.Drawing.Point(200, 338);
            this.lblNetPayValue.Name = "lblNetPayValue";
            this.lblNetPayValue.Size = new System.Drawing.Size(57, 20);
            this.lblNetPayValue.TabIndex = 12;
            this.lblNetPayValue.Text = "$0.00";
            // 
            // lblLateDeduction
            // 
            this.lblLateDeduction.AutoSize = true;
            this.lblLateDeduction.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLateDeduction.Location = new System.Drawing.Point(30, 260);
            this.lblLateDeduction.Name = "lblLateDeduction";
            this.lblLateDeduction.Size = new System.Drawing.Size(109, 17);
            this.lblLateDeduction.TabIndex = 11;
            this.lblLateDeduction.Text = "Late Deduction:";
            // 
            // lblLateDeductionValue
            // 
            this.lblLateDeductionValue.AutoSize = true;
            this.lblLateDeductionValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLateDeductionValue.Location = new System.Drawing.Point(200, 260);
            this.lblLateDeductionValue.Name = "lblLateDeductionValue";
            this.lblLateDeductionValue.Size = new System.Drawing.Size(40, 17);
            this.lblLateDeductionValue.TabIndex = 12;
            this.lblLateDeductionValue.Text = "$0.00";
            // 
            // lblTotalDeductions
            // 
            this.lblTotalDeductions.AutoSize = true;
            this.lblTotalDeductions.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalDeductions.Location = new System.Drawing.Point(30, 300);
            this.lblTotalDeductions.Name = "lblTotalDeductions";
            this.lblTotalDeductions.Size = new System.Drawing.Size(123, 17);
            this.lblTotalDeductions.TabIndex = 13;
            this.lblTotalDeductions.Text = "Total Deductions:";
            // 
            // lblTotalDeductionsValue
            // 
            this.lblTotalDeductionsValue.AutoSize = true;
            this.lblTotalDeductionsValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalDeductionsValue.Location = new System.Drawing.Point(200, 300);
            this.lblTotalDeductionsValue.Name = "lblTotalDeductionsValue";
            this.lblTotalDeductionsValue.Size = new System.Drawing.Size(40, 17);
            this.lblTotalDeductionsValue.TabIndex = 14;
            this.lblTotalDeductionsValue.Text = "$0.00";
            // 
            // lblGrossPay
            // 
            this.lblGrossPay.AutoSize = true;
            this.lblGrossPay.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGrossPay.Location = new System.Drawing.Point(30, 240);
            this.lblGrossPay.Name = "lblGrossPay";
            this.lblGrossPay.Size = new System.Drawing.Size(80, 17);
            this.lblGrossPay.TabIndex = 9;
            this.lblGrossPay.Text = "Gross Pay:";
            // 
            // lblGrossPayValue
            // 
            this.lblGrossPayValue.AutoSize = true;
            this.lblGrossPayValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGrossPayValue.Location = new System.Drawing.Point(200, 240);
            this.lblGrossPayValue.Name = "lblGrossPayValue";
            this.lblGrossPayValue.Size = new System.Drawing.Size(40, 17);
            this.lblGrossPayValue.TabIndex = 8;
            this.lblGrossPayValue.Text = "$0.00";
            // 
            // lblHourlyRate
            // 
            this.lblHourlyRate.AutoSize = true;
            this.lblHourlyRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHourlyRate.Location = new System.Drawing.Point(30, 200);
            this.lblHourlyRate.Name = "lblHourlyRate";
            this.lblHourlyRate.Size = new System.Drawing.Size(90, 17);
            this.lblHourlyRate.TabIndex = 7;
            this.lblHourlyRate.Text = "Hourly Rate:";
            // 
            // lblHourlyRateValue
            // 
            this.lblHourlyRateValue.AutoSize = true;
            this.lblHourlyRateValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHourlyRateValue.Location = new System.Drawing.Point(200, 200);
            this.lblHourlyRateValue.Name = "lblHourlyRateValue";
            this.lblHourlyRateValue.Size = new System.Drawing.Size(40, 17);
            this.lblHourlyRateValue.TabIndex = 6;
            this.lblHourlyRateValue.Text = "$0.00";
            // 
            // lblTotalHours
            // 
            this.lblTotalHours.AutoSize = true;
            this.lblTotalHours.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalHours.Location = new System.Drawing.Point(30, 160);
            this.lblTotalHours.Name = "lblTotalHours";
            this.lblTotalHours.Size = new System.Drawing.Size(90, 17);
            this.lblTotalHours.TabIndex = 5;
            this.lblTotalHours.Text = "Total Hours:";
            // 
            // lblTotalHoursValue
            // 
            this.lblTotalHoursValue.AutoSize = true;
            this.lblTotalHoursValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalHoursValue.Location = new System.Drawing.Point(200, 160);
            this.lblTotalHoursValue.Name = "lblTotalHoursValue";
            this.lblTotalHoursValue.Size = new System.Drawing.Size(40, 17);
            this.lblTotalHoursValue.TabIndex = 4;
            this.lblTotalHoursValue.Text = "0.00";
            // 
            // lblPayrollPeriod
            // 
            this.lblPayrollPeriod.AutoSize = true;
            this.lblPayrollPeriod.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPayrollPeriod.Location = new System.Drawing.Point(30, 120);
            this.lblPayrollPeriod.Name = "lblPayrollPeriod";
            this.lblPayrollPeriod.Size = new System.Drawing.Size(108, 17);
            this.lblPayrollPeriod.TabIndex = 3;
            this.lblPayrollPeriod.Text = "Payroll Period:";
            // 
            // lblPayrollPeriodValue
            // 
            this.lblPayrollPeriodValue.AutoSize = true;
            this.lblPayrollPeriodValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPayrollPeriodValue.Location = new System.Drawing.Point(200, 120);
            this.lblPayrollPeriodValue.Name = "lblPayrollPeriodValue";
            this.lblPayrollPeriodValue.Size = new System.Drawing.Size(46, 17);
            this.lblPayrollPeriodValue.TabIndex = 2;
            this.lblPayrollPeriodValue.Text = "N/A";
            // 
            // lblEmployeeName
            // 
            this.lblEmployeeName.AutoSize = true;
            this.lblEmployeeName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmployeeName.Location = new System.Drawing.Point(30, 50);
            this.lblEmployeeName.Name = "lblEmployeeName";
            this.lblEmployeeName.Size = new System.Drawing.Size(130, 20);
            this.lblEmployeeName.TabIndex = 1;
            this.lblEmployeeName.Text = "Employee Name:";
            // 
            // lblEmployeeNameValue
            // 
            this.lblEmployeeNameValue.AutoSize = true;
            this.lblEmployeeNameValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmployeeNameValue.Location = new System.Drawing.Point(200, 50);
            this.lblEmployeeNameValue.Name = "lblEmployeeNameValue";
            this.lblEmployeeNameValue.Size = new System.Drawing.Size(35, 20);
            this.lblEmployeeNameValue.TabIndex = 0;
            this.lblEmployeeNameValue.Text = "N/A";
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.btnDownloadPayslip);
            this.panelButtons.Controls.Add(this.btnClose);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(0, 410);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(584, 50);
            this.panelButtons.TabIndex = 1;
            // 
            // btnDownloadPayslip
            // 
            this.btnDownloadPayslip.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDownloadPayslip.Location = new System.Drawing.Point(200, 10);
            this.btnDownloadPayslip.Name = "btnDownloadPayslip";
            this.btnDownloadPayslip.Size = new System.Drawing.Size(150, 35);
            this.btnDownloadPayslip.TabIndex = 1;
            this.btnDownloadPayslip.Text = "Download Payslip";
            this.btnDownloadPayslip.UseVisualStyleBackColor = true;
            this.btnDownloadPayslip.Enabled = false;
            this.btnDownloadPayslip.Click += new System.EventHandler(this.btnDownloadPayslip_Click);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(360, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 35);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FrmPayslip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 460);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.groupBoxPayslip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmPayslip";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "View Payslip";
            this.groupBoxPayslip.ResumeLayout(false);
            this.groupBoxPayslip.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxPayslip;
        private System.Windows.Forms.Label lblEmployeeName;
        private System.Windows.Forms.Label lblEmployeeNameValue;
        private System.Windows.Forms.Label lblPayrollPeriod;
        private System.Windows.Forms.Label lblPayrollPeriodValue;
        private System.Windows.Forms.Label lblTotalHours;
        private System.Windows.Forms.Label lblTotalHoursValue;
        private System.Windows.Forms.Label lblHourlyRate;
        private System.Windows.Forms.Label lblHourlyRateValue;
        private System.Windows.Forms.Label lblGrossPay;
        private System.Windows.Forms.Label lblGrossPayValue;
        private System.Windows.Forms.Label lblTotalDeductions;
        private System.Windows.Forms.Label lblTotalDeductionsValue;
        private System.Windows.Forms.Label lblLateDeduction;
        private System.Windows.Forms.Label lblLateDeductionValue;
        private System.Windows.Forms.Label lblNetPay;
        private System.Windows.Forms.Label lblNetPayValue;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnDownloadPayslip;
        private System.Windows.Forms.Button btnClose;
    }
}

