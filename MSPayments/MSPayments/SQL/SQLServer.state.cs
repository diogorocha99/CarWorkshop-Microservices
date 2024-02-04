#region Usings

using System.Data;

#endregion

#region SQLServer State

namespace MSPayments.SQL
{

    public partial class SQLServer
    {

        #region IsConnectionOpen

        /// <summary>
        /// Use this method if you want to know if a connection is stablished with sql server
        /// </summary>
        /// <returns>If true you can use the connection</returns>
        public bool IsConnectionOpen()
        {

            if (sqlConnection == null)
                return false;

            if (sqlConnection.State == ConnectionState.Broken || sqlConnection.State == ConnectionState.Closed)
                return false;

            return true;

        }

        #endregion

    }

}

#endregion
