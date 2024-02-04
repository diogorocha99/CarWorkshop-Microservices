#region Usings

using System.Text;
using System.Data;
using MSGarages.Models;
using MSGarages.Exceptions;
using MSGarages.Configurations;
using System.Runtime.Serialization.Json;
using System.Net;
using System.Data.SqlClient;
using RestSharp;
using System.Reflection.PortableExecutable;
using Newtonsoft.Json;
using Azure.Core;

#endregion

namespace MSGarages.SQL
{
    public partial class SQLServer
    {

        public int HasGarage(string adress, string name)
        {

            // Variables
            Int32 hasEmail;
            SqlCommand sqlCommand;

            // Validations
            if (!IsConnectionOpen())
            {

                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.OpenConnection();
                else
                    throw new MSGaragesException(ExceptionsDetails.SQLSERVER_CONNECTIONS_NO_CONNECTION);

            }

            // Prepare execution
            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;

            try
            {

                // Parameters
                sqlCommand.Parameters.AddWithValue("@Address", adress);
                sqlCommand.Parameters.AddWithValue("@Name", name);

                // Query
                sqlCommand.CommandText = @"SELECT COUNT(u.[GarageId]) FROM [dbo].Garage u WITH(NOLOCK) WHERE u.[Address] = @Address and u.[Name] = @Name";

                // Execute query and keep the returned object
                hasEmail = (Int32)sqlCommand.ExecuteScalar();

            }
            catch (SqlException)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSGaragesException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSGaragesException(ExceptionsDetails.OTHERS_EXCEPTION_PREFIX_ERROR);

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
            return hasEmail;

        }





        public int ExecuteCreateGarage(int userId, string adress, string name, string postal_code)
        {

            // Variables
            int affectedLines;
            SqlCommand sqlCommand;

            // Validations
            if (!IsConnectionOpen())
            {

                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.OpenConnection();
                else
                    throw new MSGaragesException(ExceptionsDetails.SQLSERVER_CONNECTIONS_NO_CONNECTION);

            }

            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;

            try
            {

                // Parameters
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                sqlCommand.Parameters.AddWithValue("@Address", adress);
                sqlCommand.Parameters.AddWithValue("@Name", name);
                sqlCommand.Parameters.AddWithValue("@Postal_Code", postal_code);

                // Query
                sqlCommand.CommandText = @"INSERT INTO[dbo].Garage(GarageId, Address, Name, Postal_Code) VALUES((SELECT MAX(GarageId) FROM dbo.[Garage]) + 1, @Address, @Name, @Postal_Code);";

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

                throw new MSGaragesException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSGaragesException(ex.Message);

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


        public int ExecuteAddEmployee(string email, int garageid)
        {
            //Int32 updateResult;
            string userIdstring;
            int userId;
            string roletrim;
            string userIdformated;
            string roleformated;
            string role;
            Int32 hasdata = 0;

            string url = "http://host.docker.internal:7023/api/User/RolebyEmail";

            var client = new RestClient(url);
            var request = new RestRequest(url, Method.Get);

            request.AddHeader("email", email);

            var response = client.Execute(request);

            var responseformated = response.Content;

            //string[] tokens = responseformated.ToString().Split(';');

            //role = tokens[1];

            //userId = Int32.Parse(tokens[0]);

            userIdstring = responseformated.ToString().Split(";")[0];

            role = responseformated.ToString().Split(";")[1];

            userIdformated = userIdstring.Replace('"',' ');

            roleformated = role.Replace('"', ' ');



            userId = Convert.ToInt32(userIdformated);

            if(garageid > 3)
            {
                throw new MSGaragesException(ExceptionsDetails.GARAGE_DOES_NOT_EXIST);
            }


            if (roleformated != "MANGR ")
                throw new MSGaragesException(ExceptionsDetails.USER_IS_NOT_A_EMPLOYEE);

            // Variables
            int affectedLines = 0;
            SqlCommand sqlCommand;

            // Validations
            if (!IsConnectionOpen())
            {

                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.OpenConnection();
                else
                    throw new MSGaragesException(ExceptionsDetails.SQLSERVER_CONNECTIONS_NO_CONNECTION);

            }

            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;

            try
            {

                // Parameters
                sqlCommand.Parameters.AddWithValue("@GarageId", garageid);
                sqlCommand.Parameters.AddWithValue("@Email", email);
                sqlCommand.Parameters.AddWithValue("@UserId", userId);

                sqlCommand.CommandText = @"SELECT u.[GarageId], u.[UserId] FROM[dbo].Garage_User u WHERE u.[UserId] = @UserId";

                //hasdata = (Int32)sqlCommand.ExecuteScalar();


                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        reader.Close();
                        throw new MSGaragesException(ExceptionsDetails.EMPLOYEE_ALREADY_WORKING);
                    }
                    else
                    {
                        reader.Close();
                        sqlCommand.CommandText = @"INSERT INTO[dbo].Garage_User(GarageId, UserId) VALUES(@GarageId, @UserId)";

                        affectedLines = sqlCommand.ExecuteNonQuery();
                    }

                }
                

                //SqlDataReader reader = sqlCommand.ExecuteReader();
                //if (hasdata != 0)
                //{
                //    sqlCommand.CommandText = @"INSERT INTO[dbo].Garage_User(GarageId, UserId) VALUES(@GarageId, @UserId)";

                //    affectedLines = sqlCommand.ExecuteNonQuery();
                //}
                //else
                //{
                //    throw new MSGaragesException(ExceptionsDetails.EMPLOYEE_ALREADY_WORKING);
                //}


            }
            catch (SqlException)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSGaragesException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSGaragesException(ex.Message);

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



        public int EmployeeGarage(int userId)
        {

            // Variables
            Int32 garageid;
            SqlCommand sqlCommand;

            // Validations
            if (!IsConnectionOpen())
            {

                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.OpenConnection();
                else
                    throw new MSGaragesException(ExceptionsDetails.SQLSERVER_CONNECTIONS_NO_CONNECTION);

            }

            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;

            try
            {

                // Parameters
                sqlCommand.Parameters.AddWithValue("@UserId", userId);

                // Query
                sqlCommand.CommandText = @"SELECT GarageId FROM [dbo].Garage_User where UserId= @UserId";

                // Execute query and keep number of affectedLines

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Close();
                        garageid = (Int32)sqlCommand.ExecuteScalar();
                    }
                    else
                    {
                        throw new MSGaragesException(ExceptionsDetails.EMPLOYEE_NOT_WORKING);
                    }
                    reader.Close();
                }
            }
            catch (SqlException)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSGaragesException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSGaragesException(ex.Message);

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
            return garageid;

        }


