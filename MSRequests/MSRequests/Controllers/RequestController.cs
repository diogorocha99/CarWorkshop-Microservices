#region Usings

using MSRequests.Tools;
using MSRequests.Engine;
using MSRequests.Models;
using MSRequests.Exceptions;
using Microsoft.AspNetCore.Mvc;
using MSRequests.Configurations;

#endregion

#region RequestController

namespace MSRequests.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {

        /// <summary>
        /// Variables used to get token credentials at launchSettings
        /// </summary>
        private readonly IConfiguration _configuration;


        /// <summary>
        /// RequestController constructor
        /// </summary>
        /// <param name="configuration">Token credentials configuration</param>
        public RequestController(IConfiguration configuration)
        {

            _configuration = configuration;

        }

        #region CreateRequest

        [HttpPost]
        public ActionResult<string> CreateRequest([FromHeader] string token, [FromHeader] string licensePlate, [FromHeader] int garageid)
        {

            // Local variables
            int userId;
            string userRole;

            try
            {

                // Variables
                MSRequestsEngine mSRequestsEngine;
                MSRequestsToken mSRequestsToken;

                // Setting the connection string
                mSRequestsEngine = new MSRequestsEngine(Configs.ConnectionString);

                // Setting the token configuration
                mSRequestsToken = new MSRequestsToken(_configuration);

                // Get userId from token
                userId = int.Parse(mSRequestsToken.GetUserIdRoleToken(token).Split(";")[0]);

                // Get userRole from token
                userRole = mSRequestsToken.GetUserIdRoleToken(token).Split(";")[1];

                if (userRole == "USERS")
                {
                    if (!mSRequestsEngine.RequestMethod.CreateRequest(userId, licensePlate, garageid))
                        throw new MSRequestsException(ExceptionsDetails.SOMETHING_WENT_WRONG_WHILE_CREATING_REQUEST);

                }
                else throw new MSRequestsException(ExceptionsDetails.INVALID_PERMISSION);

                return StatusCode(StatusCodes.Status200OK, "Request Registered with Success!");

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }

        #endregion

        #region ValidateRequest

        [HttpPut]
        public ActionResult<string> ValidateRequest([FromHeader] string token, [FromHeader] string requestId, [FromHeader] bool validate)
        {     
            
            // Local variables
            int managerId;
            string userRole;

            try
            {

                // Variables
                MSRequestsToken mSRequestsToken;
                MSRequestsEngine mSRequestsEngine;

                // Setting the connection string
                mSRequestsEngine = new MSRequestsEngine(Configs.ConnectionString);

                // Setting the token configuration
                mSRequestsToken = new MSRequestsToken(_configuration);

                // Get userId from token
                managerId = int.Parse(mSRequestsToken.GetUserIdRoleToken(token).Split(";")[0]);

                // Get userRole from token
                userRole = mSRequestsToken.GetUserIdRoleToken(token).Split(";")[1];

                if (userRole == "MANGR" || userRole == "ADMIN")
                {

                    if (!mSRequestsEngine.RequestMethod.ValidateRequest(managerId, requestId, validate, token))
                        throw new MSRequestsException(ExceptionsDetails.SOMETHING_WENT_WRONG_WHILE_VALIDATING_REQUEST);

                    mSRequestsEngine.RequestMethod.SendEmail();
                }
                else throw new MSRequestsException(ExceptionsDetails.INVALID_PERMISSION);

                //if (validate)
                //    mSRequestsEngine.RequestMethod.CreateReparation(managerId, requestId);

                return StatusCode(StatusCodes.Status200OK, "Request Validated with Success!");

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }

        #endregion

        #region GetUnvalidatedRequests

        [HttpGet]
        public ActionResult<List<Requests>> GetUnvalidatedRequests([FromHeader] string token)
        {

            // Local variables
            List<Requests> requests;
            int userId;
            string userRole;

            try
            {

                // Variables
                MSRequestsToken mSRequestsToken;
                MSRequestsEngine mSRequestsEngine;

                // Setting the connection string
                mSRequestsEngine = new MSRequestsEngine(Configs.ConnectionString);

                // Setting the token configuration
                mSRequestsToken = new MSRequestsToken(_configuration);

                // Get userRole from token
                userRole = mSRequestsToken.GetUserIdRoleToken(token).Split(";")[1];

                if (userRole == "MANGR" || userRole == "ADMIN")
                {

                    requests = mSRequestsEngine.RequestMethod.GetUnvalidatedRequests();

                }
                else throw new MSRequestsException(ExceptionsDetails.INVALID_PERMISSION);

                return requests;

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }

        #endregion



        #region GetRequestInformation

        [Route("RequestInfo")]
        [HttpGet]
        public ActionResult<Requests> GetRequestInformation([FromHeader] string requestId)
        {

            // Local variables
            Requests request;

            try
            {

                // Variables
                MSRequestsEngine mSRequestsEngine;

                // Setting the connection string
                mSRequestsEngine = new MSRequestsEngine(Configs.ConnectionString);

                request = mSRequestsEngine.RequestMethod.GetRequestInformation(requestId);

                return request;

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }

        #endregion

        #region GetRequestsUser
        [Route("GetUnvalidatedRequestsUser")]
        [HttpGet]
        public ActionResult<List<Requests>> GetUnvalidatedRequestsUser([FromHeader] string token)
        {

            // Local variables
            List<Requests> requests;
            int userId;
            string userRole;

            try
            {

                // Variables
                MSRequestsToken mSRequestsToken;
                MSRequestsEngine mSRequestsEngine;

                // Setting the connection string
                mSRequestsEngine = new MSRequestsEngine(Configs.ConnectionString);

                // Setting the token configuration
                mSRequestsToken = new MSRequestsToken(_configuration);
                userId = int.Parse(mSRequestsToken.GetUserIdRoleToken(token).Split(";")[0]);
                // Get userRole from token
                userRole = mSRequestsToken.GetUserIdRoleToken(token).Split(";")[1];

                if (userRole == "USERS")
                {

                    requests = mSRequestsEngine.RequestMethod.GetRequestsUser(userId);

                }
                else throw new MSRequestsException(ExceptionsDetails.INVALID_PERMISSION);

                return requests;

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }

        #endregion



        #region GetRequestsUser
        [Route("GetallRequestIDsUser")]
        [HttpGet]
        public ActionResult<List<string>> GetallPendingdRequestsUser([FromHeader] string token)
        {

            // Local variables
            List<string> requests;
            int userId;
            string userRole;

            try
            {

                // Variables
                MSRequestsToken mSRequestsToken;
                MSRequestsEngine mSRequestsEngine;

                // Setting the connection string
                mSRequestsEngine = new MSRequestsEngine(Configs.ConnectionString);

                // Setting the token configuration
                mSRequestsToken = new MSRequestsToken(_configuration);
                userId = int.Parse(mSRequestsToken.GetUserIdRoleToken(token).Split(";")[0]);
                // Get userRole from token
                userRole = mSRequestsToken.GetUserIdRoleToken(token).Split(";")[1];

                if (userRole == "USERS")
                {

                    requests = mSRequestsEngine.RequestMethod.GetalldRequestsUser(userId);

                }
                else throw new MSRequestsException(ExceptionsDetails.INVALID_PERMISSION);

                return requests;

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
