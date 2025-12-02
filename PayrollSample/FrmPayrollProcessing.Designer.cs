namespace PayrollSample
{
    partial class FrmPayrollProcessing
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
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblCutoffInfo = new System.Windows.Forms.Label();
            this.btnLoadAttendance = new System.Windows.Forms.Button();
            this.btnGeneratePayroll = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.dataGridViewPayroll = new System.Windows.Forms.DataGridView();
            this.panelDateRange = new System.Windows.Forms.Panel();
            this.panelButtons = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPayroll)).BeginInit();
            this.panelDateRange.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // dtpFrom
            // 
            this.dtpFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(71, 15);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(180, 26);
            this.dtpFrom.TabIndex = 0;
            // 
            // dtpTo
            // 
            this.dtpTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(309, 15);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(180, 26);
            this.dtpTo.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "From:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(270, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "To:";
            // 
            // lblCutoffInfo
            // 
            this.lblCutoffInfo.AutoSize = true;
            this.lblCutoffInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCutoffInfo.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblCutoffInfo.Location = new System.Drawing.Point(510, 18);
            this.lblCutoffInfo.Name = "lblCutoffInfo";
            this.lblCutoffInfo.Size = new System.Drawing.Size(250, 18);
            this.lblCutoffInfo.TabIndex = 4;
            this.lblCutoffInfo.Text = "Cutoff Period: 1-15 or 16-End of Month";
            // 
            // btnLoadAttendance
            // 
            this.btnLoadAttendance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadAttendance.Location = new System.Drawing.Point(15, 10);
            this.btnLoadAttendance.Name = "btnLoadAttendance";
            this.btnLoadAttendance.Size = new System.Drawing.Size(180, 40);
            this.btnLoadAttendance.TabIndex = 4;
            this.btnLoadAttendance.Text = "Load Attendance";
            this.btnLoadAttendance.UseVisualStyleBackColor = true;
            this.btnLoadAttendance.Click += new System.EventHandler(this.btnLoadAttendance_Click);
            // 
            // btnGeneratePayroll
            // 
            this.btnGeneratePayroll.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGeneratePayroll.Location = new System.Drawing.Point(201, 10);
            this.btnGeneratePayroll.Name = "btnGeneratePayroll";
            this.btnGeneratePayroll.Size = new System.Drawing.Size(180, 40);
            this.btnGeneratePayroll.TabIndex = 5;
            this.btnGeneratePayroll.Text = "Generate Payroll";
            this.btnGeneratePayroll.UseVisualStyleBackColor = true;
            this.btnGeneratePayroll.Click += new System.EventHandler(this.btnGeneratePayroll_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(850, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(120, 40);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // dataGridViewPayroll
            // 
            this.dataGridViewPayroll.AllowUserToAddRows = false;
            this.dataGridViewPayroll.AllowUserToDeleteRows = false;
            this.dataGridViewPayroll.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewPayroll.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewPayroll.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewPayroll.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPayroll.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridViewPayroll.Location = new System.Drawing.Point(12, 120);
            this.dataGridViewPayroll.MultiSelect = false;
            this.dataGridViewPayroll.Name = "dataGridViewPayroll";
            this.dataGridViewPayroll.ReadOnly = true;
            this.dataGridViewPayroll.RowTemplate.Height = 30;
            this.dataGridViewPayroll.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewPayroll.Size = new System.Drawing.Size(960, 400);
            this.dataGridViewPayroll.TabIndex = 6;
            // 
            // panelDateRange
            // 
            this.panelDateRange.Controls.Add(this.label1);
            this.panelDateRange.Controls.Add(this.dtpFrom);
            this.panelDateRange.Controls.Add(this.label2);
            this.panelDateRange.Controls.Add(this.dtpTo);
            this.panelDateRange.Controls.Add(this.lblCutoffInfo);
            this.panelDateRange.Location = new System.Drawing.Point(12, 12);
            this.panelDateRange.Name = "panelDateRange";
            this.panelDateRange.Size = new System.Drawing.Size(700, 50);
            this.panelDateRange.TabIndex = 7;
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.btnLoadAttendance);
            this.panelButtons.Controls.Add(this.btnGeneratePayroll);
            this.panelButtons.Controls.Add(this.btnClose);
            this.panelButtons.Location = new System.Drawing.Point(12, 68);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(960, 50);
            this.panelButtons.TabIndex = 8;
            // 
            // FrmPayrollProcessing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 532);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.panelDateRange);
            this.Controls.Add(this.dataGridViewPayroll);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.Name = "FrmPayrollProcessing";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Payroll Processing";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblCutoffInfo;
        private System.Windows.Forms.Button btnLoadAttendance;
        private System.Windows.Forms.Button btnGeneratePayroll;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView dataGridViewPayroll;
        private System.Windows.Forms.Panel panelDateRange;
        private System.Windows.Forms.Panel panelButtons;
    }
}

