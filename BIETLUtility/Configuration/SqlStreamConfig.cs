using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIETLUtility.Configuration
{
    public class SqlStreamConfig
    {
        public Connection OriginServer { get; set; }
        public Connection DesitinationServer { get; set; }
        public string WorkingFolder { get; set; }
        public int MaxRecordsPerFile { get; set; }
        public bool IsTotalSetted { get; set; }
        public int TotalCount { get; set; }
    }
}