        //public List<User> ExecuteCurrentReparations(int garageId)
        //{

        //    // Variables
        //    SqlCommand sqlCommand;
        //    DataTable dataTable = new DataTable();
        //    List<User> garageuser = new List<User>();
        //    int[] userId;
        //    int i = 0;

        //    // Validations
        //    if (!IsConnectionOpen())
        //    {

        //        if (openCloseConnectionMode == SQLConnectionModes.auto)
        //            this.OpenConnection();
        //        else
        //            throw new MSGaragesException(ExceptionsDetails.SQLSERVER_CONNECTIONS_NO_CONNECTION);

        //    }

        //    sqlCommand = new SqlCommand();
        //    sqlCommand.Connection = sqlConnection;

        //    try
        //    {
        //        sqlCommand.Parameters.AddWithValue("@GarageId", garageId);
        //        // Query
        //        sqlCommand.CommandText = @"SELECT u.UserId FROM dbo.Garage_User u where GarageId = @GarageId'";

        //        // Execute query and bring information
        //        sqlCommand.ExecuteNonQuery();

        //        SqlDataReader executeIds = sqlCommand.ExecuteReader();

        //        while (executeIds.Read())
        //        {
        //            userId = (int[])executeIds["u.UserId"];
        //        }
        //        executeIds.Close();


        //        for (i = 0; i <= userId.Length; i++)
        //        {
        //            sqlCommand.Parameters.AddWithValue("@UserId", userId[i]);
        //            sqlCommand.CommandText = "SELECT UserId, Email, Name, Password "
        //        } 


        //        sqlCommand.Parameters.AddWithValue("@UserId", userId);

        //        dataAdapter.Fill(dataTable);

        //        string json = JsonConvert.SerializeObject(dataTable);

        //        DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<User>));

        //        var ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
        //        List<User> h = (List<User>)jsonSerializer.ReadObject(ms);

        //        foreach (var user in h)
        //            garageuser.Add(new User(user.UserId, user.Email, user.Name, user.PhoneNumber));

        //    }
        //    catch (SqlException)
        //    {

        //        // dispose object
        //        sqlCommand.Dispose();

        //        // close connection if using auto connection mode
        //        if (openCloseConnectionMode == SQLConnectionModes.auto)
        //            this.CloseConnection();

        //        throw new MSGaragesException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

        //    }
        //    catch (Exception ex)
        //    {

        //        // Dispose object
        //        sqlCommand.Dispose();

        //        // Close connection if using auto connection mode
        //        if (openCloseConnectionMode == SQLConnectionModes.auto)
        //            this.CloseConnection();

        //        throw new MSGaragesException(ex.Message);

        //    }
        //    finally
        //    {

        //        // Dispose object
        //        sqlCommand.Dispose();

        //        // Close connection if using auto connection mode
        //        if (openCloseConnectionMode == SQLConnectionModes.auto)
        //            this.CloseConnection();

        //    }

        //    // Return object returned by sql engine
        //    return garageuser;

        //}


    }
}
