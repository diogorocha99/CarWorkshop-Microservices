#region Usings

using System.Text;
using System.Data;
using Newtonsoft.Json;
using MSRequests.Models;
using MSRequests.Exceptions;
using System.Data.SqlClient;
using MSRequests.Configurations;
using System.Runtime.Serialization.Json;
using RestSharp;
using System.Net;

#endregion

#region SQLServer Execution

namespace MSRequests.SQL
{

    public partial class SQLServer
    {

        public int ExecuteCreateRequest(int userId, string licensePlate, int garageid)
        {

            if (garageid > 3) {
                throw new MSRequestsException(ExceptionsDetails.GARAGE_DOES_NOT_EXIST);
            }


            string url = "http://host.docker.internal:7023/api/Vehicle";

            var client = new RestClient(url);
            var request = new RestRequest(url, Method.Get);

            request.AddHeader("userId", userId);
            request.AddHeader("licensePlate", licensePlate);

            RestResponse response = client.Execute(request);

            if(response.StatusCode != HttpStatusCode.OK)
                throw new MSRequestsException(ExceptionsDetails.INVALID_LICENSEPLATE);

            // Variables
            int affectedLines;
            SqlCommand sqlCommand;

            // Validations
            if (!IsConnectionOpen())
            {

                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.OpenConnection();
                else
                    throw new MSRequestsException(ExceptionsDetails.SQLSERVER_CONNECTIONS_NO_CONNECTION);

            }

            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;

            try
            {

                // Parameters
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                sqlCommand.Parameters.AddWithValue("@LicensePlate", licensePlate);
                sqlCommand.Parameters.AddWithValue("@GarageId", garageid);

                // Query
                sqlCommand.CommandText = @"INSERT INTO[dbo].Requests(RequestId, ManagerUserId, UserId, GarageId, LicensePlate, Validated, RequestDate, ValidateDateTime, Inactive) VALUES(NEWID(), NULL, @UserId, @GarageId ,@LicensePlate, NULL, GETDATE(), NULL, cast(0 AS bit));";

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

                throw new MSRequestsException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSRequestsException(ex.Message);

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
            return affectedLines;

        }


        public int ExecuteValidateRequest(int userId, string requestId, bool validate, string token)
        {

            // Variables
            int affectedLines;
            SqlCommand sqlCommand;
            int garageIdresponse;
            Int32 garageId;
            int userrequestId;

            // Validations
            if (!IsConnectionOpen())
            {

                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.OpenConnection();
                else
                    throw new MSRequestsException(ExceptionsDetails.SQLSERVER_CONNECTIONS_NO_CONNECTION);

            }

            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;



            string url = "http://host.docker.internal:7140/api/Garage/CheckEmployeeGarage";

            var client = new RestClient(url);
            var request = new RestRequest(url, Method.Get);

            request.AddHeader("userId", userId);
            request.AddHeader("token", token);

            RestResponse response = client.Execute(request);

            var responseformated = response.Content;

            garageIdresponse = Convert.ToInt32(responseformated);



            try
            {


                // Parameters
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                sqlCommand.Parameters.AddWithValue("@Validate", validate);
                sqlCommand.Parameters.AddWithValue("@RequestId", requestId);



                sqlCommand.CommandText = @"SELECT GarageId FROM dbo.[Requests] where requestId = @RequestId";

                garageId = Convert.ToInt32(sqlCommand.ExecuteScalar());



                if (garageId != garageIdresponse)
                    throw new MSRequestsException(ExceptionsDetails.INVALID_PERMISSION);

                sqlCommand.CommandText = @"SELECT UserId FROM dbo.[Requests] where requestId = @RequestId";

                userrequestId = Convert.ToInt32(sqlCommand.ExecuteScalar());


                // Query
                sqlCommand.CommandText = @"UPDATE dbo.[Requests] SET ManagerUserId = @UserId, Validated = @Validate, ValidateDateTime = GETDATE(), Inactive = cast(0 AS bit) WHERE RequestId = @RequestId AND Validated IS NULL;";

                // Execute query and keep number of affectedLines
                affectedLines = sqlCommand.ExecuteNonQuery();


                string urlRequestId = "http://host.docker.internal:7159/api/Repair";

                var clientRequestId = new RestClient(urlRequestId);
                var requestRequestId = new RestRequest(urlRequestId, Method.Post);

                requestRequestId.AddHeader("managerId", userId);
                requestRequestId.AddHeader("requestId", requestId);
                requestRequestId.AddHeader("garageId", garageId);
                requestRequestId.AddHeader("userId", userrequestId);


                RestResponse responseId = clientRequestId.Execute(requestRequestId);

                if (response.StatusCode != HttpStatusCode.OK)
                    throw new MSRequestsException(ExceptionsDetails.SOMETHING_WENT_WRONG_WHILE_CREATING_REPARATION);

            }
            catch (SqlException)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSRequestsException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSRequestsException(ex.Message);

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
            return affectedLines;

        }


        public List<Requests> ExecuteUnvalidatedRequests()
        {

            // Variables
            SqlCommand sqlCommand;
            List<Requests> requestsList = new List<Requests>();
            DataTable dataTable = new DataTable();

            // Validations
            if (!IsConnectionOpen())
            {

                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.OpenConnection();
                else
                    throw new MSRequestsException(ExceptionsDetails.SQLSERVER_CONNECTIONS_NO_CONNECTION);

            }

            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;

            try
            {

                // Query
                sqlCommand.CommandText = @"SELECT RequestId, UserId, LicensePlate, GarageId FROM [dbo].Requests WHERE Validated IS NULL";

                // Execute query and bring information
                sqlCommand.ExecuteNonQuery();

                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);

                dataAdapter.Fill(dataTable);

                string json = JsonConvert.SerializeObject(dataTable);

                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<Requests>));

