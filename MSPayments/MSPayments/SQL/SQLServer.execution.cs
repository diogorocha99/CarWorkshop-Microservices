#region Usings

using System.Text;
using System.Data;
using Newtonsoft.Json;
using MSPayments.Exceptions;
using System.Data.SqlClient;
using MSPayments.Configurations;
using System.Runtime.Serialization.Json;
using System.Runtime.InteropServices;
using MSPayments.Model;

#endregion

#region SQLServer Execution

namespace MSPayments.SQL
{

    public partial class SQLServer
    {


        public int ExecutePayment(int UserId, string RepairId, string LicensePlate, string ServiceTypeId, double Price) {


            // Variables
            SqlCommand sqlCommand;
            int affectedLines;

            // Validations
            if (!IsConnectionOpen())
            {

                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.OpenConnection();
                else
                    throw new MSPaymentsException(ExceptionsDetails.SQLSERVER_CONNECTIONS_NO_CONNECTION);

            }

            // Prepare execution
            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;

            // Prepare execution
            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;

            try
            {

                sqlCommand.Parameters.AddWithValue("@UserId", UserId);
                sqlCommand.Parameters.AddWithValue("@RepairId", RepairId);
                sqlCommand.Parameters.AddWithValue("@Plate", LicensePlate);
                sqlCommand.Parameters.AddWithValue("@ServiceTypeId", ServiceTypeId);
                sqlCommand.Parameters.AddWithValue("@Price", Price);

                //  Query
                sqlCommand.CommandText = @"INSERT INTO [dbo].Payments (PaymentId, UserId, RepairId, Plate, StateId, ServiceTypeId, Price) VALUES (NEWID(), @UserId, @RepairId, @Plate, cast('be88317d-fcdc-4df2-9d2d-00d3886e46c0' AS uniqueidentifier), @ServiceTypeId, @Price)";


                // Execute query and keep number of affectedLines
                affectedLines = sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSPaymentsException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSPaymentsException(ExceptionsDetails.OTHERS_EXCEPTION_PREFIX_ERROR);

            }
            finally
            {

                // Cispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

            }

            // Return object returned by sql engine
            return affectedLines;

        }




        public List<Payments> UserHistoryPayments(int userId)
        {

            // Variables
            SqlCommand sqlCommand;
            DataTable dataTable = new DataTable();
            List<Payments> paymentList = new List<Payments>();

            // Validations
            if (!IsConnectionOpen())
            {

                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.OpenConnection();
                else
                    throw new MSPaymentsException(ExceptionsDetails.SQLSERVER_CONNECTIONS_NO_CONNECTION);

            }

            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;

            try
            {
                //Parameters
                sqlCommand.Parameters.AddWithValue("@UserId", userId);


                // Query
                sqlCommand.CommandText = @"SELECT * FROM [dbo].Payments WHERE UserId = @UserId";

                // Execute query and bring information
                sqlCommand.ExecuteNonQuery();

                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);

                dataAdapter.Fill(dataTable);

                string json = JsonConvert.SerializeObject(dataTable);

                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<Payments>));

                var ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
                List<Payments> h = (List<Payments>)jsonSerializer.ReadObject(ms);

                foreach (var payments in h)
                    paymentList.Add(new Payments(payments.PaymentId, payments.RepairId, payments.LicensePlate, payments.StateId, payments.ServiceTypeId, payments.Price, payments.Date));

            }
            catch (SqlException)
            {

                // dispose object
                sqlCommand.Dispose();

                // close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSPaymentsException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSPaymentsException(ex.Message);

            }
            finally
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

            }

            // Return object returned by sql engine
            return paymentList;

        }
    }

}

#endregion
