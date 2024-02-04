using System.Data.SqlClient;
using MSGarages.Engine;
using MSGarages.Exceptions;
using MSGarages.Tools;
using MSGarages.Models;

namespace MSGarages.Database_Methods
{
    public class GarageMethod
    {
        // State variables
        protected MSGaragesEngine mSGaragesEngine;


        public GarageMethod(MSGaragesEngine mSGaragesEngine)
        {
            this.mSGaragesEngine = mSGaragesEngine;
        }

        public bool GarageExists(string adress, string name)
        {

            // Variables
            int count;
            bool setNewConnection;

            // Flag that defines if a new connection must be settle to database
            setNewConnection = !this.mSGaragesEngine.SQLServer.IsConnectionOpen();

            try
            {
                // If a new connection is required, open-it
                if (setNewConnection)
                    this.mSGaragesEngine.SQLServer.OpenConnection();

                // Get counter from database engine
                count = (int)this.mSGaragesEngine.SQLServer.HasGarage(adress, name);

                if (count > 0)
                    return false;

                // If a new connection was required, close-it
                if (setNewConnection)
                    this.mSGaragesEngine.SQLServer.CloseConnection();

            }
            catch (SqlException)
            {

                // Close connection
                if (this.mSGaragesEngine.SQLServer.IsConnectionOpen())
                    this.mSGaragesEngine.SQLServer.CloseConnection();

                throw new MSGaragesException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception)
            {

                // Close connection
                if (this.mSGaragesEngine.SQLServer.IsConnectionOpen())
                    this.mSGaragesEngine.SQLServer.CloseConnection();

                throw new MSGaragesException(ExceptionsDetails.OTHERS_EXCEPTION_PREFIX_ERROR);

            }

            return true;

        }




        public bool CreateGarage(int userId, string adress, string name, string postal_code)
        {

            // Variables
            bool setNewConnection;

            if (!MSGaragesValidator.IsValidPostalCode(postal_code))
                throw new MSGaragesException(ExceptionsDetails.SPECS_PREFIX_INVALID_VALUE);


            // Flag that defines if a new connection must be settle to database
            setNewConnection = !this.mSGaragesEngine.SQLServer.IsConnectionOpen();


            try
            {

                // If a new connection is required, open it and initiate a new transaction
                if (setNewConnection)
                    this.mSGaragesEngine.SQLServer.OpenConnection();


                // Get number of affected records from database engine
                if (this.mSGaragesEngine.SQLServer.ExecuteCreateGarage(userId, adress, name, postal_code) == 1)
                {

                    // If a new connection was required, commit the transaction and close it
                    if (setNewConnection)
                        this.mSGaragesEngine.SQLServer.CloseConnection();

                    return true;

                }
                else
                {

                    // If a new connection was required, commit the transaction and close it
                    if (setNewConnection)
                        this.mSGaragesEngine.SQLServer.CloseConnection();

                    return false;

                }

            }
            catch (SqlException)
            {

                // Close connection
                if (this.mSGaragesEngine.SQLServer.IsConnectionOpen())
                    this.mSGaragesEngine.SQLServer.CloseConnection();

                throw new MSGaragesException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Close connection
                if (this.mSGaragesEngine.SQLServer.IsConnectionOpen())
                    this.mSGaragesEngine.SQLServer.CloseConnection();

                throw new MSGaragesException(ex.Message);

            }

        }

