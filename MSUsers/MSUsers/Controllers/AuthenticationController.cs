#region Usings

using MSUsers.Tools;
using MSUsers.Engine;
using MSUsers.Models;
using MSUsers.Exceptions;
using MSUsers.Configurations;
using Microsoft.AspNetCore.Mvc;

#endregion

#region AuthenticationController

namespace MSUsers.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        /// <summary>
        /// Variables used to get token credentials at launchSettings
        /// </summary>
        private readonly IConfiguration _configuration;


        /// <summary>
        /// AuthenticationController constructor
        /// </summary>
        /// <param name="configuration">Token credentials configuration</param>
        public AuthenticationController(IConfiguration configuration)
        {

            _configuration = configuration;

        }

        #region Authentication

        [HttpPost]
        public ActionResult<AuthenticationResponse> Authentication([FromBody] AuthenticationRequest authenticationRequest)
        {

            // Local variables
            string token;
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

                // Login method
                if (!mSUsersEngine.Login(authenticationRequest.Email, authenticationRequest.Password))
                {

                    // SE AS CREDENCIAIS ERRADAS OU 3 TRIES
                    mSUsersEngine.AuthenticationMethod.AddLoginAttemps(authenticationRequest.Email);
                    throw new MSUsersException(ExceptionsDetails.AUTHENTICATION_INVALID_CREDENTIALS);
                  
                }

                //calls logs api
                Logs loginSuccessfulLog = new Logs(mSUsersEngine.AuthenticatedUser.UserId.ToString(), "Login Successful");


                // Generate the token with userId and userRole
                token = mSUsersToken.GenerateToken(mSUsersEngine.AuthenticatedUser);

                if (MSLogs.InsertLog(loginSuccessfulLog))
                    MSLogs.CallLogsApi();


                // Returns the authentication response object
                return new AuthenticationResponse(mSUsersEngine.AuthenticatedUser, token);

            }
            catch (Exception ex)
            {

                Logs loginFailedLog = new Logs(authenticationRequest.Email, "Login Failed");
                if (MSLogs.InsertLog(loginFailedLog))
                    MSLogs.CallLogsApi();


                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }

        #endregion

        #region ValidateToken

        [Route("ValidateToken")]
        [HttpPost]
        public bool GenerateTokenKey([FromHeader] string token)
        {

            // Variables
            MSUsersToken mSUsersToken;

            // Setting the token configuration
            mSUsersToken = new MSUsersToken(_configuration);

            // Returns the boolean value according to token validation
            bool response = mSUsersToken.ValidateToken(token);

            return response;

        }

        #endregion

    }

}

#endregion
