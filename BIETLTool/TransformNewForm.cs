using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BIETLTool
{
    public partial class TransformNewForm : Form
    {
        public string Name { get; set; }
        public string Location { get; set; }

        public TransformNewForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtName.Text) || string.IsNullOrWhiteSpace(this.txtLocation.Text))
            {
                MessageBox.Show("Please input name and location!");

                return;
            }

            this.Name = this.txtName.Text.Trim();
            this.Location = this.txtLocation.Text.Trim();

            DialogResult = System.Windows.Forms.DialogResult.OK;

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "XML Files (*.xml)|*.xml";

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.txtLocation.Text = dialog.FileName;
            }
        }
    }
}
