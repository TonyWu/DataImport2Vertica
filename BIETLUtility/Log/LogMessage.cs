using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIETLUtility.Log
{
    public enum LogType
    {
        Message,
        Error,
        Debug
    }

    public class LogMessage
    {
        public DateTime LogDataTime { get; set; }
        public LogType Type { get; set; }
        public string Message { get; set; }
        public object Value { get; set; }
    }

    public class LogMessageArg : EventArgs
    {
        public LogMessage LogMessage { get; set; }
    }
}
