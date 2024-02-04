#region Usings

using MSRepairs.Engine;
using MSRepairs.Exceptions;
using MSRepairs.Models;
using System.Data.SqlClient;
using System.Net.Mail;

#endregion

namespace MSRepairs.Database_Methods
{

    public class RepairMethod
    {

        protected MSRepairsEngine mSRepairsEngine;

        public RepairMethod(MSRepairsEngine mSRepairsEngine)
        {

            this.mSRepairsEngine = mSRepairsEngine;

        }

        public bool CreateReparation(int managerId, string requestId, int garageid, int userId)
        {

            // Variables
            bool setNewConnection;

            // Flag that defines if a new connection must be settle to database
            setNewConnection = !this.mSRepairsEngine.SQLServer.IsConnectionOpen();


            try
            {

                // If a new connection is required, open it and initiate a new transaction
                if (setNewConnection)
                    this.mSRepairsEngine.SQLServer.OpenConnection();


                // Get number of affected records from database engine
                if (this.mSRepairsEngine.SQLServer.ExecuteCreateReparation(managerId, requestId, garageid, userId) == 1)
                {

                    // If a new connection was required, commit the transaction and close it
                    if (setNewConnection)
                        this.mSRepairsEngine.SQLServer.CloseConnection();

                    return true;

                }
                else
                {

                    // If a new connection was required, commit the transaction and close it
                    if (setNewConnection)
                        this.mSRepairsEngine.SQLServer.CloseConnection();

                    return false;

                }

            }
            catch (SqlException)
            {

                // Close connection
                if (this.mSRepairsEngine.SQLServer.IsConnectionOpen())
                    this.mSRepairsEngine.SQLServer.CloseConnection();

                throw new MSRepairsException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Close connection
                if (this.mSRepairsEngine.SQLServer.IsConnectionOpen())
                    this.mSRepairsEngine.SQLServer.CloseConnection();

                throw new MSRepairsException(ex.Message);

            }

        }


        public List<Repair> CurrentReparations()
        {

             // Variables
            bool setNewConnection;
            List<Repair> repair;

            // Flag that defines if a new connection must be settle to database
            setNewConnection = !this.mSRepairsEngine.SQLServer.IsConnectionOpen();


            try
            {

                // If a new connection is required, open it and initiate a new transaction
                if (setNewConnection)
                    this.mSRepairsEngine.SQLServer.OpenConnection();


                // Get number of affected records from database engine
                repair = this.mSRepairsEngine.SQLServer.ExecuteCurrentReparations();
               
                if (repair != null)
                {
                    if (setNewConnection)
                        this.mSRepairsEngine.SQLServer.CloseConnection();

                    return repair;
                }
                else
                {

                    // If a new connection was required, commit the transaction and close it
                    if (setNewConnection)
                        this.mSRepairsEngine.SQLServer.CloseConnection();

                    return null;

                }

            }
            catch (SqlException)
            {

                // Close connection
                if (this.mSRepairsEngine.SQLServer.IsConnectionOpen())
                    this.mSRepairsEngine.SQLServer.CloseConnection();

                throw new MSRepairsException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Close connection
                if (this.mSRepairsEngine.SQLServer.IsConnectionOpen())
                    this.mSRepairsEngine.SQLServer.CloseConnection();

                throw new MSRepairsException(ex.Message);

            }

        }


        public List<Repair> CurrentReparationsUser(int userId)
        {

            // Variables
            bool setNewConnection;
            List<Repair> repair;

            // Flag that defines if a new connection must be settle to database
            setNewConnection = !this.mSRepairsEngine.SQLServer.IsConnectionOpen();


            try
            {

                // If a new connection is required, open it and initiate a new transaction
                if (setNewConnection)
                    this.mSRepairsEngine.SQLServer.OpenConnection();


                // Get number of affected records from database engine
                repair = this.mSRepairsEngine.SQLServer.ExecuteCurrentReparationsUser(userId);

                if (repair != null)
                {
                    if (setNewConnection)
                        this.mSRepairsEngine.SQLServer.CloseConnection();

                    return repair;
                }
                else
                {

                    // If a new connection was required, commit the transaction and close it
                    if (setNewConnection)
                        this.mSRepairsEngine.SQLServer.CloseConnection();

                    return null;

                }

            }
            catch (SqlException)
            {

                // Close connection
                if (this.mSRepairsEngine.SQLServer.IsConnectionOpen())
                    this.mSRepairsEngine.SQLServer.CloseConnection();

                throw new MSRepairsException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Close connection
                if (this.mSRepairsEngine.SQLServer.IsConnectionOpen())
                    this.mSRepairsEngine.SQLServer.CloseConnection();

                throw new MSRepairsException(ex.Message);

            }

        }


