using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIETLUtility.Delta
{
    public class PrimaryKey
    {
        public string Table { get; set; }

        public List<string> NameList { get; set; }

        public List<Dictionary<string, string>> Collection { get; private set; }

        public List<string> ValueList { get; private set; }

        public bool HasPrimaryKey
        {
            get
            {
               return this.NameList.Count > 0;
            }
        }

        public bool IsMultiplePrimaryKey
        {
            get
            {
                return this.NameList.Count > 1;
            }
        }

        public string NameString
        {
            get
            {
                return string.Join(",", this.NameList);
            }
        }

        public string CreateTempTableScript
        {
            get
            {
                return string.Format("select {0} into {1}_temp from {1} limit 0;", string.Join(",", this.NameList), this.Table);
            }
        }

        public string TruncateTempTableScript
        {
            get
            {
                return string.Format("drop table {0}_temp;", this.Table);
            }
        }

        public string DeleteStatement
        {
            get
            {
                StringBuilder statement = new StringBuilder();

                if (IsMultiplePrimaryKey)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var key in this.NameList)
                    {
                        sb.Append(string.Format("and {0}.{1} = {2}.{1} ", this.Table, key, this.Table + "_temp"));
                    }

                    return string.Format("delete from {0} where exists (select * from {1} where {2});", this.Table, this.Table + "_temp", sb.Remove(0, 3));
                }
                else
                {
                    foreach (var item in ValueList)
                    {
                        statement.Append("'" + item + "'");
                        statement.Append(",");
                    }

                    statement.Remove(statement.Length - 1, 1);

                    return string.Format("Delete From {0} WHERE {1} IN ({2})", this.Table, this.NameList[0], statement.ToString());
                }
            }
        }

        public PrimaryKey()
        {
            this.Collection = new List<Dictionary<string, string>>();
            this.NameList = new List<string>();
            this.ValueList = new List<string>();
        }


    }
}
