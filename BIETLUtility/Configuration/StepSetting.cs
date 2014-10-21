using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIETLUtility.Configuration
{
     public class StepSetting
    {
        public string Name { get; set; }
        public Connection Connection { get; set; }
        public string SQLStatement { get; set; }
        public bool EnableSQL { get; set; }
        public string TargetTable { get; set; }
    }
}
