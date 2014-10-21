using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIETLUtility.Configuration;
using BIETLUtility.Log;
using Vertica.Data.VerticaClient;

namespace BIETLUtility
{
    public class SqlStreamClient
    {
        private string _delimiter = "|";
        private string _pipeAsciiCode = "&#124;";
        private string _slashAsciiCode = "&#92";
        private Transformation _transform;
        ILogEx _log;

        private string logFileName;
        private string fileName;
        private bool _truncatedDestinationFlag;
        private bool _deltaRecordsDeletedFlag;

        string minDeltaID = string.Empty;
        string maxDeltaID = string.Empty;

        const string sqlBetween = " %00 >= '%01'";
        const string sqlAnd = " AND ";
        const string sqlWhere = " WHERE ";

        private Delta.DeltaUtility _deltaUtility;

        public EventHandler<LogMessageArg> MessageLog;

        public SqlStreamClient(Transformation config, ILogEx log)
        {
            this._transform = config;
            this._truncatedDestinationFlag = this._transform.IsTruncateDestination;
            this._deltaRecordsDeletedFlag = this._transform.OriginDeltasChecked;
            this._log = log;

            logFileName = @"%05%00%01.log".Replace("%00", _transform.InputStep.TargetTable).Replace("%01", DateTime.Now.ToString("MMddyyyy_hh_mm_ss_fff_tt")).Replace("%05", _transform.WorkingFolder);
            fileName = @"%05%00_%01.txt".Replace("%00", _transform.InputStep.TargetTable).Replace("%01", (this._transform.MaxRecordCounter.ToString() + DateTime.Now.ToString("MMddyyyy_hh_mm_ss_fff_tt"))).Replace("%05", _transform.WorkingFolder.EndsWith("\\") ? _transform.WorkingFolder : _transform.WorkingFolder + "\\");
            
        }

        public async Task ExecuteTransformAsync()
        {
            if (this._transform.InputStep.Connection.ConnectionType == ConnectionType.MSSQLSERVER)
                await CopySQLStreamToDiskAsync();
            else
                await CopyVerticaToDiskAsync();
        }

        public void ExecuteTransform()
        {
            this.CopySQLStreamToDisk();
        }

