#region Usings

using System.Text;
using System.Data;
using Newtonsoft.Json;
using MSRepairs.Exceptions;
using System.Data.SqlClient;
using MSRepairs.Configurations;
using System.Runtime.Serialization.Json;
using MSRepairs.Models;
using RestSharp;
using System.Net;
using System.Net.Mail;

#endregion

namespace MSRepairs.SQL
{
    public partial class SQLServer
    {


        public int ExecuteCreateReparation(int managerId, string requestId, int garageid, int userId)
        {

            // Variables
            Int32 affectedLines;
            SqlCommand sqlCommand;

            // Validations
            if (!IsConnectionOpen())
            {

                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.OpenConnection();
                else
                    throw new MSRepairsException(ExceptionsDetails.SQLSERVER_CONNECTIONS_NO_CONNECTION);

            }

            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;

            try
            {

                // Parameters
                sqlCommand.Parameters.AddWithValue("@ManagerUserId", managerId);
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                sqlCommand.Parameters.AddWithValue("@RequestId", requestId);
                sqlCommand.Parameters.AddWithValue("@GarageId", garageid);
                // Query
                sqlCommand.CommandText = @"INSERT INTO[dbo].Repairs(RepairId, RequestId, ManagerUserId, GarageId, UserId, StateId, ServiceTypeId, RepairCreatedDateTime, RepairDoneDatime, WorkTime, Notes) VALUES(NEWID(), @RequestId, @ManagerUserId, @GarageId, @UserId,'NTCMPL', NULL, GETDATE(), NULL, 0, NULL)";


                // Execute query and keep number of affectedLines
                affectedLines = (Int32)sqlCommand.ExecuteNonQuery();

            }
            catch (SqlException)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSRepairsException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSRepairsException(ex.Message);

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


        public List<Repair> ExecuteCurrentReparations()
        {

            // Variables
            SqlCommand sqlCommand;        
            DataTable dataTable = new DataTable();
            List<Repair> repairList = new List<Repair>();

            // Validations
            if (!IsConnectionOpen())
            {

                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.OpenConnection();
                else
                    throw new MSRepairsException(ExceptionsDetails.SQLSERVER_CONNECTIONS_NO_CONNECTION);

            }

            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;

            try
            {

                // Query
                sqlCommand.CommandText = @"SELECT RepairId, RequestId, ManagerUserId, GarageId, UserId, ServiceTypeId, WorkTime, Notes FROM [dbo].Repairs WHERE StateId = 'INPROG' OR StateId = 'NTCMPL'";

                // Execute query and bring information
                sqlCommand.ExecuteNonQuery();

                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);

                dataAdapter.Fill(dataTable);

                string json = JsonConvert.SerializeObject(dataTable);

                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<Repair>));

                var ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
                List<Repair> h = (List<Repair>)jsonSerializer.ReadObject(ms);

                foreach (var repair in h)
                    repairList.Add(new Repair(repair.RepairId, repair.RequestId, repair.ManagerUserId, repair.GarageId, repair.UserId, repair.ServiceTypeId, repair.WorkTime, repair.Notes));

            }
            catch (SqlException)
            {

                // dispose object
                sqlCommand.Dispose();

                // close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSRepairsException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSRepairsException(ex.Message);

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
            return repairList;

        }


