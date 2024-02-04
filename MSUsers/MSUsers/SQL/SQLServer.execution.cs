#region Usings

using System.Text;
using System.Data;
using MSUsers.Models;
using Newtonsoft.Json;
using MSUsers.Exceptions;
using System.Data.SqlClient;
using MSUsers.Configurations;
using System.Runtime.Serialization.Json;

#endregion

#region SQLServer Execution

namespace MSUsers.SQL
{

    public partial class SQLServer
    {

        public int ExecuteHasAccount(string email, string password)
        {

            // Variables
            SqlCommand sqlCommand;
            Int32 credentialsCorrect = 0;

            // Validations
            if (!IsConnectionOpen())
            {

                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.OpenConnection();
                else
                    throw new MSUsersException(ExceptionsDetails.SQLSERVER_CONNECTIONS_NO_CONNECTION);

            }

            // Prepare execution
            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;

            try
            {

                // Parameters
                sqlCommand.Parameters.AddWithValue("@Email", email);
                sqlCommand.Parameters.AddWithValue("@Password", Configs.Encrypt(password));

                // Query
                sqlCommand.CommandText = @"SELECT COUNT(u.[UserId]) FROM [dbo].Users u WITH(NOLOCK) WHERE u.[Email] = @Email AND u.[Inactive] = 'False'";

                // Execute query and keep the returned object
                Int32 emailExists = (Int32)sqlCommand.ExecuteScalar();


                if(emailExists == 1)
                {

                    sqlCommand.CommandText = @"SELECT COUNT(u.[UserId]) FROM [dbo].Users u WITH(NOLOCK) WHERE u.[Email] = @Email AND u.[Password] = @Password AND u.[Inactive] = 'False'";

                    credentialsCorrect = (Int32)sqlCommand.ExecuteScalar();

                    if (credentialsCorrect == 1) {

                        sqlCommand.CommandText = @"SELECT [AuthenticationAvailableTries] FROM [dbo].Users u WHERE u.[Email] = @Email";

                        Int32 availableTries = (Int32)sqlCommand.ExecuteScalar();

                        if(availableTries != 0)
                        {

                            if(availableTries == 3) {

                                sqlCommand.CommandText = @"UPDATE [dbo].Users SET [AuthenticationAvailableTries] = 0, [AuthenticationLastFail] = NULL WHERE [AuthenticationLastFail] < DATEADD(HOUR, -1, GETDATE()) AND [AuthenticationAvailableTries] = 3 AND [Email] = @Email";
                                
                                Int32 availableTriesReseted = (Int32)sqlCommand.ExecuteNonQuery();

                                if (availableTriesReseted != 1)
                                    throw new MSUsersException(ExceptionsDetails.LOGIN_IS_CURRENTLY_DISABLED_DUE_TRIES);

                            }
                            else
                            {

                                sqlCommand.CommandText = @"UPDATE [dbo].Users SET [AuthenticationAvailableTries] = 0, [AuthenticationLastFail] = NULL WHERE [Email] = @Email";
                                
                                sqlCommand.ExecuteNonQuery();
                            
                            }

                        }

                    }

                }

            }
            catch (SqlException)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSUsersException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSUsersException(ex.Message);

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
            return credentialsCorrect;

        }


