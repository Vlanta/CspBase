using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;

namespace CSUST
{
    /// <summary>
    /// SqlServer数据库类, 可以再派生并增加属性与字段
    /// </summary>
    public class TSqlServerBase : TDatabaseBase
    {
        public override DbConnection DbConnection
        {
            get
            {
                SqlConnection dbConn = m_dbConnection as SqlConnection;
                return dbConn;
            }
        }

        public override void Open()
        {
            try
            {
                this.Close();

                m_dbConnection = new SqlConnection();
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
