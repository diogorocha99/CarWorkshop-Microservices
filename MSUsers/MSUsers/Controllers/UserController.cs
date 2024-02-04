#region Usings

using MSUsers.Tools;
using MSUsers.Engine;
using MSUsers.Models;
using MSUsers.Exceptions;
using MSUsers.Configurations;
using Microsoft.AspNetCore.Mvc;

#endregion

#region UserController

namespace MSUsers.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        /// <summary>
        /// Variables used to get token credentials at launchSettings
        /// </summary>
        private readonly IConfiguration _configuration;


        /// <summary>
        /// UserController constructor
        /// </summary>
        /// <param name="configuration">Token credentials configuration</param>
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region Register

        [HttpPost]
        public ActionResult<User> Register([FromBody] User user)
        {
            MSUsersLogs MSLogs;
            MSLogs = new MSUsersLogs();

            try
            {

                // Variables
                MSUsersEngine mSUsersEngine;

                // Setting the connection string
                mSUsersEngine = new MSUsersEngine(Configs.ConnectionString);

                // Verify is account exists
                if (!mSUsersEngine.UserMethod.HasEmailRegistered(user.Email))
                    throw new MSUsersException(ExceptionsDetails.ACCOUNT_ALREADY_EXISTS);

                // Register user method
                mSUsersEngine.UserMethod.Register(user);

                Logs registerSuccessfulLog = new Logs(user.Email.ToString(), "Register Successful");

                if (MSLogs.InsertLog(registerSuccessfulLog))
                    MSLogs.CallLogsApi();

                // Returns the new user
                return new User(user.UserId, user.Email, user.Password, user.Name, user.Address,  user.PhoneNumber, user.AuthenticationAvailableTries, user.Role);

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }

    

        #endregion

        #region GetUser

        [HttpGet]
        public ActionResult<User> GetUser([FromHeader] string token)
        {

            // Local variables
            User user;
            int userId;
            string userRole;

            try
            {

                // Variables
                MSUsersEngine mSUsersEngine;
                MSUsersToken mSUsersToken;

                // Setting the connection string
                mSUsersEngine = new MSUsersEngine(Configs.ConnectionString);

                // Setting the token configuration
                mSUsersToken = new MSUsersToken(_configuration);

                // Token validation
                if (!mSUsersToken.ValidateToken(token))
                    throw new Exception(ExceptionsDetails.INVALID_TOKEN);

                // Get userId from token
                userId =  int.Parse(mSUsersToken.GetUserIdRoleToken(token).Split(";")[0]);

                // Get suerRole from token
                userRole = mSUsersToken.GetUserIdRoleToken(token).Split(";")[1];


                // Get user informations method
                user = mSUsersEngine.UserMethod.GetUserInformations(userId);

                //var data = new
                //{
                //    email = user.Email,
                //    name = user.Name
                //};

                // Return the user
                //return $"{data}";
                return user;

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }

        #endregion

        #region UserRole

        [Route("Role")]
        [HttpGet]
        public ActionResult<string> UserRole([FromHeader] string token)
        {

            // Local variables
            string userRole;

            try
            {

                // Variables
                MSUsersEngine mSUsersEngine;
                MSUsersToken mSUsersToken;

                // Setting the connection string
                mSUsersEngine = new MSUsersEngine(Configs.ConnectionString);

                // Setting the token configuration
                mSUsersToken = new MSUsersToken(_configuration);

                // Token validation
                if (!mSUsersToken.ValidateToken(token))
                    throw new Exception(ExceptionsDetails.INVALID_TOKEN);

                // Get suerRole from token
                userRole = mSUsersToken.GetUserIdRoleToken(token).Split(";")[1];

                // Return the user role
                return userRole;

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }

        #endregion

        //TO use in Garage 
        [Route("RolebyEmail")]
        [HttpGet]
        public string UserRoleID([FromHeader] string email)
        {

          // Local variables
            User user;

            //Variables
           MSUsersEngine mSUsersEngine;
            MSUsersToken mSUsersToken;

            //Setting the connection string
           mSUsersEngine = new MSUsersEngine(Configs.ConnectionString);

            //Setting the token configuration

            user = mSUsersEngine.UserMethod.GetUserInformationsLogin(email);


            //Return the user role
            return $"{user.UserId};{user.Role}";

        }


            #region ChangePassword

        [Route("ChangePassword")]
        [HttpPut]
        public ActionResult<string> ChangePassword([FromHeader] string token, [FromHeader] string password, [FromHeader] string newpassword)
        {

            // Local variables
            int userId;
            MSUsersLogs MSLogs;
            MSLogs = new MSUsersLogs();

            try
            {
                // Variables
                MSUsersEngine mSUsersEngine;
                MSUsersToken mSUsersToken;

                // Setting the connection string
                mSUsersEngine = new MSUsersEngine(Configs.ConnectionString);

                // Setting the token configuration
                mSUsersToken = new MSUsersToken(_configuration);

                // Token validation
                if (!mSUsersToken.ValidateToken(token))
                    throw new MSUsersException(ExceptionsDetails.INVALID_TOKEN);

                // Get userId from token
                userId = int.Parse(mSUsersToken.GetUserIdRoleToken(token).Split(";")[0]);

                // Change password method
                if (!mSUsersEngine.UserMethod.ChangePassword(userId, password, newpassword))
                    throw new Exception(ExceptionsDetails.SOMETHING_WENT_WRONG_WHILE_CHANGING_PASSWORD);


                Logs passwordchangeSuccessfulLog = new Logs(userId.ToString(), "Password Changed");

                if (MSLogs.InsertLog(passwordchangeSuccessfulLog))
                    MSLogs.CallLogsApi();


                return "Password Changed With Success!";

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }

        #endregion

        #region DeleteUser

        [Route("Delete")]
        [HttpPut]
        public ActionResult<string> DeleteUser([FromHeader] string token, [FromHeader] string password)
        {

            // Local variables
            int userId;
            string userRole;
            MSUsersLogs MSLogs;
            MSLogs = new MSUsersLogs();

            try
            {

                // Variables
                MSUsersEngine mSUsersEngine;
                MSUsersToken mSUsersToken;

                // Setting the connection string
                mSUsersEngine = new MSUsersEngine(Configs.ConnectionString);

                // Setting the token configuration
                mSUsersToken = new MSUsersToken(_configuration);

                // Token validation
                if (!mSUsersToken.ValidateToken(token))
                    throw new MSUsersException(ExceptionsDetails.INVALID_TOKEN);

                // Get userId from token
                userId = int.Parse(mSUsersToken.GetUserIdRoleToken(token).Split(";")[0]);

                // Get suerRole from token
                userRole = mSUsersToken.GetUserIdRoleToken(token).Split(";")[1];

                // Delete user method
                if (!mSUsersEngine.UserMethod.DeleteUser(userId, password))
                    throw new Exception(ExceptionsDetails.SOMETHING_WENT_WRONG_WHILE_DELETING_ACCOUNT);


                Logs accountdeletedSuccessfulLog = new Logs(userId.ToString(), "Account Deleted");

                if (MSLogs.InsertLog(accountdeletedSuccessfulLog))
                    MSLogs.CallLogsApi();

                return "Account Deleted with Success!";

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }

        #endregion

    }

}

#endregion
