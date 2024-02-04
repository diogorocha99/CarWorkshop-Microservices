#region Usings

using MSUsers.Engine;
using MSUsers.Exceptions;
using System.Data.SqlClient;

#endregion

#region AuthenticationMethod

namespace MSUsers.Database_Methods
{

    public class AuthenticationMethod
    {

        // State variables
        protected MSUsersEngine mSUsersEngine;
 
        public AuthenticationMethod(MSUsersEngine mSUsersEngine)
        {

            this.mSUsersEngine = mSUsersEngine;

        }

        #region LoginGetCount

        /// <summary>
        /// Method used to initialize a database connection
        /// </summary>
        /// <param name="email">Users email</param>
        /// <param name="password">Users password</param>
        /// <returns>Returns the count from the database</returns>
        /// <exception cref="MSUsersException"></exception>
        public bool LoginGetCount(string email, string password)
        {

            // Local variables
            int countLogin;
            bool setNewConnection;

            // Flag that defines if a new connection must be settle to database
            setNewConnection = !this.mSUsersEngine.SQLServer.IsConnectionOpen();

            try
            {

                // If a new connection is required, open-it
                if (setNewConnection)
                    this.mSUsersEngine.SQLServer.OpenConnection();

                // Get counter from database engine
                countLogin = (int)this.mSUsersEngine.SQLServer.ExecuteHasAccount(email, password);

                if(countLogin > 0) return true;

                // If a new connection was required, close-it
                if (setNewConnection)
                    this.mSUsersEngine.SQLServer.CloseConnection();

            }
            catch (SqlException)
            {

                // Close connection
                if (this.mSUsersEngine.SQLServer.IsConnectionOpen())
                    this.mSUsersEngine.SQLServer.CloseConnection();

                throw new MSUsersException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Close connection
                if (this.mSUsersEngine.SQLServer.IsConnectionOpen())
                    this.mSUsersEngine.SQLServer.CloseConnection();

                throw new MSUsersException(ex.Message);

            }

            return false;

        }

        #endregion

        #region AddLoginAttemps

        public void AddLoginAttemps(string email)
        {

            // Local variables
            bool setNewConnection;

            // Flag that defines if a new connection must be settle to database
            setNewConnection = !this.mSUsersEngine.SQLServer.IsConnectionOpen();

            try
            {

                // If a new connection is required, open-it
                if (setNewConnection)
                    this.mSUsersEngine.SQLServer.OpenConnection();

                // Get counter from database engine
                this.mSUsersEngine.SQLServer.ExecuteAddLoginAttemps(email);

                // If a new connection was required, close-it
                if (setNewConnection)
                    this.mSUsersEngine.SQLServer.CloseConnection();

            }
            catch (SqlException)
            {

                // Close connection
                if (this.mSUsersEngine.SQLServer.IsConnectionOpen())
                    this.mSUsersEngine.SQLServer.CloseConnection();

                throw new MSUsersException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Close connection
                if (this.mSUsersEngine.SQLServer.IsConnectionOpen())
                    this.mSUsersEngine.SQLServer.CloseConnection();

                throw new MSUsersException(ex.Message);

            }

        }

        #endregion

    }

}

#endregion
