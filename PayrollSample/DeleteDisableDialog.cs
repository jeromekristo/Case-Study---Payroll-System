using System;
using System.Windows.Forms;

namespace PayrollSample
{
    public partial class DeleteDisableDialog : Form
    {
        public DialogResult UserChoice { get; private set; }

        public DeleteDisableDialog()
        {
            InitializeComponent();
            UserChoice = DialogResult.Cancel;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            UserChoice = DialogResult.Yes; // Yes = Delete
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void btnDisable_Click(object sender, EventArgs e)
        {
            UserChoice = DialogResult.No; // No = Disable
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            UserChoice = DialogResult.Cancel;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

