using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BIETLUtility.Delta;

namespace BIETLUtility.Configuration
{
    public class Transformation
    {
        public string Configfile { get; set; }

        public bool IsTotalSetted { get; set; }
        public int TotalCount { get; set; }

        public bool IsNew { get; set; }
        public string Name { get; set; }
        public StepSetting InputStep { get; set; }
        public StepSetting OutputStep { get; set; }
        public string WorkingFolder { get; set; }
        public int MaxRecordCounter { get; set; }
        public bool IsTruncateDestination { get; set; }
        public bool OriginDeltasChecked { get; set; }
        public string DeltaId { get; set; }
        public PrimaryKey PrimaryKey { get; set; }

        public Transformation(string name, string filename)
        {
            this.Name = name;
            this.Configfile = filename;
            this.PrimaryKey = new PrimaryKey();
            this.IsNew = true;
        }

        public Transformation(string filename)
        {
            this.Configfile = filename;
            this.PrimaryKey = new PrimaryKey();
            this.IsNew = false;
            this.Load();
        }

        private void Load()
        {
            XDocument xmlDoc = XDocument.Load(this.Configfile);
            this.Name = xmlDoc.Root.Attribute("Name").Value;
            this.WorkingFolder = xmlDoc.Root.Element("WorkingFolder").Value;
            this.MaxRecordCounter = int.Parse(xmlDoc.Root.Element("MaxRecordCounter").Value);
            this.IsTruncateDestination = bool.Parse(xmlDoc.Root.Element("IsTruncateDestination").Value);
            this.OriginDeltasChecked = bool.Parse(xmlDoc.Root.Element("OriginDeltasChecked").Value);
            this.DeltaId = xmlDoc.Root.Element("DeltaId").Value;

            if (xmlDoc.Root.Element("PrimaryKey") != null && !string.IsNullOrWhiteSpace(xmlDoc.Root.Element("PrimaryKey").Value))
            {
                this.PrimaryKey.NameList = xmlDoc.Root.Element("PrimaryKey").Value.Split(",".ToArray()).ToList();
            }

            //var conList = xmlDoc.Root.Element("Connections").Elements("Connection");

            //foreach (var item in conList)
            //{
            //    Connection con = new Connection();
            //    con.Name = item.Attribute("Name").Value;
            //    con.ConnectionType = item.Element("ConnectionType").Value == ConnectionType.MSSQLSERVER.ToString() ? ConnectionType.MSSQLSERVER : ConnectionType.Vertica;
            //    con.Host = item.Element("Server").Value;
            //    con.Username = item.Element("Username").Value;
            //    con.Password = item.Element("Password").Value;
            //    con.Database = item.Element("Database").Value;
            //    con.TargetTable = item.Element("Table").Value;

            //    this.ConnectionCollection.Add(con.Name, con);
            //}

            this.InputStep = new StepSetting();
            this.InputStep.Name = xmlDoc.Root.Element("InputSetting").Attribute("Name").Value;
            this.InputStep.EnableSQL = bool.Parse(xmlDoc.Root.Element("InputSetting").Element("EnableSQL").Value);
            this.InputStep.SQLStatement = xmlDoc.Root.Element("InputSetting").Element("SQLStatement").Value;
            this.InputStep.Connection = Transformations.Instance().GetConnection(xmlDoc.Root.Element("InputSetting").Element("Connection").Value);
            this.InputStep.TargetTable = xmlDoc.Root.Element("InputSetting").Element("TargetTable").Value;


            this.OutputStep = new StepSetting();
            this.OutputStep.Name = xmlDoc.Root.Element("OutputSetting").Attribute("Name").Value;
            this.OutputStep.EnableSQL = bool.Parse(xmlDoc.Root.Element("OutputSetting").Element("EnableSQL").Value);
            this.OutputStep.SQLStatement = xmlDoc.Root.Element("OutputSetting").Element("SQLStatement").Value;
            this.OutputStep.Connection = Transformations.Instance().GetConnection(xmlDoc.Root.Element("OutputSetting").Element("Connection").Value);
            this.OutputStep.TargetTable = xmlDoc.Root.Element("OutputSetting").Element("TargetTable").Value;
            this.PrimaryKey.Table = this.OutputStep.TargetTable;
        }

