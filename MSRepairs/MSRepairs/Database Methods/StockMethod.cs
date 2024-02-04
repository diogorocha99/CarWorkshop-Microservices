using MSRepairs.Engine;
using MSRepairs.Exceptions;
using MSRepairs.Models;
using System.Data.SqlClient;

namespace MSRepairs.Database_Methods
{
    public class StockMethod
    {

        protected MSRepairsEngine mSRepairsEngine;

        public StockMethod(MSRepairsEngine mSRepairsEngine)
        {

            this.mSRepairsEngine = mSRepairsEngine;

        }

        public bool RemoveStock(string stockId, int quantity, int garageId)
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
                if (this.mSRepairsEngine.SQLServer.ExecuteRemoveStock(stockId, quantity, garageId) == 1)
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


        public List<Stock> GetStock(int garageId)
        {

            // Variables
            bool setNewConnection;
            List<Stock> stock;

            // Flag that defines if a new connection must be settle to database
            setNewConnection = !this.mSRepairsEngine.SQLServer.IsConnectionOpen();


            try
            {

                // If a new connection is required, open it and initiate a new transaction
                if (setNewConnection)
                    this.mSRepairsEngine.SQLServer.OpenConnection();


                // Get number of affected records from database engine
                stock = this.mSRepairsEngine.SQLServer.ExecuteGetStock(garageId);

                if (stock != null)
                {
                    if (setNewConnection)
                        this.mSRepairsEngine.SQLServer.CloseConnection();

                    return stock;
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

    }

}
