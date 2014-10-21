using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BIETLUtility.Configuration;

namespace BIETLTool
{
    public partial class OutputSetting : Form
    {
        public StepSetting OutputStep { get; set; }

        public OutputSetting()
        {
            InitializeComponent();
        }

        private void OutputSetting_Load(object sender, EventArgs e)
        {
            var query = from q in Transformations.Instance().GetConnections()
                        where q.ConnectionType == ConnectionType.Vertica
                        select q.Name;

            this.comboBoxConnection.DataSource = query.ToList();

            if (this.OutputStep != null)
            {
                this.txtName.Text = this.OutputStep.Name;
                this.txtTargetTable.Text = this.OutputStep.TargetTable;
                this.txtName.Enabled = false;
                this.comboBoxConnection.SelectedItem = this.OutputStep.Connection.Name;
            }
            else
            {
                this.OutputStep = new StepSetting();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (this.comboBoxConnection.SelectedIndex > -1)
            {
                DatabaseConnection dbconnection = new DatabaseConnection();
                dbconnection.Connection = Transformations.Instance().GetConnection(this.comboBoxConnection.SelectedItem.ToString());
                dbconnection.ShowDialog();
            }
            else
            {
                MessageBox.Show("No connection selected!");
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            DatabaseConnection dbconnection = new DatabaseConnection();
            dbconnection.Connection = new Connection();
            dbconnection.Connection.ConnectionType = ConnectionType.Vertica;

            if (dbconnection.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Transformations.Instance().CurrentTranformation.AddConnection(dbconnection.Connection, OutputStep);

                var query = from q in Transformations.Instance().GetConnections()
                            where q.ConnectionType == ConnectionType.Vertica
                            select q.Name;

                this.comboBoxConnection.DataSource = query.ToList();
                this.comboBoxConnection.SelectedItem = this.OutputStep.Connection.Name;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtName.Text) || this.comboBoxConnection.SelectedItem == null)
            {
                MessageBox.Show("Name and connection needed!");
                return;
            }

            this.OutputStep.Name = this.txtName.Text;
            this.OutputStep.TargetTable = this.txtTargetTable.Text;
            this.OutputStep.Connection = Transformations.Instance().GetConnection(this.comboBoxConnection.SelectedValue.ToString());

            DialogResult = System.Windows.Forms.DialogResult.OK;

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;

            this.Close();
        }
    }
}