        public List<Repair> ExecuteCurrentReparationsUser(int userId)
        {

            // Variables
            SqlCommand sqlCommand;
            DataTable dataTable = new DataTable();
            List<Repair> repairList = new List<Repair>();
            List<int> UserIdList = new List<int>();

            // Validations
            if (!IsConnectionOpen())
            {

                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.OpenConnection();
                else
                    throw new MSRepairsException(ExceptionsDetails.SQLSERVER_CONNECTIONS_NO_CONNECTION);

            }

            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;

            try
            {


                //Parameters
                sqlCommand.Parameters.AddWithValue("@UserId", userId);

                // Query
                sqlCommand.CommandText = @"SELECT RepairId, RequestId, GarageId FROM [dbo].Repairs WHERE UserId = @UserId AND StateId = 'INPROG' OR StateId = 'NTCMPL'";

                // Execute query and bring information
                sqlCommand.ExecuteNonQuery();

                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);

                dataAdapter.Fill(dataTable);

                string json = JsonConvert.SerializeObject(dataTable);

                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<Repair>));

                var ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
                List<Repair> h = (List<Repair>)jsonSerializer.ReadObject(ms);

                foreach (var repair in h)
                    repairList.Add(new Repair(repair.RepairId, repair.RequestId, repair.ManagerUserId, repair.GarageId, repair.UserId, repair.ServiceTypeId, repair.WorkTime, repair.Notes));

            }
            catch (SqlException)
            {

                // dispose object
                sqlCommand.Dispose();

                // close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSRepairsException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSRepairsException(ex.Message);

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
            return repairList;

        }

        public List<Repair> UserHistoryRepairs(int userId)
        {

            // Variables
            SqlCommand sqlCommand;
            DataTable dataTable = new DataTable();
            List<Repair> repairList = new List<Repair>();
            List<int> UserIdList = new List<int>();

            // Validations
            if (!IsConnectionOpen())
            {

                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.OpenConnection();
                else
                    throw new MSRepairsException(ExceptionsDetails.SQLSERVER_CONNECTIONS_NO_CONNECTION);

            }

            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;

            try
            {


                //Parameters
                sqlCommand.Parameters.AddWithValue("@UserId", userId);

                // Query
                sqlCommand.CommandText = @"SELECT RepairId, RequestId, GarageId FROM [dbo].Repairs WHERE UserId = @UserId AND StateId = 'CMPLET'";

                // Execute query and bring information
                sqlCommand.ExecuteNonQuery();

                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);

                dataAdapter.Fill(dataTable);

                string json = JsonConvert.SerializeObject(dataTable);

                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<Repair>));

                var ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
                List<Repair> h = (List<Repair>)jsonSerializer.ReadObject(ms);

                foreach (var repair in h)
                    repairList.Add(new Repair(repair.RepairId, repair.RequestId, repair.ManagerUserId, repair.GarageId, repair.UserId, repair.ServiceTypeId, repair.WorkTime, repair.Notes));

            }
            catch (SqlException)
            {

                // dispose object
                sqlCommand.Dispose();

                // close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSRepairsException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSRepairsException(ex.Message);

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
            return repairList;

        }






        public int ExecuteReparationState(string repairId, string stateId, string serviceTypeId, int workTime, string notes, int userId)
        {

            // Variables
            Int32 affectedLines;
            SqlCommand sqlCommand;
            double repairPrice = 0;
            string requestId = "";
            int employeeId;

            // Validations
            if (!IsConnectionOpen())
            {

                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.OpenConnection();
                else
                    throw new MSRepairsException(ExceptionsDetails.SQLSERVER_CONNECTIONS_NO_CONNECTION);

            }

            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;

            try
            {

                if (workTime < 0)
                    workTime = 0;




                // Parameters
                sqlCommand.Parameters.AddWithValue("@RepairId", repairId);
                sqlCommand.Parameters.AddWithValue("@StateId", stateId);
                sqlCommand.Parameters.AddWithValue("@ServiceTypeId", serviceTypeId);
                sqlCommand.Parameters.AddWithValue("@WorkTime", workTime);
                sqlCommand.Parameters.AddWithValue("@Notes", notes);
                sqlCommand.Parameters.AddWithValue("@User", userId);

                sqlCommand.CommandText = @"SELECT ManagerUserId FROM dbo.[Repairs] where RepairId = @RepairId";

                employeeId = (Int32)sqlCommand.ExecuteScalar();

                if (employeeId != userId)
                    throw new MSRepairsException(ExceptionsDetails.INVALID_PERMISSION);



                // Query
                sqlCommand.CommandText = @"UPDATE [dbo].Repairs SET StateId = @StateId, ServiceTypeId = @ServiceTypeId, WorkTime =  (SELECT MAX(WorkTime) from [dbo].Repairs r WHERE RepairId = @RepairId) + @WorkTime, Notes = @Notes  WHERE RepairId = @RepairId;";

                // Execute query and keep number of affectedLines
                affectedLines = (Int32)sqlCommand.ExecuteNonQuery();

                if(stateId == "CMPLET")
                {
                    
                    sqlCommand.CommandText = @"SELECT SUM(p.Quantity * s.Price + (p.Quantity * s.Price * 0.23) + (r.Worktime * 15)) Price FROM [dbo].Repairs r INNER JOIN [dbo].RepairsParts rp ON r.RepairId = rp.RepairId INNER JOIN [dbo].Parts p ON rp.PartsId = p.PartsId INNER JOIN [dbo].Stock s ON p.StockId = s.StockId WHERE rp.RepairId = @RepairId";
                
                    SqlDataReader executePrice = sqlCommand.ExecuteReader();

                    while (executePrice.Read())
                    {
                        repairPrice = Math.Round(double.Parse(executePrice["Price"].ToString()), 0);

                    }
                    executePrice.Close();


                    sqlCommand.CommandText = @"SELECT RequestId FROM [dbo].Repairs WHERE RepairId = @RepairId;";

                    SqlDataReader executeRequestId = sqlCommand.ExecuteReader();

                    while (executeRequestId.Read())
                    {
                        requestId = executeRequestId["RequestId"].ToString();
                    }
                    executeRequestId.Close();



                    string urlRequestId = "http://host.docker.internal:7171/api/Request/RequestInfo";

                    var clientRequestId = new RestClient(urlRequestId);
                    var requestRequestId = new RestRequest(urlRequestId, Method.Get);

                    requestRequestId.AddHeader("RequestId", requestId);

                    RestResponse response = clientRequestId.Execute(requestRequestId);

                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new MSRepairsException(ExceptionsDetails.SOMETHING_WENT_WRONG);

                    Requests requestResponse = JsonConvert.DeserializeObject<Requests>(response.Content);



                    string urlPayments = "http://host.docker.internal:7118/api/Payments";

                    var clientPayments = new RestClient(urlPayments);
                    var requestPayments = new RestRequest(urlPayments, Method.Post);

                    requestPayments.AddHeader("UserId", requestResponse.UserId);
                    requestPayments.AddHeader("RepairId", repairId);
                    requestPayments.AddHeader("LicensePlate", requestResponse.LicensePlate);
                    requestPayments.AddHeader("ServiceTypeId", serviceTypeId);
                    requestPayments.AddHeader("Price", repairPrice);

                    RestResponse responsePayments = clientPayments.Execute(requestPayments);

                    SendEmail(requestResponse.LicensePlate,repairPrice);



                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new MSRepairsException(ExceptionsDetails.SOMETHING_WENT_WRONG);

                }

            }
            catch (SqlException)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSRepairsException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSRepairsException(ex.Message);

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


        public void SendEmail(string licenseplate, double price)
        {
            string emailTittle = "Reparation Done";
            string emaildestiny = "ProjetosIpcaMS@outlook.pt";
            string emailsend = "ProjetosIpcaMS@outlook.pt";
            string subject = "Reparation Done";
            string emailpass = "";

            MailMessage message = new MailMessage(new MailAddress(emailsend, emailTittle), new MailAddress(emaildestiny));
            message.Subject = subject;


            message.Body = "Your reparation to the car with the license plate " + licenseplate  + " is now finished, the total of the reparation is " + price + "." + "\nPlease confirm the payment and the come to the the garage pickup the car ";
            message.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            //465 (SSL) 587 (TLS)
            smtp.Host = "smtp.office365.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;


            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential();
            credentials.UserName = emailsend;
            credentials.Password = emailpass;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = credentials;

            smtp.Send(message);
        }




        public int ExecuteRemoveStock(string stockId, int quantity, int garageId)
        {

            // Variables
            Int32 affectedLines;
            SqlCommand sqlCommand;

            // Validations
            if (!IsConnectionOpen())
            {

                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.OpenConnection();
                else
                    throw new MSRepairsException(ExceptionsDetails.SQLSERVER_CONNECTIONS_NO_CONNECTION);

            }

            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;

            try
            {

                // Parameters
                sqlCommand.Parameters.AddWithValue("@StockId", stockId);
                sqlCommand.Parameters.AddWithValue("@Quatity", quantity);
                sqlCommand.Parameters.AddWithValue("@GarageId", garageId);

                // Query
                sqlCommand.CommandText = @"UPDATE [dbo].Stock SET Quantity = ((SELECT MAX(Quantity) from [dbo].Stock r WHERE StockId = @StockId) - @Quatity) WHERE StockId = @StockId AND GarageId = @GarageId;";

                // Execute query and keep number of affectedLines
                affectedLines = (Int32)sqlCommand.ExecuteNonQuery();

            }
            catch (SqlException)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSRepairsException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSRepairsException(ex.Message);

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


        public List<Stock> ExecuteGetStock(int garageId)
        {

            // Variables
            SqlCommand sqlCommand;
            DataTable dataTable = new DataTable();
            List<Stock> stockList = new List<Stock>();

            // Validations
            if (!IsConnectionOpen())
            {

                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.OpenConnection();
                else
                    throw new MSRepairsException(ExceptionsDetails.SQLSERVER_CONNECTIONS_NO_CONNECTION);

            }

            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;

            try
            {
                sqlCommand.Parameters.AddWithValue("@GarageId", garageId);
                // Query
                sqlCommand.CommandText = @"SELECT * FROM [dbo].Stock where GarageId = @GarageId";

                // Execute query and bring information
                sqlCommand.ExecuteNonQuery();

                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);

                dataAdapter.Fill(dataTable);

                string json = JsonConvert.SerializeObject(dataTable);

                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<Stock>));

                var ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
                List<Stock> h = (List<Stock>)jsonSerializer.ReadObject(ms);

                foreach (var stock in h)
                    stockList.Add(new Stock(stock.StockId, stock.PartName, stock.Quantity, stock.Price));

            }
            catch (SqlException)
            {

                // dispose object
                sqlCommand.Dispose();

                // close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSRepairsException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSRepairsException(ex.Message);

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
            return stockList;

        }


        public int ExecuteAddPart(string token, string repairId, string stockId, int quantity, int userId)
        {

            Int32 affectedLines;
            SqlCommand sqlCommand;
            int garageId;
            int employeeId;

            // Validations
            if (!IsConnectionOpen())
            {

                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.OpenConnection();
                else
                    throw new MSRepairsException(ExceptionsDetails.SQLSERVER_CONNECTIONS_NO_CONNECTION);

            }





            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;



            sqlCommand.Parameters.AddWithValue("@RepairId", repairId);
            sqlCommand.Parameters.AddWithValue("@User", userId);

            sqlCommand.CommandText = @"SELECT ManagerUserId FROM dbo.[Repairs] where RepairId = @RepairId";

            employeeId = (Int32)sqlCommand.ExecuteScalar();

            if (employeeId != userId)
                throw new MSRepairsException(ExceptionsDetails.INVALID_PERMISSION);


            sqlCommand.CommandText = @"SELECT GarageId from dbo.[Repairs] where RepairId = @RepairId";

            garageId = (Int32)sqlCommand.ExecuteScalar();

            string url = "http://host.docker.internal:7159/api/Stock/RemoveStock";

            var client = new RestClient(url);
            var request = new RestRequest(url, Method.Put);

            request.AddHeader("token", token);
            request.AddHeader("stockId", stockId);
            request.AddHeader("quantity", quantity);
            request.AddHeader("garageid", garageId);

            RestResponse response = client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new MSRepairsException(ExceptionsDetails.SOMETHING_WENT_WRONG_WHILE_REMOVING_PART);

            // Variables
            //Int32 affectedLines;
            //SqlCommand sqlCommand;

            //// Validations
            //if (!IsConnectionOpen())
            //{

            //    if (openCloseConnectionMode == SQLConnectionModes.auto)
            //        this.OpenConnection();
            //    else
            //        throw new MSRepairsException(ExceptionsDetails.SQLSERVER_CONNECTIONS_NO_CONNECTION);

            //}

            //sqlCommand = new SqlCommand();
            //sqlCommand.Connection = sqlConnection;

            try
            {

                // Parameters
                //sqlCommand.Parameters.AddWithValue("@RepairId", repairId);
                sqlCommand.Parameters.AddWithValue("@StockId", stockId);
                sqlCommand.Parameters.AddWithValue("@Quantity", quantity);

                // Query
                sqlCommand.CommandText = @"DECLARE @newPartsId uniqueidentifier; SET @newPartsId = NEWID(); INSERT INTO [dbo].Parts (PartsId, StockId, Quantity, Price) VALUES (@newPartsId, @StockId, @Quantity, (SELECT Price FROM [dbo].Stock WHERE StockId = @StockId) * @Quantity); INSERT INTO [dbo].RepairsParts (RepairId, PartsId) VALUES (@RepairId, @newPartsId);";

                // Execute query and keep number of affectedLines
                affectedLines = (Int32)sqlCommand.ExecuteNonQuery();

                // Revert if Error
                if(affectedLines != 2)
                {

                    client = new RestClient(url);
                    request = new RestRequest(url, Method.Put);

                    request.AddHeader("token", token);
                    request.AddHeader("stockId", stockId);
                    request.AddHeader("quantity", -quantity);

                    response = client.Execute(request);

                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new MSRepairsException(ExceptionsDetails.SOMETHING_WENT_WRONG_WHILE_ADDING_PART);
                }

            }
            catch (SqlException)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSRepairsException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSRepairsException(ex.Message);

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


    }

}
