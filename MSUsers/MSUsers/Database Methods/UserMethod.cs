#region Usings

using MSUsers.Tools;
using MSUsers.Engine;
using MSUsers.Models;
using MSUsers.Exceptions;
using System.Data.SqlClient;

#endregion

#region UserMethod

namespace MSUsers.Database_Methods
{
    public class UserMethod
    {

        // State variables
        protected MSUsersEngine mSUsersEngine;


        public UserMethod(MSUsersEngine mSUsersEngine)
        {
            this.mSUsersEngine = mSUsersEngine;
        }

        public void Register(User user)
        {

            // Variables
            int recordsAffected;
            bool setNewConnection;

            // Object validation
            if (!MSUsersValidators.IsValidEmail(user.Email) || !MSUsersValidators.IsValidPassword(user.Password) || !MSUsersValidators.IsValidName(user.Name))
                throw new MSUsersException(ExceptionsDetails.SPECS_PREFIX_INVALID_VALUE);

            // Flag that defines if a new connection must be settle to database
            setNewConnection = !this.mSUsersEngine.SQLServer.IsConnectionOpen();

            try
            {

                // If a new connection is required, open it and initiate a new transaction
                if (setNewConnection)
                    this.mSUsersEngine.SQLServer.OpenConnection();

                // Get number of affected records from database engine
                recordsAffected = this.mSUsersEngine.SQLServer.ExecuteCreateAccount(user);

                // If a new connection was required, commit the transaction and close it
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
            catch (Exception)
            {

                // Close connection
                if (this.mSUsersEngine.SQLServer.IsConnectionOpen())
                    this.mSUsersEngine.SQLServer.CloseConnection();

                throw new MSUsersException(ExceptionsDetails.OTHERS_EXCEPTION_PREFIX_ERROR);

            }
        }


        public bool HasEmailRegistered(string email)
        {

            // Variables
            int count;
            bool setNewConnection;

            // Flag that defines if a new connection must be settle to database
            setNewConnection = !this.mSUsersEngine.SQLServer.IsConnectionOpen();

            try
            {
                // If a new connection is required, open-it
                if (setNewConnection)
                    this.mSUsersEngine.SQLServer.OpenConnection();

                // Get counter from database engine
                count = (int)this.mSUsersEngine.SQLServer.ExecuteHasEmail(email);

                if (count > 0)
                    return false;

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
            catch (Exception)
            {

                // Close connection
                if (this.mSUsersEngine.SQLServer.IsConnectionOpen())
                    this.mSUsersEngine.SQLServer.CloseConnection();

                throw new MSUsersException(ExceptionsDetails.OTHERS_EXCEPTION_PREFIX_ERROR);

            }

            return true;
           
        }


        public User GetUserInformationsLogin(string email)
        {

            // Variables
            User user;
            bool setNewConnection;

            // Flag that defines if a new connection must be settle to database
            setNewConnection = !this.mSUsersEngine.SQLServer.IsConnectionOpen();

            try
            {
                // If a new connection is required, open it and initiate a new transaction
                if (setNewConnection)
                    this.mSUsersEngine.SQLServer.OpenConnection();

                // Get user infomations according to database engine
                return user = (User)this.mSUsersEngine.SQLServer.ExecuteGetUserIdRole(email);

                // If a new connection was required, commit the transaction and close it
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


        public User GetUserInformations(int userId)
        {

            // Variable
            User user;
            bool setNewConnection;

            // Flag that defines if a new connection must be settle to database
            setNewConnection = !this.mSUsersEngine.SQLServer.IsConnectionOpen();

            try
            {

                // If a new connection is required, open it
                if (setNewConnection)
                    this.mSUsersEngine.SQLServer.OpenConnection();
                
                // Get user infomations according to database engine
                return user = this.mSUsersEngine.SQLServer.ExecuteGetAccount(userId);

                // If a new connection is settled, close it
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


        public bool ChangePassword(int userId, string password, string newpassword)
        {

            // Variables
            bool setNewConnection;

            // Flag that defines if a new connection must be settle to database
            setNewConnection = !this.mSUsersEngine.SQLServer.IsConnectionOpen();


            try
            {

                // If a new connection is required, open it and initiate a new transaction
                if (setNewConnection)
                    this.mSUsersEngine.SQLServer.OpenConnection();
                

                // Get number of affected records from database engine
                if (this.mSUsersEngine.SQLServer.ExecuteChangePassword(userId, password, newpassword) == 1)
                {

                    // If a new connection was required, commit the transaction and close it
                    if (setNewConnection)
                        this.mSUsersEngine.SQLServer.CloseConnection();

                    return true;

                }
                else
                {

                    // If a new connection was required, commit the transaction and close it
                    if (setNewConnection)
                        this.mSUsersEngine.SQLServer.CloseConnection();

                    return false;

                }

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


        public bool DeleteUser(int userId, string password)
        {

            // Variables
            bool setNewConnection;

            // Flag that defines if a new connection must be settle to database
            setNewConnection = !this.mSUsersEngine.SQLServer.IsConnectionOpen();


            try
            {

                // If a new connection is required, open it and initiate a new transaction
                if (setNewConnection)
                    this.mSUsersEngine.SQLServer.OpenConnection();

                // Get number of affected records from database engine
                if (this.mSUsersEngine.SQLServer.ExecuteDeleteAccount(userId, password) == 1) {

                    // If a new connection was required, commit the transaction and close it
                    if (setNewConnection)
                        this.mSUsersEngine.SQLServer.CloseConnection();


                    return true;

                } else {

                    // If a new connection was required, commit the transaction and close it
                    if (setNewConnection)
                        this.mSUsersEngine.SQLServer.CloseConnection();

                    return false;

                }

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

    }

}

#endregion
