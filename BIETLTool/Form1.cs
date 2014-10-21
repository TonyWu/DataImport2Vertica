using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vertica.Data.VerticaClient;

namespace BIETLTool
{
    public partial class Form1 : Form
    {
        public const string connectionString = @"server=%01;initial catalog=%02;uid=%03;password=%04;";

        public Form1()
        {
            InitializeComponent();
            folderName.Text = ConfigurationManager.AppSettings["folder"].ToString();
            maxRecordCounter.Text = ConfigurationManager.AppSettings["maxRecords"].ToString();
#if DEBUG
            originUsername.Text = "sa";
            originPassword.Text = "password";
            destinationUsername.Text = "dbadmin";
            destinationPassword.Text = "birocks992";
            destinationDatabase.Text = "Reporting";
            enableSQL.Checked = true;
            originSQL.Text = "Select * from dbo.PageVisit";

#endif
        }

        private async void startButton_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> collection =  new Dictionary<string, string>(); 
            collection.Add("originServer", originServer.Text);
            collection.Add("originDatabase", originDatabase.Text);
            collection.Add("originTable", originTable.Text);
            collection.Add("originDeltaID", originDeltaID.Text);
            collection.Add("originSQL", originSQL.Text);
            collection.Add("originUsername", originUsername.Text);
            collection.Add("originPassword", originPassword.Text);
            collection.Add("destinationServer", destinationServer.Text);
            collection.Add("destinationDatabase", destinationDatabase.Text);
            collection.Add("destinationTable", destinationTable.Text);
            collection.Add("destinationUsername", destinationUsername.Text);
            collection.Add("destinationPassword", destinationPassword.Text);
            collection.Add("folderName", folderName.Text);
            collection.Add("maxRecordCounter", maxRecordCounter.Text);
            collection.Add("truncateDestination", truncateDestination.Checked.ToString());
            collection.Add("enableSQL", enableSQL.Checked.ToString());
            collection.Add("originDeltas", originDeltas.Checked.ToString());
            string minDeltaID = string.Empty;
            string maxDeltaID = string.Empty;
            if(originDeltas.Checked)
            {
                GetMinMaxFromVertica(collection, out minDeltaID, out maxDeltaID);
            }
            collection.Add("minDeltaID", minDeltaID);
            collection.Add("maxDeltaID", maxDeltaID);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            await Task.Run(() => CopySQLStreamToDisk(collection));
            stopwatch.Stop();
            loadedCount.Text = string.Format("Hours :{0}\nMinutes :{1}\nSeconds :{2}\n Mili seconds :{3}", stopwatch.Elapsed.Hours, stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds, stopwatch.Elapsed.TotalMilliseconds);
        }

