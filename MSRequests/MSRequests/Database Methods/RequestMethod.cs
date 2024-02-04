#region Usings

using MSRequests.Engine;
using MSRequests.Models;
using MSRequests.Exceptions;
using System.Data.SqlClient;
using RestSharp;
using System.Net;
using MSRequests.Configurations;
using System.Net.Mail;

#endregion

#region RequestMethod

namespace MSRequests.Database_Methods
{

    public class RequestMethod
    {

        protected MSRequestsEngine mSRequestsEngine;

        public RequestMethod(MSRequestsEngine mSRequestsEngine)
        {

            this.mSRequestsEngine = mSRequestsEngine;

        }


        public bool CreateRequest(int userId, string licensePlate, int garageid)
        {

            // Variables
            bool setNewConnection;

            // Flag that defines if a new connection must be settle to database
            setNewConnection = !this.mSRequestsEngine.SQLServer.IsConnectionOpen();


            try
            {

                // If a new connection is required, open it and initiate a new transaction
                if (setNewConnection)
                    this.mSRequestsEngine.SQLServer.OpenConnection();


                // Get number of affected records from database engine
                if (this.mSRequestsEngine.SQLServer.ExecuteCreateRequest(userId, licensePlate, garageid) == 1)
                {

                    // If a new connection was required, commit the transaction and close it
                    if (setNewConnection)
                        this.mSRequestsEngine.SQLServer.CloseConnection();

                    return true;

                }
                else
                {

                    // If a new connection was required, commit the transaction and close it
                    if (setNewConnection)
                        this.mSRequestsEngine.SQLServer.CloseConnection();

                    return false;

                }

            }
            catch (SqlException)
            {

                // Close connection
                if (this.mSRequestsEngine.SQLServer.IsConnectionOpen())
                    this.mSRequestsEngine.SQLServer.CloseConnection();

                throw new MSRequestsException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Close connection
                if (this.mSRequestsEngine.SQLServer.IsConnectionOpen())
                    this.mSRequestsEngine.SQLServer.CloseConnection();

                throw new MSRequestsException(ex.Message);

            }

        }


