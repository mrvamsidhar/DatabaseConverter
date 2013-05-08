using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConverter.Common
{
    public static class Utils
    {
        public static List<SqlServerList> GetSQLInstanceNames()
        {
            List<SqlServerList> list = new List<SqlServerList>();
            try
            {
                SqlServerList SqlSL = new SqlServerList();
                SqlDataSourceEnumerator instance = SqlDataSourceEnumerator.Instance;
                DataTable table = instance.GetDataSources();
                foreach (DataRow row in table.Rows)
                {
                    SqlSL = new SqlServerList();
                    SqlSL.ServerName = row[0].ToString();
                    SqlSL.InstanceName = row[1].ToString();
                    SqlSL.IsClustered = row[2].ToString();
                    SqlSL.Version = row[3].ToString();
                    list.Add(SqlSL);
                }
            }
            catch
            {
                return null;
            }
            return list;
        }
    }

    [Serializable]
    public class SqlServerList : IComparable, ICloneable
    {
        public SqlServerList()
        {
            ServerName = string.Empty;
            InstanceName = string.Empty;
            IsClustered = string.Empty;
            Version = string.Empty;
        }

        #region ICloneable Members

        public object Clone()
        {
            try
            {
                if (this == null)
                {
                    return null;
                }
                SqlServerList SqlSL = new SqlServerList { ServerName = ServerName, InstanceName = InstanceName, IsClustered = IsClustered, Version = Version };
                return SqlSL;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            try
            {
                if (!(obj is SqlServerList))
                {
                    throw new Exception("obj is not an instance of SqlServerList");
                }
                if (this == null)
                {
                    return -1;
                }
                return ServerName.CompareTo((obj as SqlServerList).ServerName);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        public string ServerName { get; set; }
        public string InstanceName { get; set; }
        public string IsClustered { get; set; }
        public string Version { get; set; }
    }
}