        public void AddConnection(Connection con, StepSetting stepSetting)
        {
            Transformations.Instance().NewConnection(con);
            stepSetting.Connection = con;
        }

        public void SetConnection(Connection con, StepSetting stepSetting)
        {
            stepSetting.Connection = con;
        }

        public void Save()
        {
            XDocument xmlDoc = XDocument.Parse("<Transformation></Transformation>");

            xmlDoc.Root.SetAttributeValue("Name", this.Name??string.Empty);
            xmlDoc.Root.SetElementValue("WorkingFolder", this.WorkingFolder??string.Empty);
            xmlDoc.Root.SetElementValue("MaxRecordCounter", this.MaxRecordCounter);
            xmlDoc.Root.SetElementValue("IsTruncateDestination", this.IsTruncateDestination);
            xmlDoc.Root.SetElementValue("OriginDeltasChecked", this.OriginDeltasChecked);
            xmlDoc.Root.SetElementValue("DeltaId", this.DeltaId??string.Empty);
            xmlDoc.Root.SetElementValue("PrimaryKey", this.PrimaryKey.NameString);

            xmlDoc.Root.SetElementValue("InputSetting", string.Empty);
            xmlDoc.Root.Element("InputSetting").SetAttributeValue("Name", this.InputStep.Name);
            xmlDoc.Root.Element("InputSetting").SetElementValue("Connection", this.InputStep.Connection.Name);
            xmlDoc.Root.Element("InputSetting").SetElementValue("EnableSQL", this.InputStep.EnableSQL);
            xmlDoc.Root.Element("InputSetting").Add(new XElement("SQLStatement", new XCData(this.InputStep.SQLStatement ?? string.Empty)));
            xmlDoc.Root.Element("InputSetting").Add(new XElement("TargetTable", this.InputStep.TargetTable ?? string.Empty));

            xmlDoc.Root.SetElementValue("OutputSetting", string.Empty);
            xmlDoc.Root.Element("OutputSetting").SetAttributeValue("Name", this.OutputStep.Name);
            xmlDoc.Root.Element("OutputSetting").SetElementValue("Connection", this.OutputStep.Connection.Name);
            xmlDoc.Root.Element("OutputSetting").SetElementValue("EnableSQL", this.OutputStep.EnableSQL);
            xmlDoc.Root.Element("OutputSetting").Add(new XElement("SQLStatement", new XCData(this.OutputStep.SQLStatement??string.Empty)));
            xmlDoc.Root.Element("OutputSetting").Add(new XElement("TargetTable", this.OutputStep.TargetTable ?? string.Empty));

            //xmlDoc.Root.SetElementValue("Connections", string.Empty);

            //foreach (var item in this.ConnectionCollection)
            //{
            //    XElement element = new XElement("Connection");
            //    element.SetAttributeValue("Name", item.Value.Name);
            //    element.SetElementValue("ConnectionType", item.Value.ConnectionType);
            //    element.SetElementValue("Server", item.Value.Host);
            //    element.SetElementValue("Username", item.Value.Username);
            //    element.SetElementValue("Password", item.Value.Password);
            //    element.SetElementValue("Database", item.Value.Database);
            //    element.SetElementValue("Table", item.Value.TargetTable);

            //    xmlDoc.Root.Element("Connections").Add(element);
            //}

            xmlDoc.Save(this.Configfile);

            this.IsNew = false;
        }

        public void SaveAs(string filename)
        {
            this.Configfile = filename;
            this.Save();
        }
    }
}
