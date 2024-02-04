using MSUsers.Engine;
using MSUsers.Exceptions;
using MSUsers.Models;
using MSUsers.Tools;
using System.Data.SqlClient;

namespace MSUsers.Database_Methods
{
    public class VehicleMethod
    {

        // State variables
        protected MSUsersEngine mSUsersEngine;


        public VehicleMethod(MSUsersEngine mSUsersEngine)
        {
            this.mSUsersEngine = mSUsersEngine;
        }

        public bool ValidateLicensePlate(int userId, string licensePlate)
        {

            // Variables
            bool setNewConnection;

            // Flag that defines if a new connection must be settle to database
            setNewConnection = !this.mSUsersEngine.SQLServer.IsConnectionOpen();


            try
            {

                if (!MSUsersValidators.IsvalidLicensePlate(licensePlate))
                        throw new MSUsersException(ExceptionsDetails.INVALID_LICENSEPLATE);


                // If a new connection is required, open it and initiate a new transaction
                if (setNewConnection)
                    this.mSUsersEngine.SQLServer.OpenConnection();





                // Get number of affected records from database engine
                if (this.mSUsersEngine.SQLServer.ExecuteValidateLicensePlate(userId, licensePlate) == 1)
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


        public bool AddNewVehicle(int userId, Vehicle vehicle)
        {

             // Variables
            bool setNewConnection;

            // Flag that defines if a new connection must be settle to database
            setNewConnection = !this.mSUsersEngine.SQLServer.IsConnectionOpen();


            try
            {

                if (!MSUsersValidators.IsvalidLicensePlate(vehicle.LicensePlate))
                    throw new MSUsersException(ExceptionsDetails.INVALID_LICENSEPLATE);


                // If a new connection is required, open it and initiate a new transaction
                if (setNewConnection)
                    this.mSUsersEngine.SQLServer.OpenConnection();

                // Get number of affected records from database engine
                if (this.mSUsersEngine.SQLServer.ExecuteAddNewVehicle(userId, vehicle) == 1)
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


        public List<Vehicle> GetVehiclesByUser(int userId)
        {

            // Variable
            List<Vehicle> vehicles;
            bool setNewConnection;

            // Flag that defines if a new connection must be settle to database
            setNewConnection = !this.mSUsersEngine.SQLServer.IsConnectionOpen();

            try
            {

                // If a new connection is required, open it
                if (setNewConnection)
                    this.mSUsersEngine.SQLServer.OpenConnection();

                // Get user infomations according to database engine
                return vehicles = this.mSUsersEngine.SQLServer.ExecuteVehiclesByUser(userId);

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

    }

}