        public bool AddEmployeeMethod(string email, int garageid)
        {
            // Variables
            bool setNewConnection;

            // Flag that defines if a new connection must be settle to database
            setNewConnection = !this.mSGaragesEngine.SQLServer.IsConnectionOpen();


            try
            {

                // If a new connection is required, open it and initiate a new transaction
                if (setNewConnection)
                    this.mSGaragesEngine.SQLServer.OpenConnection();


                // Get number of affected records from database engine
                if (this.mSGaragesEngine.SQLServer.ExecuteAddEmployee(email, garageid) == 1)
                {

                    // If a new connection was required, commit the transaction and close it
                    if (setNewConnection)
                        this.mSGaragesEngine.SQLServer.CloseConnection();

                    return true;

                }
                else
                {

                    // If a new connection was required, commit the transaction and close it
                    if (setNewConnection)
                        this.mSGaragesEngine.SQLServer.CloseConnection();

                    return false;

                }

            }
            catch (SqlException)
            {

                // Close connection
                if (this.mSGaragesEngine.SQLServer.IsConnectionOpen())
                    this.mSGaragesEngine.SQLServer.CloseConnection();

                throw new MSGaragesException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Close connection
                if (this.mSGaragesEngine.SQLServer.IsConnectionOpen())
                    this.mSGaragesEngine.SQLServer.CloseConnection();

                throw new MSGaragesException(ex.Message);

            }
        }



        public int EmployeeGarage(int userId)
        {
            // Variables
            bool setNewConnection;
            int garageid;

            // Flag that defines if a new connection must be settle to database
            setNewConnection = !this.mSGaragesEngine.SQLServer.IsConnectionOpen();


            try
            {

                // If a new connection is required, open it and initiate a new transaction
                //if (setNewConnection)
                    this.mSGaragesEngine.SQLServer.OpenConnection();


                // Get number of affected records from database engine
                garageid = this.mSGaragesEngine.SQLServer.EmployeeGarage(userId);


                this.mSGaragesEngine.SQLServer.CloseConnection();
            }



            //else



            //{

            //    // If a new connection was required, commit the transaction and close it
            //    if (setNewConnection)
            //        this.mSGaragesEngine.SQLServer.CloseConnection();

            //    return false;

            //}

            //}
            //catch (SqlException)
            //{

            //    // Close connection
            //    if (this.mSGaragesEngine.SQLServer.IsConnectionOpen())
            //        this.mSGaragesEngine.SQLServer.CloseConnection();

            //    throw new MSGaragesException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            //}


            catch (Exception ex)
            {

                // Close connection
                if (this.mSGaragesEngine.SQLServer.IsConnectionOpen())
                    this.mSGaragesEngine.SQLServer.CloseConnection();

                throw new MSGaragesException(ex.Message);

            }

            return garageid;
        }

        //public List<User> GarageEmployees(int garageId)
        //{

        //    // Variables
        //    bool setNewConnection;
        //    List<User> user;

        //    // Flag that defines if a new connection must be settle to database
        //    setNewConnection = !this.mSGaragesEngine.SQLServer.IsConnectionOpen();


        //    try
        //    {

        //        // If a new connection is required, open it and initiate a new transaction
        //        if (setNewConnection)
        //            this.mSGaragesEngine.SQLServer.OpenConnection();


        //        // Get number of affected records from database engine
        //        user = this.mSGaragesEngine.SQLServer.GarageEmployees(garageId);

        //        if (user != null)
        //        {
        //            if (setNewConnection)
        //                this.mSGaragesEngine.SQLServer.CloseConnection();

        //            return user;
        //        }
        //        else
        //        {

        //            // If a new connection was required, commit the transaction and close it
        //            if (setNewConnection)
        //                this.mSGaragesEngine.SQLServer.CloseConnection();

        //            return null;

        //        }

        //    }
        //    catch (SqlException)
        //    {

        //        // Close connection
        //        if (this.mSGaragesEngine.SQLServer.IsConnectionOpen())
        //            this.mSGaragesEngine.SQLServer.CloseConnection();

        //        throw new MSGaragesException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

        //    }
        //    catch (Exception ex)
        //    {

        //        // Close connection
        //        if (this.mSGaragesEngine.SQLServer.IsConnectionOpen())
        //            this.mSGaragesEngine.SQLServer.CloseConnection();

        //        throw new MSGaragesException(ex.Message);

        //    }

        //}



    }

}