        public List<Repair> UserHistoryRepairs(int userId)
        {

            // Variables
            bool setNewConnection;
            List<Repair> repair;

            // Flag that defines if a new connection must be settle to database
            setNewConnection = !this.mSRepairsEngine.SQLServer.IsConnectionOpen();


            try
            {

                // If a new connection is required, open it and initiate a new transaction
                if (setNewConnection)
                    this.mSRepairsEngine.SQLServer.OpenConnection();


                // Get number of affected records from database engine
                repair = this.mSRepairsEngine.SQLServer.UserHistoryRepairs(userId);

                if (repair != null)
                {
                    if (setNewConnection)
                        this.mSRepairsEngine.SQLServer.CloseConnection();

                    return repair;
                }
                else
                {

                    // If a new connection was required, commit the transaction and close it
                    if (setNewConnection)
                        this.mSRepairsEngine.SQLServer.CloseConnection();

                    return null;

                }

            }
            catch (SqlException)
            {

                // Close connection
                if (this.mSRepairsEngine.SQLServer.IsConnectionOpen())
                    this.mSRepairsEngine.SQLServer.CloseConnection();

                throw new MSRepairsException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Close connection
                if (this.mSRepairsEngine.SQLServer.IsConnectionOpen())
                    this.mSRepairsEngine.SQLServer.CloseConnection();

                throw new MSRepairsException(ex.Message);

            }

        }




        





        public bool ReparationState(string repairId, string stateId, string serviceTypeId, int workTime, string notes, int userId)
        {

            // Variables
            bool setNewConnection;

            // Flag that defines if a new connection must be settle to database
            setNewConnection = !this.mSRepairsEngine.SQLServer.IsConnectionOpen();


            try
            {

                // If a new connection is required, open it and initiate a new transaction
                if (setNewConnection)
                    this.mSRepairsEngine.SQLServer.OpenConnection();


                // Get number of affected records from database engine
                if (this.mSRepairsEngine.SQLServer.ExecuteReparationState(repairId, stateId, serviceTypeId, workTime, notes, userId) == 1)
                {

                    // If a new connection was required, commit the transaction and close it
                    if (setNewConnection)
                        this.mSRepairsEngine.SQLServer.CloseConnection();

                    return true;

                }
                else
                {

                    // If a new connection was required, commit the transaction and close it
                    if (setNewConnection)
                        this.mSRepairsEngine.SQLServer.CloseConnection();

                    return false;

                }

            }
            catch (SqlException)
            {

                // Close connection
                if (this.mSRepairsEngine.SQLServer.IsConnectionOpen())
                    this.mSRepairsEngine.SQLServer.CloseConnection();

                throw new MSRepairsException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Close connection
                if (this.mSRepairsEngine.SQLServer.IsConnectionOpen())
                    this.mSRepairsEngine.SQLServer.CloseConnection();

                throw new MSRepairsException(ex.Message);

            }

        }




        public bool AddPart(string token, string repairId, string stockId, int quantity, int userId)
        {

            // Variables
            bool setNewConnection;

            // Flag that defines if a new connection must be settle to database
            setNewConnection = !this.mSRepairsEngine.SQLServer.IsConnectionOpen();


            try
            {

                // If a new connection is required, open it and initiate a new transaction
                if (setNewConnection)
                    this.mSRepairsEngine.SQLServer.OpenConnection();


                // Get number of affected records from database engine
                if (this.mSRepairsEngine.SQLServer.ExecuteAddPart(token, repairId, stockId, quantity, userId) == 2)
                {

                    // If a new connection was required, commit the transaction and close it
                    if (setNewConnection)
                        this.mSRepairsEngine.SQLServer.CloseConnection();

                    return true;

                }
                else
                {

                    // If a new connection was required, commit the transaction and close it
                    if (setNewConnection)
                        this.mSRepairsEngine.SQLServer.CloseConnection();

                    return false;

                }

            }
            catch (SqlException)
            {

                // Close connection
                if (this.mSRepairsEngine.SQLServer.IsConnectionOpen())
                    this.mSRepairsEngine.SQLServer.CloseConnection();

                throw new MSRepairsException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Close connection
                if (this.mSRepairsEngine.SQLServer.IsConnectionOpen())
                    this.mSRepairsEngine.SQLServer.CloseConnection();

                throw new MSRepairsException(ex.Message);

            }

        }

    }

}
