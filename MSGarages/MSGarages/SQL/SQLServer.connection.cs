using System.Data.SqlClient;
using System.Data;

namespace MSGarages.SQL
{
    public partial class SQLServer
    {
        #region OpenConnection

        /// <summary>
        /// This method establishes connection with database
        /// </summary>
        public void OpenConnection()
        {

            // Create object
            if (sqlConnection == null)
                sqlConnection = new SqlConnection(connectionString);

            // Open connection
            sqlConnection.Open();

        }

        #endregion

        #region CloseConnection

        /// <summary>
        /// Use this method to close connection to database
        /// </summary>
        public void CloseConnection()
        {

            if (sqlConnection.State != ConnectionState.Closed)
            {

                sqlConnection.Close();
                sqlConnection.Dispose();
                sqlConnection = null;

            }

        }

        #endregion
    }
}
