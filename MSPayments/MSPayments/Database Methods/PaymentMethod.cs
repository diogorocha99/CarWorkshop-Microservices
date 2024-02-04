#region Usings

using Microsoft.AspNetCore.Mvc;
using MSPayments.Engine;
using MSPayments.Exceptions;
using MSPayments.Model;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

#endregion

#region PaymentMethod

namespace MSPayments.Database_Methods
{

    public class PaymentMethod
    {

        protected MSPaymentsEngine mSPaymentsEngine;

        public PaymentMethod(MSPaymentsEngine mSPaymentsEngine)
        {

            this.mSPaymentsEngine = mSPaymentsEngine;

        }


        public void VerifyPayment(int UserId, string RepairId, string LicensePlate, string ServiceTypeId, double Price)
        {
            int recordsAffected;
            bool setNewConnection;



            setNewConnection = !this.mSPaymentsEngine.SQLServer.IsConnectionOpen();

            try
            {

                // If a new connection is required, open it and initiate a new transaction
                if (setNewConnection)
                    this.mSPaymentsEngine.SQLServer.OpenConnection();

                // Get number of affected records from database engine
                recordsAffected = this.mSPaymentsEngine.SQLServer.ExecutePayment(UserId, RepairId, LicensePlate, ServiceTypeId, Price);

                // If a new connection was required, commit the transaction and close it
                if (setNewConnection)
                    this.mSPaymentsEngine.SQLServer.CloseConnection();

            }
            catch (SqlException)
            {

                // Close connection
                if (this.mSPaymentsEngine.SQLServer.IsConnectionOpen())
                    this.mSPaymentsEngine.SQLServer.CloseConnection();

                throw new MSPaymentsException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception)
            {

                // Close connection
                if (this.mSPaymentsEngine.SQLServer.IsConnectionOpen())
                    this.mSPaymentsEngine.SQLServer.CloseConnection();

                throw new MSPaymentsException(ExceptionsDetails.OTHERS_EXCEPTION_PREFIX_ERROR);

            }
        }


        public List<Payments> UserHistoryPayments(int userId)
        {

            // Variables
            bool setNewConnection;
            List<Payments> payments;

            // Flag that defines if a new connection must be settle to database
            setNewConnection = !this.mSPaymentsEngine.SQLServer.IsConnectionOpen();


            try
            {

                // If a new connection is required, open it and initiate a new transaction
                if (setNewConnection)
                    this.mSPaymentsEngine.SQLServer.OpenConnection();


                // Get number of affected records from database engine
                payments = this.mSPaymentsEngine.SQLServer.UserHistoryPayments(userId);

                if (payments != null)
                {
                    if (setNewConnection)
                        this.mSPaymentsEngine.SQLServer.CloseConnection();

                    return payments;
                }
                else
                {

                    // If a new connection was required, commit the transaction and close it
                    if (setNewConnection)
                        this.mSPaymentsEngine.SQLServer.CloseConnection();

                    return null;

                }

            }
            catch (SqlException)
            {

                // Close connection
                if (this.mSPaymentsEngine.SQLServer.IsConnectionOpen())
                    this.mSPaymentsEngine.SQLServer.CloseConnection();

                throw new MSPaymentsException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Close connection
                if (this.mSPaymentsEngine.SQLServer.IsConnectionOpen())
                    this.mSPaymentsEngine.SQLServer.CloseConnection();

                throw new MSPaymentsException(ex.Message);

            }

        }

    }
}

#endregion
