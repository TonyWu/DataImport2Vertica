using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIETLUtility.Configuration;
using BIETLUtility.Mail;
using log4net;
using Vertica.Data.VerticaClient;

namespace BIETLUtility.StudentActivityProgressDelete
{
    public class StudentActivityId
    {
        public List<long> IdList { get; set; }
        public DateTime MaxInsertDate { get; set; }
    }

    public class StudentActivityDelete
    {
        private string _sqlconnectionstring;
        private Connection _vertivaConnection;
        private ILog _logger;

        public StudentActivityDelete()
        {
            this._sqlconnectionstring = ConfigurationManager.AppSettings["ConnectionString"];

            this._vertivaConnection = new Connection()
            {
                ConnectionType = ConnectionType.Vertica,
                Host = ConfigurationManager.AppSettings["Host"],//"10.43.35.37",
                Database = ConfigurationManager.AppSettings["Database"],//"Reporting",
                Username = ConfigurationManager.AppSettings["Username"],//"dbadmin",
                Password = ConfigurationManager.AppSettings["Password"],//"xxxxx",
                TargetTable = ConfigurationManager.AppSettings["TargetTable"]//"StudentBehavior.StudentActivityProgress"
            };

            _logger = LogManager.GetLogger("StudentActivityDelete");
        }

        public void DoDeletion()
        {
            try
            {
                this._logger.Info("Begin to delete studentactivityprogress...");
                Console.WriteLine("Begin to delete studentactivityprogress...");
                DateTime startTime = DateTime.Parse(ConfigurationManager.AppSettings["StartTime"]);

                StudentActivityId ids = LoadDeletedStudentActivityIds(startTime);
                this._logger.Info("ids count :" + ids.IdList.Count);
                Console.WriteLine("ids count :" + ids.IdList.Count);
                this.ExecuteDeletionOnVertica(ids);

                this._logger.Info("Due StartTime: " + ids.MaxInsertDate.ToString("yyyy-MM-dd HH:mm:ss"));

                this.ChangeDueDate(ids.MaxInsertDate.AddDays(-1));
                Console.WriteLine("Complete delete student activity progress.");
                this._logger.Info("Complete delete student activity progress.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message + "\n" + ex.StackTrace);
                this.SendExeptionMail(ex);
            }
        }

        private void ChangeDueDate(DateTime datetime)
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["StartTime"].Value = datetime.ToString("yyyy-MM-dd HH:mm:ss");
            config.Save(ConfigurationSaveMode.Modified);

            ConfigurationManager.RefreshSection("appSettings");
        }

        private void SendExeptionMail(Exception ex)
        {
            SmtpServer SmtpServer = new SmtpServer();
            SmtpServer.Server = "ht.ef.com";
            SmtpServer.User = "English.Report@ef.com";
            SmtpServer.Password = "123";
            SmtpServer.PickupDirectoryLocation = @"\\10.43.42.153\Pickup";
            SmtpServer.UsePickupDirectoryLocation = false;
            SmtpServer.ExceptionFrom = "EtlException@ef.com";
            SmtpServer.ExceptionTo = "tony.wu@ef.com";

            string outstring;
            new SmtpClientAdaptor(SmtpServer).ExceptionNotify("Delete Student Activity Progress failed.", ex.Message + "\n" + ex.StackTrace, out outstring);
        }

        private void ExecuteDeletionOnVertica(StudentActivityId ids)
        {
            VerticaConnectionStringBuilder builder = new VerticaConnectionStringBuilder();
            builder.Host = this._vertivaConnection.Host;
            builder.Database = this._vertivaConnection.Database;
            builder.User = this._vertivaConnection.Username;
            builder.Password = this._vertivaConnection.Password;
            Console.WriteLine(builder.Password);
            VerticaConnection verticaConnection = new VerticaConnection(builder.ToString());
            string path = AppDomain.CurrentDomain.BaseDirectory + "log";
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
                    Console.WriteLine("execute delete on vertica..");
                    string commandText = string.Format("Delete from {0} Where StudentActivityProgress_id IN ({1});", this._vertivaConnection.TargetTable, string.Join(",", ids.IdList));
                    VerticaCommand command = new VerticaCommand(commandText, verticaConnection);
                    command.ExecuteNonQuery();
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

        private StudentActivityId LoadDeletedStudentActivityIds(DateTime startTime)
        {
            StudentActivityId ids = new StudentActivityId();
            ids.IdList = new List<long>();
            ids.MaxInsertDate = DateTime.MinValue;

            string commandText = @"SELECT InsertDate, StudentActivityProgress_id      
FROM SchoolAccount.dbo.StudentActivityProgress_Deleted WITH (NOLOCK)    
WHERE InsertDate >= @LastDeleteDate";

            using (SqlConnection connection = new SqlConnection(this._sqlconnectionstring))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.CommandTimeout = 120;
                    command.Parameters.Add(new SqlParameter()
                    {
                        ParameterName = "LastDeleteDate",
                        DbType = System.Data.DbType.DateTime,
                        Value = startTime
                    });
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime time = reader.GetDateTime(0);
                            ids.IdList.Add(reader.GetInt64(1));
                            if (time > ids.MaxInsertDate)
                                ids.MaxInsertDate = time;
                        }

                    }
                }
            }

            return ids;
        }

    }
}
