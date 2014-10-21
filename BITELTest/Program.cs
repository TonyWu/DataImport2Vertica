using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BIETLUtility.Configuration;

namespace BITELTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string workingFolder = @"E:\temp\Transforms";

            string[] files = Directory.GetFiles(workingFolder);

            foreach (var item in files)
            {
                Trans tran = new Trans(item);
                tran.Save();
            }

            XDocument xmlDoc = XDocument.Parse("<Connections></Connections>");

            foreach (var item in Trans.ConnectionCollection)
            {

                XElement element = new XElement("Connection");
                element.SetAttributeValue("Name", item.Value.Name);
                element.SetElementValue("ConnectionType", item.Value.ConnectionType);
                element.SetElementValue("Server", item.Value.Host);
                element.SetElementValue("Username", item.Value.Username);
                element.SetElementValue("Password", item.Value.Password);
                element.SetElementValue("Database", item.Value.Database);
                xmlDoc.Root.Add(element);
            //}
            }

            xmlDoc.Save(@"E:\temp\Transforms\output\connections.xml");
        }
    }
}
