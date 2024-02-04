#region Usings

using System.Data.SqlClient;
using MSGarages.Exceptions;

#endregion

#region SQL Server Main

namespace MSGarages.SQL
{

    public partial class SQLServer
    {

        // Enums
        public enum SQLConnectionModes
        { auto, manual }

        // Variables
        string connectionString;
        SqlConnection sqlConnection;
        SQLConnectionModes openCloseConnectionMode;

        /// <summary>
        /// SQLServer constructor
        /// </summary>
        /// <param name="connectionString">connection string to connect to database</param>
        /// <param name="sqlConnectionMode">sql connection mode to be used on the connection with database</param>
        public SQLServer(string connectionString, SQLConnectionModes sqlConnectionMode)
        {

            this.connectionString = connectionString;
            this.openCloseConnectionMode = sqlConnectionMode;

        }


        ~SQLServer()
        {

            if (IsConnectionOpen()) throw new MSGaragesException(ExceptionsDetails.SQLSERVER_DESTRUCTION_CONNECTION_STILL_OPEN);

        }

    }

}

#endregion
