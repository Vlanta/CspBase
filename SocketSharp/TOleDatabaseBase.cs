using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.OleDb;

namespace CSUST
{
    /// <summary>
    /// OldDb数据库类, 可以再派生并增加属性与字段
    /// </summary>
    public class TOleDatabaseBase : TDatabaseBase
    {
        public override DbConnection DbConnection
        {
            get
            {
                OleDbConnection dbConn = m_dbConnection as OleDbConnection;
                return dbConn;
            }
        }

        public override void Open()
        {
            try
            {
                this.Close();

                m_dbConnection = new OleDbConnection();
                m_dbConnection.ConnectionString = m_dbConnectionString;

                m_dbConnection.Open();
            }
            catch (Exception err)
            {
                this.OnDatabaseOpenException(err);
            }
        }
    }

}
