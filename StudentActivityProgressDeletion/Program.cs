using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIETLUtility.StudentActivityProgressDelete;

namespace StudentActivityProgressDeletion
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                var delete = new StudentActivityDelete();

                delete.DoDeletion();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
        }
    }
}
