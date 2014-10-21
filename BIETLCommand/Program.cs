using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIETLUtility;
using BIETLUtility.Configuration;
using BIETLUtility.Log;
using BIETLUtility.Mail;

namespace BIETLCommand
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Transformation parameters {0}.", string.Join(",", args));

            ILogEx logger = EtlLog.GetLogger();
            logger.Log(string.Format("Transformation name {0}.", string.Join(",", args)), LogType.Message);
            Transformation transform = null;

            try
            {
                if (args.Count() == 1)
                {
                    string transformName = args[0];

                    ILogEx log = EtlLog.NewLogger(transformName);

                    transform = Transformations.Instance().GetTransformation(transformName);

                    if (transform != null)
                    {
                        SqlStreamClient client = new SqlStreamClient(transform, log);
                        client.ExecuteTransform();
                    }
                    else
                    {
                        log.Log("The transformation is not configed.", LogType.Error);
                    }
                }
                else
                {
                    Console.WriteLine("Transformation input error.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

                var smtp = new SmtpClientAdaptor(Transformations.Instance().SmtpServer);
                string error;
                smtp.ExceptionNotify(string.Format("Execute transformation {0} failed", transform.Name), ex.Message + "\n" + ex.StackTrace, out error);

                logger.Log(ex.StackTrace, LogType.Error);
            }
        }
    }
}