        public void SendEmail()
        {
            string emailTittle = "Validation Approved";
            string emaildestiny = "";
            string emailsend = "ProjetosIpcaMS@outlook.pt";
            string subject = "Validation Approved";
            string emailpass = "";

            MailMessage message = new MailMessage(new MailAddress(emailsend, emailTittle), new MailAddress(emaildestiny));
            message.Subject = subject;


            message.Body = "Your request was validated, you can bring your car to the garage, or contact us to come get it from you";
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



        public bool ValidateRequest(int userId, string requestId, bool validate, string token)
        {

            // Variables
            bool setNewConnection;

            // Flag that defines if a new connection must be settle to database
            setNewConnection = !this.mSRequestsEngine.SQLServer.IsConnectionOpen();


            try
            {

                // If a new connection is required, open it and initiate a new transaction
                if (setNewConnection)
                    this.mSRequestsEngine.SQLServer.OpenConnection();


                // Get number of affected records from database engine
                if (this.mSRequestsEngine.SQLServer.ExecuteValidateRequest(userId, requestId, validate, token) == 1)
                {

                    // If a new connection was required, commit the transaction and close it
                    if (setNewConnection)
                        this.mSRequestsEngine.SQLServer.CloseConnection();

                    return true;

                }
                else
                {

                    // If a new connection was required, commit the transaction and close it
                    if (setNewConnection)
                        this.mSRequestsEngine.SQLServer.CloseConnection();

                    return false;

                }

            }
            catch (SqlException)
            {

                // Close connection
                if (this.mSRequestsEngine.SQLServer.IsConnectionOpen())
                    this.mSRequestsEngine.SQLServer.CloseConnection();

                throw new MSRequestsException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Close connection
                if (this.mSRequestsEngine.SQLServer.IsConnectionOpen())
                    this.mSRequestsEngine.SQLServer.CloseConnection();

                throw new MSRequestsException(ex.Message);

            }

        }


        public List<Requests> GetUnvalidatedRequests()
        {

            // Variable
            List<Requests> requests;
            bool setNewConnection;

            // Flag that defines if a new connection must be settle to database
            setNewConnection = !this.mSRequestsEngine.SQLServer.IsConnectionOpen();

            try
            {

                // If a new connection is required, open it
                if (setNewConnection)
                    this.mSRequestsEngine.SQLServer.OpenConnection();

                // Get user infomations according to database engine
                return requests = this.mSRequestsEngine.SQLServer.ExecuteUnvalidatedRequests();

                // If a new connection is settled, close it
                if (setNewConnection)
                    this.mSRequestsEngine.SQLServer.CloseConnection();

            }
            catch (SqlException)
            {

                // Close connection
                if (this.mSRequestsEngine.SQLServer.IsConnectionOpen())
                    this.mSRequestsEngine.SQLServer.CloseConnection();

                throw new MSRequestsException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Close connection
                if (this.mSRequestsEngine.SQLServer.IsConnectionOpen())
                    this.mSRequestsEngine.SQLServer.CloseConnection();

                throw new MSRequestsException(ex.Message);

            }

        }



        public Requests GetRequestInformation(string requestId)
        {

            // Variable
            Requests request;
            bool setNewConnection;

            // Flag that defines if a new connection must be settle to database
            setNewConnection = !this.mSRequestsEngine.SQLServer.IsConnectionOpen();

            try
            {

                // If a new connection is required, open it
                if (setNewConnection)
                    this.mSRequestsEngine.SQLServer.OpenConnection();

                // Get user infomations according to database engine
                return request = this.mSRequestsEngine.SQLServer.ExecuteRequestInformation(requestId);

                // If a new connection is settled, close it
                if (setNewConnection)
                    this.mSRequestsEngine.SQLServer.CloseConnection();

            }
            catch (SqlException)
            {

                // Close connection
                if (this.mSRequestsEngine.SQLServer.IsConnectionOpen())
                    this.mSRequestsEngine.SQLServer.CloseConnection();

                throw new MSRequestsException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Close connection
                if (this.mSRequestsEngine.SQLServer.IsConnectionOpen())
                    this.mSRequestsEngine.SQLServer.CloseConnection();

                throw new MSRequestsException(ex.Message);

            }

        }


        public List<Requests> GetRequestsUser(int userId)
        {

            // Variable
            List<Requests> requests;
            bool setNewConnection;

            // Flag that defines if a new connection must be settle to database
            setNewConnection = !this.mSRequestsEngine.SQLServer.IsConnectionOpen();

            try
            {

                // If a new connection is required, open it
                if (setNewConnection)
                    this.mSRequestsEngine.SQLServer.OpenConnection();

                // Get user infomations according to database engine
                return requests = this.mSRequestsEngine.SQLServer.GetRequestsUser(userId);

                // If a new connection is settled, close it
                if (setNewConnection)
                    this.mSRequestsEngine.SQLServer.CloseConnection();

            }
            catch (SqlException)
            {

                // Close connection
                if (this.mSRequestsEngine.SQLServer.IsConnectionOpen())
                    this.mSRequestsEngine.SQLServer.CloseConnection();

                throw new MSRequestsException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Close connection
                if (this.mSRequestsEngine.SQLServer.IsConnectionOpen())
                    this.mSRequestsEngine.SQLServer.CloseConnection();

                throw new MSRequestsException(ex.Message);

            }

        }


        public List<string> GetalldRequestsUser(int userId)
        {

            // Variable
            List<string> requests;
            bool setNewConnection;

            // Flag that defines if a new connection must be settle to database
            setNewConnection = !this.mSRequestsEngine.SQLServer.IsConnectionOpen();

            try
            {

                // If a new connection is required, open it
                if (setNewConnection)
                    this.mSRequestsEngine.SQLServer.OpenConnection();

                // Get user infomations according to database engine
                return requests = this.mSRequestsEngine.SQLServer.GetalldRequestsUser(userId);

                // If a new connection is settled, close it
                if (setNewConnection)
                    this.mSRequestsEngine.SQLServer.CloseConnection();

            }
            catch (SqlException)
            {

                // Close connection
                if (this.mSRequestsEngine.SQLServer.IsConnectionOpen())
                    this.mSRequestsEngine.SQLServer.CloseConnection();

                throw new MSRequestsException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Close connection
                if (this.mSRequestsEngine.SQLServer.IsConnectionOpen())
                    this.mSRequestsEngine.SQLServer.CloseConnection();

                throw new MSRequestsException(ex.Message);

            }

        }





        //public void CreateReparation(int managerId, string requestId)
        //{




        //    string url = "http://host.docker.internal:8001/api/Repair";

        //    var client = new RestClient(url);
        //    var request = new RestRequest(url, Method.Post);

        //    request.AddHeader("managerId", managerId);
        //    request.AddHeader("requestId", requestId);

        //    RestResponse response = client.Execute(request);

        //    if (response.StatusCode != HttpStatusCode.OK)
        //        throw new MSRequestsException(ExceptionsDetails.SOMETHING_WENT_WRONG_WHILE_CREATING_REPARATION);

        //}


    }

}

#endregion
