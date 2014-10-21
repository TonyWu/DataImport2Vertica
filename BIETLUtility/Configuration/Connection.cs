using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIETLUtility.Configuration
{
    public enum ConnectionType
    {
        MSSQLSERVER,
        Vertica
    }

    public class Connection
    {
        public string Name { get; set; }
        public string Host { get; set; }
        public string Database { get; set; }
        public string TargetTable { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public ConnectionType ConnectionType { get; set; }

        public string ConnectionString
        {
            get
            {
                return string.Format("server={0};initial catalog={1};uid={2};password={3};", this.Host, Database, Username, Password);
            }
        }


    }
}