        async Task CopySQLStreamToDisk(Dictionary<string, string> collection)
        {
            const string sqlBetween = " WHERE %00 NOT BETWEEN %01 AND %02";
            try
            {
                int recordCounter = 0;
                int fileCounter = 0;
                int maxRecordCount = Int32.Parse(collection["maxRecordCounter"]);
                string logFileName = @"%05%00%01.log".Replace("%00", collection["originTable"]).Replace("%01", DateTime.Now.ToString("MMddyyyy_hh_mm_ss_fff_tt")).Replace("%05", collection["folderName"]);
                string fileName = @"%05%00_%01.txt".Replace("%00", collection["originTable"]).Replace("%01", (fileCounter.ToString() + DateTime.Now.ToString("MMddyyyy_hh_mm_ss_fff_tt"))).Replace("%05", collection["folderName"]);
                Directory.CreateDirectory(collection["folderName"]);
                StreamWriter streamWriter = new StreamWriter(fileName);
                using (SqlConnection connection = new SqlConnection(connectionString.Replace("%01", collection["originServer"]).Replace("%02", collection["originDatabase"]).Replace("%03", collection["originUsername"]).Replace("%04", collection["originPassword"])))
                {
                    await connection.OpenAsync();
                    string SQLCommand = (collection["enableSQL"].Equals("True") && !collection["originSQL"].Length.Equals(0)) ? collection["originSQL"] : "SELECT * FROM dbo.%01%02".Replace("%01", collection["originTable"]).Replace("%02", (collection["originDeltas"].Equals("True") && collection["minDeltaID"].Length > 0 && collection["minDeltaID"].Length > 0) ? sqlBetween.Replace("%00", collection["originDeltaID"]).Replace("%01", collection["minDeltaID"]).Replace("%02", collection["maxDeltaID"]) : string.Empty);
                    
                    using (SqlCommand command = new SqlCommand(SQLCommand, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess))
                        {
                            if (await reader.ReadAsync())
                            {
                                if (!(await reader.IsDBNullAsync(0)))
                                {
                                    while (reader.Read())
                                    {
                                        if (recordCounter < maxRecordCount)
                                        {
                                            if (recordCounter.Equals(0) && (fileCounter > 0))
                                            {
                                                fileName = @"%05%00_%01.txt".Replace("%00", collection["originTable"]).Replace("%01", (fileCounter.ToString() + DateTime.Now.ToString("MMddyyyy_hh_mm_ss_fff_tt"))).Replace("%05", collection["folderName"]);
                                                streamWriter = new StreamWriter(fileName);
                                            }
                                            string value = string.Empty;
                                            //byte[] byteArray = null;
                                            for (int i = 0; i < reader.FieldCount; i++)
                                            {
                                                value = value + reader[i].ToString() + "|";
                                                //byteArray[i] = (byte) reader[i];
                                            }
                                            streamWriter.Write(value);
                                            streamWriter.WriteLine();
                                            recordCounter = recordCounter + 1;
                                            //stream = new MemoryStream(byteArray);
                                        }
                                        else if (recordCounter.Equals(maxRecordCount))
                                        {
                                            string value = string.Empty;
                                            //byte[] byteArray = null;
                                            for (int i = 0; i < reader.FieldCount; i++)
                                            {
                                                value = value + reader[i].ToString() + "|";
                                                //byteArray[i] = (byte) reader[i];
                                            }
                                            streamWriter.Write(value);
                                            streamWriter.WriteLine();
                                            streamWriter.Flush();
                                            streamWriter.Close();
                                            streamWriter.Dispose();
                                            recordCounter = 0;
                                            fileCounter = fileCounter + 1;
                                            await CopyDiskToVertica(fileName, logFileName, collection);
                                        }
                                    }
                                    streamWriter.Flush();
                                    streamWriter.Close();
                                    streamWriter.Dispose();
                                    await CopyDiskToVertica(fileName, logFileName, collection);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        async Task CopyDiskToVertica(string fileName, string logFileName, Dictionary<string, string> collection)
        {
            VerticaConnectionStringBuilder builder = new VerticaConnectionStringBuilder();
            builder.Host = collection["destinationServer"];
            builder.Database = collection["destinationDatabase"];
            builder.User = collection["destinationUsername"];
            builder.Password = collection["destinationPassword"];
            VerticaConnection verticaConnection = new VerticaConnection(builder.ToString());
            string path = "C:\\Working\\Data\\";
            VerticaLogProperties.SetLogPath(path, false);
            VerticaLogLevel level = VerticaLogLevel.Trace;
            VerticaLogProperties.SetLogLevel(level, false);
            string lognamespace = "Vertica.Data.VerticaClient";
            VerticaLogProperties.SetLogNamespace(lognamespace, false);

            try
            {
                await verticaConnection.OpenAsync();
                using (verticaConnection)
                {
                    verticaConnection.InfoMessage += new VerticaInfoMessageEventHandler(connection_InfoMessage); 

                    if (collection["truncateDestination"].Equals("True"))
                    {
                        VerticaCommand command = new VerticaCommand("Truncate table " + collection["destinationTable"] + ";", verticaConnection);
                        //VerticaCommand command = new VerticaCommand("CREATE TABLE PageVisit(Id int NOT NULL,Session_id varchar(400) NULL,Member_id bigint NULL,EFGuid varchar(400) NULL,ReferrerUrl varchar(4000) NULL,RequestUrl varchar(4000) NULL,PartnerTrackName varchar(512) NULL,VisitDate varchar(100) NULL,EntryTag varchar(200) NULL,MarketCode varchar(100) NULL,ServerName varchar(100) NULL,InsertDate varchar(100) NULL,VisitId varchar(100) NULL,IPAddress varchar(80) NULL,RequestUrlPageId varchar(10) NULL);", verticaConnection);
                        command.ExecuteNonQuery();
                        collection["truncateDestination"] = "False";
                    }
                    VerticaTransaction txn = verticaConnection.BeginTransaction();
                    FileStream inputfile = File.OpenRead(fileName);
                    string copy = "copy " + collection["destinationTable"] + " from stdin delimiter '|' no commit";
                    VerticaCopyStream vcs = new VerticaCopyStream(verticaConnection, copy);
                    vcs.Start();
                    vcs.AddStream(inputfile, false);
                    vcs.Execute();
                    long rowsInserted = vcs.Finish();
                    Debug.WriteLine(fileName + " : " + rowsInserted);
                    IList<long> rowsRejected = vcs.Rejects; // does not work when rejected or exceptions defined
                    inputfile.Close();
                    LogRejected(rowsRejected, fileName, logFileName);
                    txn.Commit();
                    inputfile.Dispose();
                    File.Delete(fileName);
                }
                verticaConnection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                verticaConnection.Close();
            }
        }
        static void connection_InfoMessage(object sender, VerticaInfoMessageEventArgs e)
        {
            Console.WriteLine(e.SqlState + ": " + e.Message);
        }
        
        private void GetMinMaxFromVertica(Dictionary<string, string> collection, out string minDeltaID, out string maxDeltaID)
        {
            const string deltaSQL = "select min(%00), max(%01) from public.%02;";
            VerticaConnectionStringBuilder builder = new VerticaConnectionStringBuilder();
            builder.Host = collection["destinationServer"];
            builder.Database = collection["destinationDatabase"];
            builder.User = collection["destinationUsername"];
            builder.Password = collection["destinationPassword"];
            VerticaConnection verticaConnection = new VerticaConnection(builder.ToString());
            minDeltaID = string.Empty;
            maxDeltaID = string.Empty;

            try
            {
                verticaConnection.Open();
                using (verticaConnection)
                {
                    VerticaCommand command = new VerticaCommand(deltaSQL.Replace("%00", collection["originDeltaID"]).Replace("%01", collection["originDeltaID"]).Replace("%02", collection["destinationTable"]), verticaConnection);
                    VerticaDataReader reader = command.ExecuteReader();
                    while(reader.Read())
                    {
                        minDeltaID = reader[0].ToString();
                        maxDeltaID = reader[1].ToString();
                    }
                }
                verticaConnection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                verticaConnection.Close();
            }
        }

        private static void LogRejected(IList<long> rejectedRows, string fileName, string logFileName)
        {
            StreamReader inputFile = new StreamReader(fileName);
            StreamWriter outputFile = new StreamWriter(logFileName, true);
            string line = string.Empty;
            long lineNumber = 0;
            while ((line = inputFile.ReadLine()) != null)
            {
                if(!rejectedRows.IndexOf(lineNumber).Equals(-1))
                {
                    outputFile.WriteLine(line);
                }
                lineNumber++;
            }
            inputFile.Close();
            inputFile.Dispose();
            outputFile.Flush();
            outputFile.Close();
            outputFile.Dispose();
        }

        private void originServer_DropDown(object sender, EventArgs e)
        {
            try
            {
                originServer.Items.Clear();
                NameValueCollection appSettings = ConfigurationManager.AppSettings;
                for (int i = 0; i < appSettings.Count; i++)
                {
                    if (appSettings.GetKey(i).Contains("origin"))
                    {
                        originServer.Items.Add(appSettings[i]);
                    }
                }
            }
            catch(Exception ex)
            {
                errorProvider1.SetError(originServer, "Error occured, check your app.config for the correct settings. " + ex.InnerException.ToString());
            }
        }

        private void originDatabase_DropDown(object sender, EventArgs e)
        {
            try
            {
                if (!originServer.Text.Equals(string.Empty))
                {
                    originDatabase.Items.Clear();
                    SqlConnection connection = new SqlConnection(connectionString.Replace("%01", originServer.Text).Replace("%02", "master").Replace("%03", originUsername.Text).Replace("%04", originPassword.Text));
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_databases", connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        originDatabase.Items.Add(reader["DATABASE_NAME"]);
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                //errorProvider1.SetError(originDatabase, "Error occured, Does Origin Server have a value? Check username and password" + ex.InnerException.ToString());
            }
        }

        private void originTable_DropDown(object sender, EventArgs e)
        {
            if (!originServer.Text.Equals(string.Empty) && !originDatabase.Text.Equals(string.Empty))
            {
                originTable.Items.Clear();
                SqlConnection connection = new SqlConnection(connectionString.Replace("%01", originServer.Text).Replace("%02", originDatabase.Text).Replace("%03", originUsername.Text).Replace("%04", originPassword.Text));
                connection.Open();
                SqlCommand command = new SqlCommand("sp_tables '" + originDatabase.Text + "'", connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    originTable.Items.Add(reader["TABLE_NAME"]);
                }
                connection.Close();
            }
        }

        private void destinationServer_DropDown(object sender, EventArgs e)
        {
            try
            {
                destinationServer.Items.Clear();
                NameValueCollection appSettings = ConfigurationManager.AppSettings;
                for (int i = 0; i < appSettings.Count; i++)
                {
                    if (appSettings.GetKey(i).Contains("destination"))
                    {
                        destinationServer.Items.Add(appSettings[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(destinationServer, "Error occured, check your app.config for the correct settings. " + ex.InnerException.ToString());
            }
        }

        private void destinationTable_DropDown(object sender, EventArgs e)
        {
            try
            {
                if (!destinationServer.Text.Equals(string.Empty) && !destinationDatabase.Text.Equals(string.Empty))
                {
                    destinationTable.Items.Clear();
                    VerticaConnectionStringBuilder builder = new VerticaConnectionStringBuilder();
                    builder.Host = destinationServer.Text;
                    builder.Database = destinationDatabase.Text;
                    builder.User = destinationUsername.Text;
                    builder.Password = destinationPassword.Text;
                    VerticaConnection connection = new VerticaConnection(builder.ToString());
                    connection.Open();
                    VerticaCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT table_name FROM v_catalog.tables";
                    VerticaDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        destinationTable.Items.Add(reader["table_name"]);
                    }
                }
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(destinationUsername, "Error occured, Does Destination Server have a value? Check username and password" + ex.InnerException.ToString());
            }
        }

        private void originUpdate_CheckedChanged(object sender, EventArgs e)
        {
            originSQL.Enabled = enableSQL.Checked;
            originTable.Enabled = !enableSQL.Checked;
        }

        private void updateDate_Validating(object sender, CancelEventArgs e)
        {
            DateTime dateTime;
            CultureInfo cultureInfo = new CultureInfo("en-IE");
            if(originSQL.TextLength.Equals(0))
            {
                return;
            }
            if (!DateTime.TryParseExact(this.originSQL.Text, "MM/dd/yyyy", cultureInfo, DateTimeStyles.None, out dateTime))
            {
                errorProvider1.SetError(originSQL, "Date is not in the right format MM/DD/YYYY");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void originSQL_Enter(object sender, EventArgs e)
        {
            this.originSQL.Height = 400;
        }

        private void originSQL_Leave(object sender, EventArgs e)
        {
            this.originSQL.Height = 20;
        }

        private void originDeltas_CheckedChanged(object sender, EventArgs e)
        {
            originDeltaID.Enabled = originDeltas.Checked;
            originTable.Enabled = true;
        }
    }
}
