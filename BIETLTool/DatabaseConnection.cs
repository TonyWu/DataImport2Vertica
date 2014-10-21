using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BIETLUtility.Configuration;
using Vertica.Data.VerticaClient;

namespace BIETLTool
{
    public partial class DatabaseConnection : Form
    {
        public Connection Connection { get; set; }

        public DatabaseConnection()
        {
            InitializeComponent();
        }

        private void DatabaseConnection_Load(object sender, EventArgs e)
        {
            if (this.Connection != null && !string.IsNullOrWhiteSpace(this.Connection.Name))
            {
                this.txtName.Text = this.Connection.Name;
                this.txtName.Enabled = false;
                this.txtServer.Text = this.Connection.Host;
                this.txtUsername.Text = this.Connection.Username;
                this.txtPassword.Text = this.Connection.Password;
                if (this.Connection.ConnectionType == ConnectionType.MSSQLSERVER)
                {
                    this.comboBoxSqlDB.Text = this.Connection.Database;
                }
                else
                {
                    this.txtDatabase.Text = this.Connection.Database;
                }

                this.txtTable.Text = this.Connection.TargetTable;
            }

            if (this.Connection.ConnectionType == ConnectionType.MSSQLSERVER)
            {
                this.txtDatabase.Visible = false;
            }
            else
            {
                this.comboBoxSqlDB.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!this.Validation())
            {
                MessageBox.Show("Some values are missing!");
            }
            else
            {
                this.Connection.Name = this.txtName.Text;
                this.Connection.Host = this.txtServer.Text;
                this.Connection.Username = this.txtUsername.Text;
                this.Connection.Password = this.txtPassword.Text;
                this.Connection.Database = this.Connection.ConnectionType == ConnectionType.MSSQLSERVER ? this.comboBoxSqlDB.Text : this.txtDatabase.Text;
                this.Connection.TargetTable = this.txtTable.Text;
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;

            this.Close();
        }

        private bool Validation()
        {
            if (string.IsNullOrWhiteSpace(this.txtName.Text)
                || string.IsNullOrWhiteSpace(this.txtServer.Text)
                || string.IsNullOrWhiteSpace(this.txtUsername.Text)
                || string.IsNullOrWhiteSpace(this.txtPassword.Text)
                || (this.Connection.ConnectionType == ConnectionType.MSSQLSERVER?string.IsNullOrWhiteSpace(this.comboBoxSqlDB.Text) :string.IsNullOrWhiteSpace(this.txtDatabase.Text)))
                return false;
            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void comboBoxSqlDB_DropDown(object sender, EventArgs e)
        {
            try
            {
                if (!this.txtServer.Text.Equals(string.Empty))
                {
                    this.comboBoxSqlDB.Items.Clear();
                    using (SqlConnection connection = new SqlConnection(string.Format("server={0};initial catalog=master;uid={1};password={2};", this.txtServer.Text, this.txtUsername.Text, this.txtPassword.Text)))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("sp_databases", connection);
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            this.comboBoxSqlDB.Items.Add(reader["DATABASE_NAME"]);
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //errorProvider1.SetError(originDatabase, "Error occured, Does Origin Server have a value? Check username and password" + ex.InnerException.ToString());
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
           string error;
           if (this.TestConnection(out error))
           {
               MessageBox.Show("Test connection suceed!");
           }
           else
           {
               MessageBox.Show("Test failed." + error);
           }
        }

        public bool TestConnection(out string Error)
        {
            Error = string.Empty;
            VerticaConnection verticaConnection = null;

            try
            {
                if (Connection.ConnectionType == ConnectionType.MSSQLSERVER)
                {
                    using (SqlConnection connection = new SqlConnection(string.Format("server={0};initial catalog=master;uid={1};password={2};", this.txtServer.Text, this.txtUsername.Text, this.txtPassword.Text)))
                    {
                        connection.Open();
                        connection.Close();
                    }
                }
                else
                {
                    VerticaConnectionStringBuilder builder = new VerticaConnectionStringBuilder();
                    builder.Host = this.txtServer.Text;
                    builder.Database = this.txtDatabase.Text;
                    builder.User = this.txtUsername.Text;
                    builder.Password = this.txtPassword.Text;
                    verticaConnection = new VerticaConnection(builder.ToString());

                    verticaConnection.Open();

                    verticaConnection.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                Error = ex.Message;

                if (verticaConnection != null) verticaConnection.Close();
                return false;
            }
        }
    }
}