        private void CopySQLStreamToDisk()
        {
            this.LogMessage(LogType.Message, string.Format("Begin to Tansformation {0}...", this._transform.Name));
            //const string sqlBetween = " with (nolock) WHERE %00 >= '%02'"; //" WHERE %00 < %01 OR %00 > %02";
            try
            {
                int recordBatchCounter = 0;
                int fileCounter = 0;
                int recordCounter = 0;
                int totalCounter = 0;
                int maxRecordCount = this._transform.MaxRecordCounter;

                if (this._transform.OriginDeltasChecked)
                {
                    this.LogMessage(LogType.Message, "Get Min Max From Vertica...");
                    GetMinMaxFromVertica(out minDeltaID, out maxDeltaID);
                    this.LogMessage(LogType.Message, string.Format("Min {0}:{1}, Max {0}: {2}", this._transform.DeltaId, minDeltaID, maxDeltaID));
                }

                this.LogMessage(LogType.Message, "Create working folder...");
                Directory.CreateDirectory(this._transform.WorkingFolder);

                this.LogMessage(LogType.Message, "Get total count...");
                totalCounter = 100000;// this.GetTotolCount();

                StreamWriter streamWriter = new StreamWriter(fileName);
                using (SqlConnection connection = new SqlConnection(this._transform.InputStep.Connection.ConnectionString))
                {
                    connection.Open();
                    //string SQLCommand = (this._transform.InputStep.EnableSQL && this._transform.InputStep.SQLStatement.Length > 0) ? this._transform.InputStep.SQLStatement : "SELECT * FROM dbo.%01%02".Replace("%01", this._transform.InputStep.TargetTable).Replace("%02", (this._transform.OriginDeltasChecked && minDeltaID.Length > 0 && maxDeltaID.Length > 0) ? sqlBetween.Replace("%00", this._transform.DeltaId).Replace("%01", minDeltaID).Replace("%02", maxDeltaID) : string.Empty);
                    string SQLCommand = this.GetSqlCommand();
                    
                    this.LogMessage(LogType.Debug, "CommandText:" + SQLCommand);
                    using (SqlCommand command = new SqlCommand(SQLCommand, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SequentialAccess))
                        {
                            if (reader.Read())
                            {
                                if (!(reader.IsDBNull(0)))
                                {
                                    do
                                    {
                                        if (recordBatchCounter < maxRecordCount)
                                        {
                                            if (recordBatchCounter.Equals(0) && (fileCounter > 0))
                                            {
                                                double percent = (double)recordCounter / totalCounter;
                                                this.LogMessage(LogType.Debug, "Copy SQL Stream to Disk...", percent);
                                                fileName = @"%05%00_%01.txt".Replace("%00", this._transform.InputStep.TargetTable).Replace("%01", (fileCounter.ToString() + DateTime.Now.ToString("MMddyyyy_hh_mm_ss_fff_tt"))).Replace("%05", _transform.WorkingFolder.EndsWith("\\") ? _transform.WorkingFolder : _transform.WorkingFolder + "\\");
                                                streamWriter = new StreamWriter(fileName);
                                            }
                                            this.WriteLine(reader, streamWriter);
                                            recordBatchCounter = recordBatchCounter + 1;
                                            //stream = new MemoryStream(byteArray);
                                            recordCounter++;
                                        }
                                        else if (recordBatchCounter == maxRecordCount)
                                        {
                                            this.WriteLine(reader, streamWriter);
                                            streamWriter.Flush();
                                            streamWriter.Close();
                                            streamWriter.Dispose();
                                            recordBatchCounter = 0;
                                            fileCounter = fileCounter + 1;
                                            recordCounter++;
                                            double percent = (double)recordCounter / totalCounter;
                                            this.LogMessage(LogType.Debug, "Copy Disk to Vertica...", percent);
                                            CopyDiskToVertica();
                                        }
                                    }
                                    while (reader.Read());

                                    streamWriter.Flush();
                                    streamWriter.Close();
                                    streamWriter.Dispose();
                                    CopyDiskToVertica();
                                }


                            }

                            this.LogMessage(LogType.Message, string.Format("Complete transform {0}, Total records {1}", this._transform.Name, recordCounter.ToString()), recordCounter / totalCounter);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this.LogMessage(LogType.Error, e.Message);
                throw;
            }
        }

        private void CopyDiskToVertica()
        {
            VerticaConnectionStringBuilder builder = new VerticaConnectionStringBuilder();
            builder.Host = this._transform.OutputStep.Connection.Host;
            builder.Database = this._transform.OutputStep.Connection.Database;
            builder.User = this._transform.OutputStep.Connection.Username;
            builder.Password = this._transform.OutputStep.Connection.Password;
            VerticaConnection verticaConnection = new VerticaConnection(builder.ToString());
            string path = this._transform.WorkingFolder;
            VerticaLogProperties.SetLogPath(path, false);
            VerticaLogLevel level = VerticaLogLevel.Trace;
            VerticaLogProperties.SetLogLevel(level, false);
            string lognamespace = "Vertica.Data.VerticaClient";
            VerticaLogProperties.SetLogNamespace(lognamespace, false);

            try
            {
                verticaConnection.Open();
                using (verticaConnection)
                {
                    verticaConnection.InfoMessage += new VerticaInfoMessageEventHandler(connection_InfoMessage);

                    if (this._truncatedDestinationFlag)
                    {
                        this.LogMessage(LogType.Message, string.Format("Truncate table {0} in vertica!", this._transform.OutputStep.TargetTable));
                        VerticaCommand command = new VerticaCommand("Truncate table " + this._transform.OutputStep.TargetTable + ";", verticaConnection);
                        //VerticaCommand command = new VerticaCommand("CREATE TABLE PageVisit(Id int NOT NULL,Session_id varchar(400) NULL,Member_id bigint NULL,EFGuid varchar(400) NULL,ReferrerUrl varchar(4000) NULL,RequestUrl varchar(4000) NULL,PartnerTrackName varchar(512) NULL,VisitDate varchar(100) NULL,EntryTag varchar(200) NULL,MarketCode varchar(100) NULL,ServerName varchar(100) NULL,InsertDate varchar(100) NULL,VisitId varchar(100) NULL,IPAddress varchar(80) NULL,RequestUrlPageId varchar(10) NULL);", verticaConnection);
                        command.ExecuteNonQuery();
                        this._truncatedDestinationFlag = false;
                    }

                    //if only has deltaid, do this delete action
                    if (this._deltaRecordsDeletedFlag && !this._transform.PrimaryKey.HasPrimaryKey)
                    {
                        VerticaCommand command = new VerticaCommand(string.Format("Delete from {0} Where {1} >= '{2}';", this._transform.OutputStep.TargetTable, this._transform.DeltaId, maxDeltaID), verticaConnection);
                        this.LogMessage(LogType.Debug, string.Format("Execute CommandText:{0}!", command.CommandText));
                        command.ExecuteNonQuery();

                        this._deltaRecordsDeletedFlag = false;
                    }

                    //if has primarykey, do this delete action
                    if (this._transform.OriginDeltasChecked && this._transform.PrimaryKey.HasPrimaryKey)
                    {
                        //ExecuteDeltaDeleteOnVertica(this._transform, verticaConnection);
                        new Delta.DeltaUtility(this._transform, verticaConnection).ExecuteDeltaDeleteOnVertica();
                    }

                    VerticaTransaction txn = verticaConnection.BeginTransaction();
                    FileStream inputfile = File.OpenRead(fileName);
                    string copy = "copy " + this._transform.OutputStep.TargetTable + " from stdin delimiter '" + this._delimiter + "' no commit";
                    VerticaCopyStream vcs = new VerticaCopyStream(verticaConnection, copy);
                    vcs.Start();
                    vcs.AddStream(inputfile, false);
                    vcs.Execute();
                    long rowsInserted = vcs.Finish();
                    Debug.WriteLine(fileName + " : " + rowsInserted);
                    IList<long> rowsRejected = vcs.Rejects; // does not work when rejected or exceptions defined
                    inputfile.Close();
                    this._log.LogRejected(rowsRejected, fileName, logFileName);
                    txn.Commit();
                    inputfile.Dispose();
                    File.Delete(fileName);
                }
                verticaConnection.Close();
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                verticaConnection.Close();
            }
        }

        private async Task CopySQLStreamToDiskAsync()
        {
            this.LogMessage(LogType.Message, string.Format("Begin to Tansformation {0}...",this._transform.Name));
            //const string sqlBetween = " with (nolock) WHERE %00 >= '%02'"; //" WHERE %00 < %01 OR %00 > %02";
            try
            {
                int recordBatchCounter = 0;
                int fileCounter = 0;
                int recordCounter = 0;
                int totalCounter = 0;
                int maxRecordCount = this._transform.MaxRecordCounter;


                if (this._transform.OriginDeltasChecked)
                {
                    this.LogMessage(LogType.Message, "Get Min Max From Vertica...");
                    GetMinMaxFromVertica(out minDeltaID, out maxDeltaID);
                    this.LogMessage(LogType.Debug, string.Format("Min {0}:{1}, Max {0}: {2}", this._transform.DeltaId, minDeltaID, maxDeltaID));
                }

                this.LogMessage(LogType.Message, "Create working folder...");
                Directory.CreateDirectory(this._transform.WorkingFolder);

                this.LogMessage(LogType.Message, "Get total count...");
                totalCounter = this.GetTotolCount();

                StreamWriter streamWriter = new StreamWriter(fileName);
                using (SqlConnection connection = new SqlConnection(this._transform.InputStep.Connection.ConnectionString))
                {
                    await connection.OpenAsync();
                    //string SQLCommand = (this._transform.InputStep.EnableSQL && this._transform.InputStep.SQLStatement.Length > 0) ? this._transform.InputStep.SQLStatement : "SELECT * FROM dbo.%01%02".Replace("%01", this._transform.InputStep.TargetTable).Replace("%02", (this._transform.OriginDeltasChecked && minDeltaID.Length > 0 && maxDeltaID.Length > 0) ? sqlBetween.Replace("%00", this._transform.DeltaId).Replace("%01", minDeltaID).Replace("%02", maxDeltaID) : string.Empty);
                   
                    string SQLCommand = this.GetSqlCommand();
                    
                    this.LogMessage(LogType.Debug, "CommandText:" + SQLCommand);
                    using (SqlCommand command = new SqlCommand(SQLCommand, connection))
                    {
                        command.CommandTimeout = 60;
                        using (SqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess))
                        {
                            if (await reader.ReadAsync())
                            {
                                if (!(await reader.IsDBNullAsync(0)))
                                {
                                    do
                                    {
                                        if (recordBatchCounter < maxRecordCount)
                                        {
                                            if (recordBatchCounter.Equals(0) && (fileCounter > 0))
                                            {
                                                double percent = (double)recordCounter/totalCounter;
                                                this.LogMessage(LogType.Debug, "Copy SQL Stream to Disk...", percent);
                                                fileName = @"%05%00_%01.txt".Replace("%00", this._transform.InputStep.TargetTable).Replace("%01", (fileCounter.ToString() + DateTime.Now.ToString("MMddyyyy_hh_mm_ss_fff_tt"))).Replace("%05", _transform.WorkingFolder.EndsWith("\\") ? _transform.WorkingFolder : _transform.WorkingFolder + "\\");
                                                streamWriter = new StreamWriter(fileName);
                                            }
                                            this.WriteLine(reader, streamWriter);
                                            recordBatchCounter = recordBatchCounter + 1;
                                            //stream = new MemoryStream(byteArray);
                                            recordCounter++;
                                        }
                                        else if (recordBatchCounter == maxRecordCount)
                                        {
                                            this.WriteLine(reader, streamWriter);
                                            streamWriter.Flush();
                                            streamWriter.Close();
                                            streamWriter.Dispose();
                                            recordBatchCounter = 0;
                                            fileCounter = fileCounter + 1;
                                            recordCounter++;
                                            
                                            await CopyDiskToVerticaAsync();
                                            double percent = (double)recordCounter / totalCounter;
                                            this.LogMessage(LogType.Debug, "Copy Disk to Vertica...", percent);
                                        }
                                    }
                                    while (reader.Read());

                                    streamWriter.Flush();
                                    streamWriter.Close();
                                    streamWriter.Dispose();
                                    await CopyDiskToVerticaAsync();
                                }

                                
                            }

                            this.LogMessage(LogType.Message, string.Format("Complete transform {0}, Total records {1}", this._transform.Name, recordCounter.ToString()), recordCounter / totalCounter);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this.LogMessage(LogType.Error, e.Message);
                throw;
            }
        }

        private async Task CopyDiskToVerticaAsync()
        {
            VerticaConnectionStringBuilder builder = new VerticaConnectionStringBuilder();
            builder.Host = this._transform.OutputStep.Connection.Host;
            builder.Database = this._transform.OutputStep.Connection.Database;
            builder.User = this._transform.OutputStep.Connection.Username;
            builder.Password = this._transform.OutputStep.Connection.Password;
            VerticaConnection verticaConnection = new VerticaConnection(builder.ToString());
            string path = this._transform.WorkingFolder;
            VerticaLogProperties.SetLogPath(path, false);
            VerticaLogLevel level = VerticaLogLevel.Trace;
            VerticaLogProperties.SetLogLevel(level, false);
            string lognamespace = "Vertica.Data.VerticaClient";
            VerticaLogProperties.SetLogNamespace(lognamespace, false);

            try
            {
                this.LogMessage(LogType.Debug, string.Format("verticaConnection.OpenAsync(), host:{0}, Database:{1}, table:{2}", this._transform.OutputStep.Connection.Host, this._transform.OutputStep.Connection.Database, this._transform.OutputStep.TargetTable));
                await verticaConnection.OpenAsync();
                using (verticaConnection)
                {
                    verticaConnection.InfoMessage += new VerticaInfoMessageEventHandler(connection_InfoMessage);

                    if (this._truncatedDestinationFlag)
                    {
                        this.LogMessage(LogType.Message, string.Format("Truncate table {0} in vertica!", this._transform.OutputStep.TargetTable));
                        VerticaCommand command = new VerticaCommand("Truncate table " + this._transform.OutputStep.TargetTable + ";", verticaConnection);
                        //VerticaCommand command = new VerticaCommand("CREATE TABLE PageVisit(Id int NOT NULL,Session_id varchar(400) NULL,Member_id bigint NULL,EFGuid varchar(400) NULL,ReferrerUrl varchar(4000) NULL,RequestUrl varchar(4000) NULL,PartnerTrackName varchar(512) NULL,VisitDate varchar(100) NULL,EntryTag varchar(200) NULL,MarketCode varchar(100) NULL,ServerName varchar(100) NULL,InsertDate varchar(100) NULL,VisitId varchar(100) NULL,IPAddress varchar(80) NULL,RequestUrlPageId varchar(10) NULL);", verticaConnection);
                        command.ExecuteNonQuery();
                        this._truncatedDestinationFlag = false;
                    }

                    //if only has deltaid, do this delete action
                    if (this._deltaRecordsDeletedFlag && !this._transform.PrimaryKey.HasPrimaryKey)
                    {
                        string cmdText = string.Format("Delete from {0} Where {1} >= '{2}';", this._transform.OutputStep.TargetTable, this._transform.DeltaId, maxDeltaID);
                        this.LogMessage(LogType.Debug, string.Format("Execute CommandText:{0}!", cmdText));
                        VerticaCommand command = new VerticaCommand(cmdText, verticaConnection);
                        command.ExecuteNonQuery();

                        this._deltaRecordsDeletedFlag = false;
                    }

                    //if has primarykey, do this delete action
                    if (this._transform.OriginDeltasChecked && this._transform.PrimaryKey.HasPrimaryKey)
                    {
                        this.LogMessage(LogType.Debug, "DeleteStatement:" + this._transform.PrimaryKey.DeleteStatement);
                        new Delta.DeltaUtility(this._transform, verticaConnection).ExecuteDeltaDeleteOnVertica();
                    }

                    VerticaTransaction txn = verticaConnection.BeginTransaction();
                    FileStream inputfile = File.OpenRead(fileName);
                    string copy = "copy " + this._transform.OutputStep.TargetTable + " from stdin delimiter '" + this._delimiter + "' no commit";
                    VerticaCopyStream vcs = new VerticaCopyStream(verticaConnection, copy);
                    vcs.Start();
                    vcs.AddStream(inputfile, false);
                    vcs.Execute();
                    long rowsInserted = vcs.Finish();
                    Debug.WriteLine(fileName + " : " + rowsInserted);
                    IList<long> rowsRejected = vcs.Rejects; // does not work when rejected or exceptions defined
                    inputfile.Close();
                    this._log.LogRejected(rowsRejected, fileName, logFileName);
                    txn.Commit();
                    inputfile.Dispose();
                    File.Delete(fileName);
                }
                verticaConnection.Close();
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                verticaConnection.Close();
            }
        }

        private async Task CopyVerticaToDiskAsync()
        {
            VerticaConnectionStringBuilder builder = new VerticaConnectionStringBuilder();
            builder.Host = this._transform.InputStep.Connection.Host;
            builder.Database = this._transform.InputStep.Connection.Database;
            builder.User = this._transform.InputStep.Connection.Username;
            builder.Password = this._transform.InputStep.Connection.Password;
            VerticaConnection verticaConnection = new VerticaConnection(builder.ToString());
            string path = this._transform.WorkingFolder;
            VerticaLogProperties.SetLogPath(path, false);
            VerticaLogLevel level = VerticaLogLevel.Trace;
            VerticaLogProperties.SetLogLevel(level, false);
            string lognamespace = "Vertica.Data.VerticaClient";
            VerticaLogProperties.SetLogNamespace(lognamespace, false);


            this.LogMessage(LogType.Message, string.Format("Begin to Tansformation {0}...", this._transform.Name));
            try
            {
                verticaConnection.InfoMessage += new VerticaInfoMessageEventHandler(connection_InfoMessage);

                int recordBatchCounter = 0;
                int fileCounter = 0;
                int recordCounter = 0;
                int totalCounter = 0;
                int maxRecordCount = this._transform.MaxRecordCounter;

                this.LogMessage(LogType.Message, "Create working folder...");
                Directory.CreateDirectory(this._transform.WorkingFolder);

                this.LogMessage(LogType.Message, "Get total count...");
                totalCounter = this.GetTotolCount();

                StreamWriter streamWriter = new StreamWriter(fileName);

                await verticaConnection.OpenAsync();
                string SQLCommand = "SELECT * FROM %01".Replace("%01", this._transform.InputStep.TargetTable);
                this.LogMessage(LogType.Debug, "CommandText:" + SQLCommand);

                using (VerticaCommand command = new VerticaCommand(SQLCommand + ";", verticaConnection))
                {
                    using (VerticaDataReader reader = command.ExecuteReader(CommandBehavior.SequentialAccess))
                    {
                        if (reader.Read())
                        {
                            if (!(reader.IsDBNull(0)))
                            {
                                do
                                {
                                    if (recordBatchCounter < maxRecordCount)
                                    {
                                        if (recordBatchCounter.Equals(0) && (fileCounter > 0))
                                        {
                                            double percent = (double)recordCounter / totalCounter;
                                            this.LogMessage(LogType.Debug, "Copy SQL Stream to Disk...", percent);
                                            fileName = @"%05%00_%01.txt".Replace("%00", this._transform.InputStep.TargetTable).Replace("%01", (fileCounter.ToString() + DateTime.Now.ToString("MMddyyyy_hh_mm_ss_fff_tt"))).Replace("%05", _transform.WorkingFolder.EndsWith("\\") ? _transform.WorkingFolder : _transform.WorkingFolder + "\\");
                                            streamWriter = new StreamWriter(fileName);
                                        }
                                        this.WriteLine(reader, streamWriter);
                                        recordBatchCounter = recordBatchCounter + 1;
                                        recordCounter++;
                                    }
                                    else if (recordBatchCounter == maxRecordCount)
                                    {
                                        this.WriteLine(reader, streamWriter);
                                        streamWriter.Flush();
                                        streamWriter.Close();
                                        streamWriter.Dispose();
                                        recordBatchCounter = 0;
                                        fileCounter = fileCounter + 1;
                                        recordCounter++;
                                        double percent = (double)recordCounter / totalCounter;
                                        this.LogMessage(LogType.Debug, "Copy Disk to Vertica...", percent);
                                        CopyDiskToVertica();
                                    }
                                }
                                while (reader.Read());

                                streamWriter.Flush();
                                streamWriter.Close();
                                streamWriter.Dispose();
                                CopyDiskToVertica();
                            }
                        }

                        this.LogMessage(LogType.Message, string.Format("Complete transform {0}, Total records {1}", this._transform.Name, recordCounter.ToString()), recordCounter / totalCounter);
                    }
                }

                verticaConnection.Close();

            }
            catch (Exception e)
            {
                this.LogMessage(LogType.Error, e.Message);
                throw;
            }
            finally
            {
                verticaConnection.Close();
            }
        }

        private void WriteLine(SqlDataReader reader, StreamWriter streamWriter)
        {
            try
            {
                string value = string.Empty;
                Dictionary<string, string> keyValue = new Dictionary<string, string>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string originStr = string.Empty;
                    if (!reader.IsDBNull(i))
                        originStr = reader[i].ToString();
                    string str = originStr.Replace(this._delimiter, this._pipeAsciiCode).Replace("\\", this._slashAsciiCode).Replace("\n", string.Empty).Replace("\r", string.Empty);

                    value = value + str + this._delimiter;

                    if (this._transform.OriginDeltasChecked && this._transform.PrimaryKey.HasPrimaryKey)
                    {
                        if (this._transform.PrimaryKey.IsMultiplePrimaryKey)
                        {
                            if (this._transform.PrimaryKey.NameList.Contains(reader.GetName(i)))
                            {
                                keyValue.Add(reader.GetName(i), originStr);
                            }
                        }
                        else
                        {
                            if (reader.GetName(i).ToLower() == this._transform.PrimaryKey.NameList[0].ToLower())
                            {
                                this._transform.PrimaryKey.ValueList.Add(originStr);
                            }
                        }
                    }
                }

                if (keyValue.Count > 0)
                    this._transform.PrimaryKey.Collection.Add(keyValue);

                streamWriter.Write(value);
                streamWriter.WriteLine();
            }
            catch (Exception ex)
            {
                this.LogMessage(LogType.Error, ex.Message);
                throw;
            }
        }

        private void WriteLine(VerticaDataReader reader, StreamWriter streamWriter)
        {
            string value = string.Empty;
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string originStr = reader[i].ToString();
                value = value + originStr + this._delimiter;

                if (this._transform.OriginDeltasChecked && this._transform.PrimaryKey.HasPrimaryKey)
                {
                    
                    //if (reader.GetName(i).ToLower() == this._transform.PrimaryKey.Name.ToLower())
                    //{
                    //    this._transform.PrimaryKey.ValueList.Add(originStr);
                    //}
                }
            }
            streamWriter.Write(value);
            streamWriter.WriteLine();
        }

        private void GetMinMaxFromVertica(out string minDeltaID, out string maxDeltaID)
        {
            const string deltaSQL = "select min(%00), max(%01) from %02;";
            VerticaConnectionStringBuilder builder = new VerticaConnectionStringBuilder();
            builder.Host = this._transform.OutputStep.Connection.Host;
            builder.Database = this._transform.OutputStep.Connection.Database;
            builder.User = this._transform.OutputStep.Connection.Username;
            builder.Password = this._transform.OutputStep.Connection.Password;
            VerticaConnection verticaConnection = new VerticaConnection(builder.ToString());
            minDeltaID = string.Empty;
            maxDeltaID = string.Empty;

            try
            {
                verticaConnection.Open();
                using (verticaConnection)
                {
                    VerticaCommand command = new VerticaCommand(deltaSQL.Replace("%00", this._transform.DeltaId).Replace("%01", this._transform.DeltaId).Replace("%02", this._transform.OutputStep.TargetTable), verticaConnection);
                    this.LogMessage(LogType.Debug, command.CommandText);
                    VerticaDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        minDeltaID = reader[0].ToString();
                        maxDeltaID = reader[1].ToString();
                        this.LogMessage(LogType.Debug, "MaxDeltaId:" + maxDeltaID);
                        DateTime datetime;
                        if (DateTime.TryParse(maxDeltaID, out datetime))
                        {
                            maxDeltaID = datetime.ToString("yyyy-MM-dd HH:mm:ss");
                            minDeltaID = DateTime.Parse(minDeltaID).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                }
                verticaConnection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                verticaConnection.Close();
            }
        }

        void connection_InfoMessage(object sender, VerticaInfoMessageEventArgs e)
        {
            this.LogMessage(LogType.Message, e.SqlState + ": " + e.Message);
        }

        void LogMessage(LogType type, string message, double percent=0)
        {
            this._log.Log(message, type);

            if (this.MessageLog != null)
                this.MessageLog(this, new LogMessageArg()
                {
                    LogMessage = new LogMessage()
                    {
                        LogDataTime = DateTime.Now,
                        Type = type,
                        Message = message,
                        Value = percent
                    }
                });
        }

        private int GetTotolCount()
        {
            if (this._transform.IsTotalSetted)
                return this._transform.TotalCount;

            int total = 0;
            using (SqlConnection connection = new SqlConnection(this._transform.InputStep.Connection.ConnectionString))
            {
                connection.Open();
                string commandText = string.Empty;

                if (this._transform.InputStep.EnableSQL)
                {
                    //int index = this._transform.InputStep.SQLStatement.ToLower().IndexOf(" from ");
                    //commandText = "Select count(*) " + this._transform.InputStep.SQLStatement.Remove(0, index);
                    return 100000;
                }
                else if (this._transform.OriginDeltasChecked)
                {
                    commandText = string.Format("Select count(*) from {0} with (nolock) where {1} >= '{2}' ", this._transform.InputStep.TargetTable, this._transform.DeltaId, maxDeltaID);
                    this.LogMessage(LogType.Debug, "CommandText:" + commandText);
                }
                else
                {
                    commandText = string.Format("Select count(*) from {0} with (nolock)", this._transform.InputStep.TargetTable);
                    this.LogMessage(LogType.Debug, "CommandText:" + commandText);
                }
                SqlCommand command = new SqlCommand(commandText, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    total = reader.GetInt32(0);
                }
                connection.Close();
            }

            return total;
        }

        private string GetSqlCommand()
        {
            string SQLCommand = string.Empty;
            if (this._transform.InputStep.EnableSQL && this._transform.InputStep.SQLStatement.Length > 0)
            {
                if (this._transform.OriginDeltasChecked && maxDeltaID.Length > 0)
                {
                    SQLCommand = this._transform.InputStep.SQLStatement.Replace("@MaxDeltaId", maxDeltaID);
                }
                else
                {
                    SQLCommand = this._transform.InputStep.SQLStatement;
                }
            }
            else
            {
                SQLCommand = string.Format("SELECT * FROM dbo.{0} with (nolock)", this._transform.InputStep.TargetTable);
                if (this._transform.OriginDeltasChecked && minDeltaID.Length > 0 && maxDeltaID.Length > 0)
                {
                    SQLCommand = SQLCommand + sqlWhere + sqlBetween.Replace("%00", this._transform.DeltaId).Replace("%01", maxDeltaID);
                }
            }

            return SQLCommand;
        }

    }
}
