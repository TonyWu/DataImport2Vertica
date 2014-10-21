using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BIETLUtility.Common;
using BIETLUtility.Mail;

namespace BIETLUtility.Configuration
{
    public class Transformations
    {
        private static Transformations instance;
        private static object locker = new object();
        private Dictionary<string, Transformation> _transforms;
        string configFile;

        private Dictionary<string, Connection> _connections;

        public SmtpServer SmtpServer
        {
            get;
            set;
        }

        public static Transformations Instance()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = new Transformations();
                        instance.PopulateTransforms();
                    }
                }
            }

            return instance;
        }

        private Transformations()
        {
            this._transforms = new Dictionary<string, Transformation>();
            this._connections = new Dictionary<string, Connection>();
            this.SmtpServer = new Mail.SmtpServer();

            string folder = System.Reflection.Assembly.GetExecutingAssembly().CodeBase.Remove(System.Reflection.Assembly.GetExecutingAssembly().CodeBase.LastIndexOf("/"));
            folder = folder.Replace("file:///", "");
            configFile = folder + "/" + System.Configuration.ConfigurationManager.AppSettings["ConfigFile"];

            XDocument xDoc = XDocument.Load(configFile);

            this.SmtpServer.Server = xDoc.Root.Element("SmtpServer").Element("Server").Value;
            this.SmtpServer.PickupDirectoryLocation = xDoc.Root.Element("SmtpServer").Element("PickupDirectoryLocation").Value;
            this.SmtpServer.User = xDoc.Root.Element("SmtpServer").Element("User").Value;
            this.SmtpServer.Password = EncryptionDecryption.Decode(xDoc.Root.Element("SmtpServer").Element("Password").Value);
            this.SmtpServer.ExceptionFrom = xDoc.Root.Element("SmtpServer").Element("ExceptionMailFrom").Value;
            this.SmtpServer.ExceptionTo = xDoc.Root.Element("SmtpServer").Element("ExceptionMailTo").Value;
            this.SmtpServer.UsePickupDirectoryLocation = bool.Parse(xDoc.Root.Element("SmtpServer").Element("UsePickupDirectoryLocation").Value);

            foreach (var item in xDoc.Root.Element("Connections").Elements("Connection"))
            {
                Connection con = new Connection();
                con.Name = item.Attribute("Name").Value;
                con.ConnectionType = item.Element("ConnectionType").Value == ConnectionType.MSSQLSERVER.ToString() ? ConnectionType.MSSQLSERVER : ConnectionType.Vertica;
                con.Host = item.Element("Server").Value;
                con.Username = item.Element("Username").Value;
                con.Password = item.Element("Password").Value;
                con.Database = item.Element("Database").Value;
                //con.TargetTable = item.Element("Table").Value;

                this._connections.Add(con.Name, con);
            }
        }

        private void PopulateTransforms()
        {
            string folder = System.Reflection.Assembly.GetExecutingAssembly().CodeBase.Remove(System.Reflection.Assembly.GetExecutingAssembly().CodeBase.LastIndexOf("/"));
            folder = folder.Replace("file:///", "");
            configFile = folder + "/" + System.Configuration.ConfigurationManager.AppSettings["ConfigFile"];

            XDocument xDoc = XDocument.Load(configFile);

            foreach (var item in xDoc.Root.Element("Transformations").Elements("Transformation"))
            {
                string name = item.Attribute("Name").Value;
                string file = item.Attribute("Location").Value;

                var t = new Transformation(file);
                this._transforms.Add(name, t);
            }
        }

        public Transformation CurrentTranformation { get; set; }

        public void AddTransformation(Transformation transformation)
        {
            if (!this._transforms.ContainsKey(transformation.Name))
            {
                this._transforms.Add(transformation.Name, transformation);

                this.CurrentTranformation = transformation;

                this.Save();
            }
            else
            {
                throw new Exception("The same name transformation exsited!");
            }
        }

        public void RemoveTransformation(Transformation transformation)
        {
            this._transforms.Remove(transformation.Name);
            this.Save();
        }

        public Transformation GetTransformation(string name)
        {
            if (this._transforms.ContainsKey(name))
                return this._transforms[name];
            return null;
        }

        public void NewConnection(Connection connection)
        {
            if (!this._connections.ContainsKey(connection.Name))
            {
                this._connections.Add(connection.Name, connection);
                this.Save();
            }
            else
            {
                throw new Exception("The same name connection existed!");
            }
        }

        public Connection GetConnection(string name)
        {
            if (this._connections.ContainsKey(name))
                return this._connections[name];
            return null;
        }

        public List<Connection> GetConnections()
        {
            return this._connections.Values.ToList();
        }

        public List<string> GetTransformList()
        {
            return this._transforms.Keys.ToList();
        }

        public void Save()
        {
            XDocument Xdoc = XDocument.Parse(@"<EtlConfig>
<SmtpServer>
    <Server></Server>
    <PickupDirectoryLocation></PickupDirectoryLocation>
    <User></User>
    <Password></Password>
    <ExceptionMailFrom></ExceptionMailFrom>
    <ExceptionMailTo></ExceptionMailTo>
    <UsePickupDirectoryLocation></UsePickupDirectoryLocation>
</SmtpServer>
<Transformations></Transformations>
<Connections></Connections>
</EtlConfig>");

            Xdoc.Root.Element("SmtpServer").Element("Server").Value = this.SmtpServer.Server;
            Xdoc.Root.Element("SmtpServer").Element("PickupDirectoryLocation").Value = this.SmtpServer.PickupDirectoryLocation;
            Xdoc.Root.Element("SmtpServer").Element("User").Value = this.SmtpServer.User;
            Xdoc.Root.Element("SmtpServer").Element("Password").Value = EncryptionDecryption.Encode(this.SmtpServer.Password);
            Xdoc.Root.Element("SmtpServer").Element("ExceptionMailFrom").Value = this.SmtpServer.ExceptionFrom;
            Xdoc.Root.Element("SmtpServer").Element("ExceptionMailTo").Value = this.SmtpServer.ExceptionTo;
            Xdoc.Root.Element("SmtpServer").Element("UsePickupDirectoryLocation").Value = this.SmtpServer.UsePickupDirectoryLocation.ToString();

            foreach (var item in this._transforms)
            {
                if (item.Value.IsNew == false)
                {
                    XElement element = new XElement("Transformation");
                    element.SetAttributeValue("Name", item.Key);
                    element.SetAttributeValue("Location", item.Value.Configfile);
                    Xdoc.Root.Element("Transformations").Add(element);
                }
            }

            foreach (var item in this._connections)
            {
                XElement element = new XElement("Connection");
                element.SetAttributeValue("Name", item.Value.Name);
                element.SetElementValue("ConnectionType", item.Value.ConnectionType);
                element.SetElementValue("Server", item.Value.Host);
                element.SetElementValue("Username", item.Value.Username);
                element.SetElementValue("Password", item.Value.Password);
                element.SetElementValue("Database", item.Value.Database);
                element.SetElementValue("Table", item.Value.TargetTable);

                Xdoc.Root.Element("Connections").Add(element);
            }

            Xdoc.Save(this.configFile);
        }
    }
}
