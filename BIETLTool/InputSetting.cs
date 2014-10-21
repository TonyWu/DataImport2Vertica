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
    public partial class InputSetting : Form
    {
        public StepSetting InputStep { get; set; }

        public InputSetting()
        {
            InitializeComponent();
        }

        private void InputSetting_Load(object sender, EventArgs e)
        {
            var query = from q in Transformations.Instance().GetConnections()
                    //where q.ConnectionType == ConnectionType.MSSQLSERVER
                    select q.Name;

            this.comboBoxConnection.DataSource = query.ToList();
            
            if (this.InputStep != null)
            {
                this.txtName.Text = this.InputStep.Name;
                this.txtName.Enabled = false;
                this.comboBoxConnection.SelectedItem = this.InputStep.Connection.Name;
                this.checkBoxEnableSQL.Checked = this.InputStep.EnableSQL;
                this.txtTargetTable.Text = this.InputStep.TargetTable;

                if (this.InputStep.EnableSQL)
                {
                    this.txtSql.Enabled = true;
                    this.txtSql.Text = this.InputStep.SQLStatement;
                }
                else
                {
                    this.txtSql.Enabled = false;
                }
            }
            else
            {
                this.InputStep = new StepSetting();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtName.Text) || this.comboBoxConnection.SelectedItem == null)
            {
                MessageBox.Show("Name and connection needed!");
                return;
            }

            if (this.checkBoxEnableSQL.Checked && string.IsNullOrWhiteSpace(this.txtSql.Text))
            {
                MessageBox.Show("Sql statement needed!");
                return;
            }

            this.InputStep.Name = this.txtName.Text;
            this.InputStep.Connection = Transformations.Instance().GetConnection(this.comboBoxConnection.SelectedValue.ToString());
            this.InputStep.EnableSQL = this.checkBoxEnableSQL.Checked;
            this.InputStep.SQLStatement = this.InputStep.EnableSQL ? this.txtSql.Text : string.Empty;
            this.InputStep.TargetTable = txtTargetTable.Text;

            DialogResult = System.Windows.Forms.DialogResult.OK;

            this.Close();
        }

        private void btnNewConnection_Click(object sender, EventArgs e)
        {
            DatabaseConnection dbconnection = new DatabaseConnection();
            dbconnection.Connection = new Connection();
            dbconnection.Connection.ConnectionType = ConnectionType.MSSQLSERVER;

            if (dbconnection.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Transformations.Instance().CurrentTranformation.AddConnection(dbconnection.Connection, InputStep);

                var query = from q in Transformations.Instance().GetConnections()
                            //where q.ConnectionType == ConnectionType.MSSQLSERVER
                            select q.Name;

                this.comboBoxConnection.DataSource = query.ToList();
                this.comboBoxConnection.SelectedItem = this.InputStep.Connection.Name;
            }
        }

        private void btnEditConnection_Click(object sender, EventArgs e)
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void checkBoxEnableSQL_CheckedChanged(object sender, EventArgs e)
        {
            this.txtSql.Enabled = this.checkBoxEnableSQL.Checked;
        }

        private void btnNewVertica_Click(object sender, EventArgs e)
        {
            DatabaseConnection dbconnection = new DatabaseConnection();
            dbconnection.Connection = new Connection();
            dbconnection.Connection.ConnectionType = ConnectionType.Vertica;

            if (dbconnection.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Transformations.Instance().CurrentTranformation.AddConnection(dbconnection.Connection, InputStep);

                var query = from q in Transformations.Instance().GetConnections()
                            //where q.ConnectionType == ConnectionType.Vertica
                            select q.Name;

                this.comboBoxConnection.DataSource = query.ToList();
                this.comboBoxConnection.SelectedItem = this.InputStep.Connection.Name;
            }
        }
    }
}