        public void ExecuteAddLoginAttemps(string email)
        {

            // Variables
            Int32 emailExists;
            Int32 updateResult;
            SqlCommand sqlCommand;

            // Validations
            if (!IsConnectionOpen())
            {

                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.OpenConnection();
                else
                    throw new MSUsersException(ExceptionsDetails.SQLSERVER_CONNECTIONS_NO_CONNECTION);

            }

            // Prepare execution
            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;

            try
            {

                // Parameters
                sqlCommand.Parameters.AddWithValue("@Email", email);


                sqlCommand.CommandText = @"SELECT COUNT(u.[UserId]) FROM[dbo].Users u WHERE u.[Email] = @Email AND u.[Inactive] = 'False' AND u.[AuthenticationAvailableTries] = 3 AND[AuthenticationLastFail] < DATEADD(HOUR, -1, GETDATE());";

                Int32 failedAttempsTimeout = (Int32)sqlCommand.ExecuteScalar();

                if (failedAttempsTimeout == 1)
                {

                    sqlCommand.CommandText = @"UPDATE [dbo].Users SET [AuthenticationAvailableTries] = 0, [AuthenticationLastFail] = NULL WHERE [AuthenticationLastFail] < DATEADD(HOUR, -1, GETDATE()) AND [AuthenticationAvailableTries] = 3 AND [Email] = @Email";

                    sqlCommand.ExecuteNonQuery();

                }

                // Query
                sqlCommand.CommandText = @"SELECT COUNT(u.[UserId]) FROM [dbo].Users u WITH(NOLOCK) WHERE u.[Email] = @Email AND u.[Inactive] = 'False'";

                // Execute query and keep the returned object
                emailExists = (Int32)sqlCommand.ExecuteScalar();

                if (emailExists == 1)
                {

                    sqlCommand.CommandText = @"UPDATE [dbo].Users SET [AuthenticationAvailableTries] = [AuthenticationAvailableTries] + 1, [AuthenticationLastFail] = GETDATE() WHERE [Email] = @Email AND [AuthenticationAvailableTries] < 3";

                    // Execute query and keep the returned object
                    updateResult = (Int32)sqlCommand.ExecuteNonQuery();

                    if (updateResult == 0)
                        throw new MSUsersException(ExceptionsDetails.LOGIN_IS_CURRENTLY_DISABLED_DUE_TRIES);

                }

            }
            catch (SqlException)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSUsersException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSUsersException(ex.Message);

            }
            finally
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

            }

        }


        public int ExecuteCreateAccount(User user)
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
                    throw new MSUsersException(ExceptionsDetails.SQLSERVER_CONNECTIONS_NO_CONNECTION);

            }

            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;

            try
            {

                // Parameters
                sqlCommand.Parameters.AddWithValue("@Email", user.Email);
                sqlCommand.Parameters.AddWithValue("@Password", Configs.Encrypt(user.Password));
                sqlCommand.Parameters.AddWithValue("@Name", user.Name);
                sqlCommand.Parameters.AddWithValue("@Address", user.Address);
                sqlCommand.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                sqlCommand.Parameters.AddWithValue("@AuthenticationAvailableTries", user.AuthenticationAvailableTries);
                sqlCommand.Parameters.AddWithValue("@Inactive", false);
                sqlCommand.Parameters.AddWithValue("@CreationDate", DateTime.Now);

                // Query
                sqlCommand.CommandText = @"INSERT INTO [dbo].Users (UserId, Email, Password, Name, Address, PhoneNumber, AuthenticationAvailableTries, AuthenticationLastFail, Inactive, CreationDate, RoleId) VALUES ((SELECT MAX(UserId) FROM dbo.[Users]) + 1, @Email, @Password, @Name, @Address, @PhoneNumber, @AuthenticationAvailableTries, NULL, @Inactive, @CreationDate, cast('b6149a90-7994-44c1-b832-113cda923ee6' AS uniqueidentifier))";

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

                throw new MSUsersException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSUsersException(ExceptionsDetails.OTHERS_EXCEPTION_PREFIX_ERROR);

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


        public int ExecuteHasEmail(string email)
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
                    throw new MSUsersException(ExceptionsDetails.SQLSERVER_CONNECTIONS_NO_CONNECTION);

            }

            // Prepare execution
            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;

            try
            {

                // Parameters
                sqlCommand.Parameters.AddWithValue("@Email", email);

                // Query
                sqlCommand.CommandText = @"SELECT COUNT(u.[UserId]) FROM [dbo].Users u WITH(NOLOCK) WHERE u.[Email] = @Email";

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

                throw new MSUsersException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSUsersException(ExceptionsDetails.OTHERS_EXCEPTION_PREFIX_ERROR);

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


        public User ExecuteGetUserIdRole(string email)
        {

            // Variables
            User user;
            SqlCommand sqlCommand;
            DataTable dataTable = new DataTable();
            List<User> userList = new List<User>();
         
            // Validations
            if (!IsConnectionOpen())
            {

                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.OpenConnection();
                else
                    throw new MSUsersException(ExceptionsDetails.SQLSERVER_CONNECTIONS_NO_CONNECTION);

            }

            // Prepare execution
            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;

            try
            { 
                
                // Parameters
                sqlCommand.Parameters.AddWithValue("@Email", email);

                // Query
                sqlCommand.CommandText = @"SELECT u.UserId, r.Role FROM [dbo].Users u INNER JOIN [dbo].Roles r ON u.RoleId = r.RoleId WHERE Email = @Email";
                
                // Executa a query
                sqlCommand.ExecuteNonQuery();

                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);

                dataAdapter.Fill(dataTable);

                string json = JsonConvert.SerializeObject(dataTable);

                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<User>));

                var ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
                List<User> h = (List<User>)jsonSerializer.ReadObject(ms);

                foreach (var item in h)
                {

                    userList.Add(new User(item.UserId, null, null, null, null, null, 0, item.Role));

                };

                user = userList[0];

            }
            catch (SqlException)
            {

                // dispose object
                sqlCommand.Dispose();

                // close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSUsersException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception)
            {
                // dispose object
                sqlCommand.Dispose();

                // close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSUsersException(ExceptionsDetails.OTHERS_EXCEPTION_PREFIX_ERROR);

            }
            finally
            {
                // dispose object
                sqlCommand.Dispose();

                // close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();
            }

            // return object returned by sql engine
            return user;
        }

       
        public User ExecuteGetAccount(int userId)
        {

            // Variables
            User user;
            SqlCommand sqlCommand;
            List<User> userList = new List<User>();
            DataTable dataTable = new DataTable();

            // Validations
            if (!IsConnectionOpen())
            {

                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.OpenConnection();
                else
                    throw new MSUsersException(ExceptionsDetails.SQLSERVER_CONNECTIONS_NO_CONNECTION);

            }

            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;

            try
            {

                // Parameters
                sqlCommand.Parameters.AddWithValue("@UserId", userId);

                // Query
                sqlCommand.CommandText = @"SELECT Email, Name, Address, PhoneNumber FROM [dbo].Users WHERE UserId = @UserId AND Inactive = 'False'";

                // Execute query and bring information
                sqlCommand.ExecuteNonQuery();

                // Execute query and keep number of affectedLines
                String hasActiveAccount = (String)sqlCommand.ExecuteScalar();

                if (hasActiveAccount == null)
                    throw new MSUsersException(ExceptionsDetails.ACCOUNT_IS_DISABLED);


                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);

                dataAdapter.Fill(dataTable);

                string json = JsonConvert.SerializeObject(dataTable);

                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<User>));

                var ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
                List<User> h = (List<User>)jsonSerializer.ReadObject(ms);

                foreach (var item in h)
                    userList.Add(new User(item.UserId, item.Email, "", item.Name, item.Address, item.PhoneNumber, 0, ""));
            
                user = userList[0];

            }
            catch (SqlException)
            {

                // dispose object
                sqlCommand.Dispose();

                // close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSUsersException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSUsersException(ex.Message);

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
            return user;

        }


        public int ExecuteDeleteAccount(int userId, string password)
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
                    throw new MSUsersException(ExceptionsDetails.SQLSERVER_CONNECTIONS_NO_CONNECTION);

            }

            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;

            try
            {

                // Parameters
                sqlCommand.Parameters.AddWithValue("@Password", Configs.Encrypt(password));
                sqlCommand.Parameters.AddWithValue("@UserId", userId);

                // Query
                sqlCommand.CommandText = @"UPDATE [dbo].Users SET Inactive = 1 WHERE UserId = @UserId AND Password = @Password;";

                // Execute query and keep number of affectedLines
                affectedLines = sqlCommand.ExecuteNonQuery();

                if (affectedLines == 0)
                    throw new MSUsersException(ExceptionsDetails.WRONG_PASSWORD);

            }
            catch (SqlException)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSUsersException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSUsersException(ex.Message);

            }
            finally
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();
            }

            // return object returned by sql engine
            return affectedLines;

        }


        public int ExecuteChangePassword(int userId, string password, string newpassword)
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
                    throw new MSUsersException(ExceptionsDetails.SQLSERVER_CONNECTIONS_NO_CONNECTION);
            
            }

            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;

            try
            {

                // Parameters
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                sqlCommand.Parameters.AddWithValue("@Password", Configs.Encrypt(password));
                sqlCommand.Parameters.AddWithValue("@NewPassword", Configs.Encrypt(newpassword));
               
                // Query
                sqlCommand.CommandText = @"UPDATE [dbo].Users SET Password = @NewPassword WHERE Password = @Password AND UserId = @UserId;";

                // Execute query and keep number of affectedLines
                affectedLines = sqlCommand.ExecuteNonQuery();

                if (affectedLines == 0)
                    throw new MSUsersException(ExceptionsDetails.WRONG_PASSWORD);

            }
            catch (SqlException)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSUsersException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSUsersException(ex.Message);

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


        public int ExecuteValidateLicensePlate(int userId, string licensePlate)
        {

            // Variables
            Int32 hasLicensePlate;
            SqlCommand sqlCommand;

            // Validations
            if (!IsConnectionOpen())
            {

                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.OpenConnection();
                else
                    throw new MSUsersException(ExceptionsDetails.SQLSERVER_CONNECTIONS_NO_CONNECTION);

            }

            // Prepare execution
            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;

            try
            {

                // Parameters
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                sqlCommand.Parameters.AddWithValue("@LicensePlate", licensePlate);

                // Query
                sqlCommand.CommandText = @"SELECT COUNT(v.[LicensePlate]) FROM[dbo].Vehicles v WHERE v.[UserId] = @UserId AND[LicensePlate] = @LicensePlate;";

                // Execute query and keep the returned object
                hasLicensePlate = (Int32)sqlCommand.ExecuteScalar();

            }
            catch (SqlException)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSUsersException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSUsersException(ExceptionsDetails.OTHERS_EXCEPTION_PREFIX_ERROR);

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
            return hasLicensePlate;

        }


        public int ExecuteAddNewVehicle(int userId, Vehicle vehicle)
        {

            // Variables
            Int32 hadAddedVehicle;
            SqlCommand sqlCommand;

            // Validations
            if (!IsConnectionOpen())
            {

                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.OpenConnection();
                else
                    throw new MSUsersException(ExceptionsDetails.SQLSERVER_CONNECTIONS_NO_CONNECTION);

            }

            // Prepare execution
            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;

            try
            {

                // Parameters
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                sqlCommand.Parameters.AddWithValue("@LicensePlate", vehicle.LicensePlate);
                sqlCommand.Parameters.AddWithValue("@Description", vehicle.VehicleType);

                // Query
                sqlCommand.CommandText = @"SELECT COUNT(LicensePlate) FROM [dbo].Vehicles WHERE LicensePlate = @LicensePlate";

                Int32 licensePlateExists = (Int32)sqlCommand.ExecuteScalar();

                if (licensePlateExists == 0)
                {

                    // Query
                    sqlCommand.CommandText = @"INSERT INTO [dbo].Vehicles (VehicleId, UserId, LicensePlate, VehicleTypeId) VALUES (NEWID(), @UserId, @LicensePlate, (SELECT VehicleTypeId FROM [dbo].VehiclesType WHERE Description = @Description));";

                    // Execute query and keep the returned object
                    hadAddedVehicle = (Int32)sqlCommand.ExecuteNonQuery();

                }
                else
                {
                    
                    throw new MSUsersException(ExceptionsDetails.LICENSE_PLATE_ALREADY_EXISTS);
               
                }

            }
            catch (SqlException)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSUsersException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSUsersException(ex.Message);

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
            return hadAddedVehicle;

        }

        public List<Vehicle> ExecuteVehiclesByUser(int userId)
        {

            // Variables
            SqlCommand sqlCommand;
            List<Vehicle> vehiclesList = new List<Vehicle>();
            DataTable dataTable = new DataTable();

            // Validations
            if (!IsConnectionOpen())
            {

                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.OpenConnection();
                else
                    throw new MSUsersException(ExceptionsDetails.SQLSERVER_CONNECTIONS_NO_CONNECTION);

            }

            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;

            try
            {

                // Parameters
                sqlCommand.Parameters.AddWithValue("@UserId", userId);

                // Query
                sqlCommand.CommandText = @"SELECT veh.LicensePlate, vet.Description as VehicleType FROM[dbo].Vehicles veh INNER JOIN [dbo].VehiclesType vet ON veh.[VehicleTypeId] = vet.[VehicleTypeId] WHERE veh.UserId = @UserId";

                // Execute query and bring information
                sqlCommand.ExecuteNonQuery();

                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);

                dataAdapter.Fill(dataTable);

                string json = JsonConvert.SerializeObject(dataTable);

                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<Vehicle>));

                var ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
                List<Vehicle> vehicles = jsonSerializer.ReadObject(ms) as List<Vehicle>;

                foreach (var vehicle in vehicles)
                    vehiclesList.Add(new Vehicle(vehicle.LicensePlate, vehicle.VehicleType));

            }
            catch (SqlException)
            {

                // dispose object
                sqlCommand.Dispose();

                // close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSUsersException(ExceptionsDetails.SQLSERVER_PREFIX_INTERNAL_ERROR);

            }
            catch (Exception ex)
            {

                // Dispose object
                sqlCommand.Dispose();

                // Close connection if using auto connection mode
                if (openCloseConnectionMode == SQLConnectionModes.auto)
                    this.CloseConnection();

                throw new MSUsersException(ex.Message);

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
            return vehiclesList;

        }

    }

}

#endregion
