using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIETLUtility.Configuration;
using Vertica.Data.VerticaClient;

namespace BIETLUtility.Delta
{
    public class DeltaUtility
    {
        private VerticaConnection _verticaConnection;
        private Transformation _transformation;
        private string fileName;

        public DeltaUtility(Transformation transformation, VerticaConnection verticaConnection)
        {
            this._verticaConnection = verticaConnection;
            this._transformation = transformation;
            this.fileName = this._transformation.WorkingFolder.EndsWith("\\") ? this._transformation.WorkingFolder : this._transformation.WorkingFolder + "//";

            this.fileName = this.fileName + "deltaTempfile_" + this._transformation.Name + "_" + DateTime.Now.ToString("HHmmss") + ".txt";
        }

        public void ExecuteDeltaDeleteOnVertica()
        {
            if (this._transformation.PrimaryKey.IsMultiplePrimaryKey)
            {
                this.DoExecuteMultipePrimaryKey();
                this._transformation.PrimaryKey.Collection.Clear();
            }
            else
            {
                this.DoExecuteDeletion();
                this._transformation.PrimaryKey.ValueList.Clear();
            }
            
        }

        private void DoExecuteMultipePrimaryKey()
        {
            try
            {
                this.CreateTempTable();
                this.GeneratePrimaryKeyData();
                this.CopyDataToVertica();
                this.DoExecuteDeletion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.ClearTempTable();
            }
        }

        private void CreateTempTable()
        {
            var command = new VerticaCommand(_transformation.PrimaryKey.CreateTempTableScript, _verticaConnection);
            command.ExecuteNonQuery();
        }

        private void CopyDataToVertica()
        {
            VerticaTransaction txn = _verticaConnection.BeginTransaction();
            FileStream inputfile = File.OpenRead(fileName);
            string copy = "copy " + this._transformation.PrimaryKey.Table + "_temp" + " from stdin delimiter '|' no commit";
            VerticaCopyStream vcs = new VerticaCopyStream(_verticaConnection, copy);
            vcs.Start();
            vcs.AddStream(inputfile, false);
            vcs.Execute();
            long rowsInserted = vcs.Finish();
            IList<long> rowsRejected = vcs.Rejects;
            inputfile.Close();
            txn.Commit();
            inputfile.Dispose();
            File.Delete(fileName);
        }

        private void GeneratePrimaryKeyData()
        {
            if (File.Exists(this.fileName)) 
                File.Delete(this.fileName);

            using (FileStream fs = File.Create(this.fileName))
            {
                StreamWriter sw = new StreamWriter(fs);

                foreach (var item in this._transformation.PrimaryKey.Collection)
                {
                    string value = string.Empty;
                    foreach (var key in item.Keys)
                    {
                        value = value + item[key] + "|";
                    }

                    sw.Write(value);
                    sw.WriteLine();
                }

                sw.Close();
            }
        }

        private void ClearTempTable()
        {
            var command = new VerticaCommand(this._transformation.PrimaryKey.TruncateTempTableScript, _verticaConnection);
            command.ExecuteNonQuery();
        }

        private void DoExecuteDeletion()
        {
            VerticaCommand command = new VerticaCommand(_transformation.PrimaryKey.DeleteStatement, _verticaConnection);

            command.ExecuteNonQuery();
        }
    }
}