                var ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
                List<Requests> requests = (List<Requests>)jsonSerializer.ReadObject(ms);

                foreach (var request in requests)
                    requestsList.Add(new Requests(request.RequestId, request.UserId, request.LicensePlate, request.GarageId));

            }
            catch (SqlException)
            {

                // dispose object
                sqlCommand.Dispose();

                // close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSRequestsException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSRequestsException(ex.Message);

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
            return requestsList;

        }


        public Requests ExecuteRequestInformation(string requestId)
        {
            // Variables
            SqlCommand sqlCommand;
            Requests returnedRequest = new Requests("", 0, "", 0);

            // Validations
            if (!IsConnectionOpen())
            {

                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.OpenConnection();
                else
                    throw new MSRequestsException(ExceptionsDetails.SQLSERVER_CONNECTIONS_NO_CONNECTION);

            }

            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;

            try
            {

                // Parameters
                sqlCommand.Parameters.AddWithValue("@RequestId", requestId);

                // Query
                sqlCommand.CommandText = @"SELECT UserId, LicensePlate, GarageId FROM [dbo].Requests WHERE RequestId = @RequestId;";

                SqlDataReader executeRequestId = sqlCommand.ExecuteReader();

                while (executeRequestId.Read())
                {
                    returnedRequest = new Requests(requestId, int.Parse(executeRequestId["UserId"].ToString()), executeRequestId["LicensePlate"].ToString(), int.Parse(executeRequestId["GarageId"].ToString()));    
                }
                executeRequestId.Close();

            }
            catch (SqlException)
            {

                // dispose object
                sqlCommand.Dispose();

                // close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSRequestsException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSRequestsException(ex.Message);

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
            return returnedRequest;

        }



        public List<Requests> GetRequestsUser(int userId)
        {

            // Variables
            SqlCommand sqlCommand;
            List<Requests> requestsList = new List<Requests>();
            DataTable dataTable = new DataTable();

            // Validations
            if (!IsConnectionOpen())
            {

                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.OpenConnection();
                else
                    throw new MSRequestsException(ExceptionsDetails.SQLSERVER_CONNECTIONS_NO_CONNECTION);

            }

            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;

            try
            {
                //Parameters
                sqlCommand.Parameters.AddWithValue("@UserId", userId);

                // Query
                sqlCommand.CommandText = @"SELECT RequestId, UserId, LicensePlate, GarageId FROM [dbo].Requests WHERE Validated IS NULL AND UserId = @UserId";

                // Execute query and bring information
                sqlCommand.ExecuteNonQuery();

                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);

                dataAdapter.Fill(dataTable);

                string json = JsonConvert.SerializeObject(dataTable);

                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<Requests>));

                var ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
                List<Requests> requests = (List<Requests>)jsonSerializer.ReadObject(ms);

                foreach (var request in requests)
                    requestsList.Add(new Requests(request.RequestId, request.UserId, request.LicensePlate, request.GarageId));

            }
            catch (SqlException)
            {

                // dispose object
                sqlCommand.Dispose();

                // close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSRequestsException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSRequestsException(ex.Message);

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
            return requestsList;

        }

        public List<string> GetalldRequestsUser(int userId)
        {

            // Variables
            SqlCommand sqlCommand;
            List<string> requestsIdList = new List<string>();
            DataTable dataTable = new DataTable();

            // Validations
            if (!IsConnectionOpen())
            {

                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.OpenConnection();
                else
                    throw new MSRequestsException(ExceptionsDetails.SQLSERVER_CONNECTIONS_NO_CONNECTION);

            }

            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;

            try
            {
                //Parameters
                sqlCommand.Parameters.AddWithValue("@UserId", userId);

                // Query
                sqlCommand.CommandText = @"SELECT RequestId FROM [dbo].Requests WHERE Validated = 1 AND UserId = @UserId";

                // Execute query and bring information
                sqlCommand.ExecuteNonQuery();

                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);

                dataAdapter.Fill(dataTable);

                string json = JsonConvert.SerializeObject(dataTable);

                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<Requests>));

                var ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
                List<Requests> requests = (List<Requests>)jsonSerializer.ReadObject(ms);

                foreach (var request in requests)
                    requestsIdList.Add(request.RequestId);

            }
            catch (SqlException)
            {

                // dispose object
                sqlCommand.Dispose();

                // close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSRequestsException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSRequestsException(ex.Message);

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
            return requestsIdList;

        }

    }

}

#endregion
