using Microsoft.AspNetCore.Mvc;
using MSRepairs.Configurations;
using MSRepairs.Engine;
using MSRepairs.Exceptions;
using MSRepairs.Models;
using MSRepairs.Tools;

namespace MSRepairs.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RepairController : ControllerBase
    {

        /// <summary>
        /// Variables used to get token credentials at launchSettings
        /// </summary>
        private readonly IConfiguration _configuration;


        /// <summary>
        /// RepairController constructor
        /// </summary>
        /// <param name="configuration">Token credentials configuration</param>
        public RepairController(IConfiguration configuration)
        {

            _configuration = configuration;

        }



        [Route("CurrentReparations")]
        [HttpGet]
        public ActionResult<List<Repair>> CurrentReparations([FromHeader] string token)
        {

            // Local variables
            string userRole;
            List<Repair> repair;

            try
            {

                // Variables
                MSRepairsEngine mSRepairsEngine;
                MSRepairsToken mSRepairsToken;

                // Setting the connection string
                mSRepairsEngine = new MSRepairsEngine(Configs.ConnectionString);

                // Setting the token configuration
                mSRepairsToken = new MSRepairsToken(_configuration);

                // Token validation
                if (!mSRepairsToken.ValidateToken(token))
                    throw new MSRepairsException(ExceptionsDetails.INVALID_TOKEN);

                // Get suerRole from token
                userRole = mSRepairsToken.GetUserIdRoleToken(token).Split(";")[1];

                if (userRole == "MANGR" || userRole == "ADMIN")
                {

                    repair = mSRepairsEngine.RepairMethod.CurrentReparations();

                    if (repair == null)
                        throw new MSRepairsException(ExceptionsDetails.SOMETHING_WENT_WRONG_WHILE_LOADING_REPARATIONS);

                }
                else
                {
                    throw new MSRepairsException(ExceptionsDetails.INVALID_PERMISSION);
                }

                return repair;

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }


        [Route("CurrentRepairsUser")]
        [HttpGet]
        public ActionResult<List<Repair>> CurrentRepairsUser([FromHeader] string token)
        {

            // Local variables
            string userRole;
            int userId;
            List<Repair> repair;

            try
            {

                // Variables
                MSRepairsEngine mSRepairsEngine;
                MSRepairsToken mSRepairsToken;

                // Setting the connection string
                mSRepairsEngine = new MSRepairsEngine(Configs.ConnectionString);

                // Setting the token configuration
                mSRepairsToken = new MSRepairsToken(_configuration);

                // Token validation
                if (!mSRepairsToken.ValidateToken(token))
                    throw new MSRepairsException(ExceptionsDetails.INVALID_TOKEN);

                userId = int.Parse(mSRepairsToken.GetUserIdRoleToken(token).Split(";")[0]);


                // Get suerRole from token
                userRole = mSRepairsToken.GetUserIdRoleToken(token).Split(";")[1];


                    repair = mSRepairsEngine.RepairMethod.CurrentReparationsUser(userId);

                    if (repair == null)
                        throw new MSRepairsException(ExceptionsDetails.SOMETHING_WENT_WRONG_WHILE_LOADING_REPARATIONS);


                return repair;

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }



        [Route("UserHistoryRepairs")]
        [HttpGet]
        public ActionResult<List<Repair>> UserHistoryRepairs([FromHeader] string token)
        {

            // Local variables
            string userRole;
            List<Repair> repair;
            int userId;

            try
            {

                // Variables
                MSRepairsEngine mSRepairsEngine;
                MSRepairsToken mSRepairsToken;

                // Setting the connection string
                mSRepairsEngine = new MSRepairsEngine(Configs.ConnectionString);

                // Setting the token configuration
                mSRepairsToken = new MSRepairsToken(_configuration);

                // Token validation
                if (!mSRepairsToken.ValidateToken(token))
                    throw new MSRepairsException(ExceptionsDetails.INVALID_TOKEN);

                userId = int.Parse(mSRepairsToken.GetUserIdRoleToken(token).Split(";")[0]);

                // Get userRole from token


                userRole = mSRepairsToken.GetUserIdRoleToken(token).Split(";")[1];


               repair = mSRepairsEngine.RepairMethod.UserHistoryRepairs(userId);

                if (repair == null)
                   throw new MSRepairsException(ExceptionsDetails.SOMETHING_WENT_WRONG_WHILE_LOADING_REPARATIONS);

            return repair;

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }



        [HttpPost]
        public ActionResult<bool> CreateReparation([FromHeader] int managerId, [FromHeader] string requestId, [FromHeader] int garageId, [FromHeader] int userId)
        {

            try
            {

                // Variables
                MSRepairsEngine mSRepairsEngine;

                // Setting the connection string
                mSRepairsEngine = new MSRepairsEngine(Configs.ConnectionString);

                if (!mSRepairsEngine.RepairMethod.CreateReparation(managerId, requestId, garageId, userId))
                    throw new MSRepairsException(ExceptionsDetails.SOMETHING_WENT_WRONG_WHILE_CREATING_REPARATION);

                return true;

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }


        [Route("AddPart")]
        [HttpPost]
        public ActionResult<string> AddPart([FromHeader] string token, [FromHeader] string repairId, [FromHeader] string stockId, [FromHeader] int quantity)
        {

            // Local variables
            string userRole;
            int userId;

            try
            {

                // Variables
                MSRepairsEngine mSRepairsEngine;
                MSRepairsToken mSRepairsToken;

                // Setting the connection string
                mSRepairsEngine = new MSRepairsEngine(Configs.ConnectionString);

                // Setting the token configuration
                mSRepairsToken = new MSRepairsToken(_configuration);

                // Token validation
                if (!mSRepairsToken.ValidateToken(token))
                    throw new Exception(ExceptionsDetails.INVALID_TOKEN);

                userId = int.Parse(mSRepairsToken.GetUserIdRoleToken(token).Split(";")[0]);

                // Get suerRole from token
                userRole = mSRepairsToken.GetUserIdRoleToken(token).Split(";")[1];

                if (userRole == "MANGR" || userRole == "ADMIN")
                {
                    // CHAMAR O METODO DE REMOVER PEÇA DEPENDEDNO DA QUANTIDADE
                    if (!mSRepairsEngine.RepairMethod.AddPart(token, repairId, stockId, quantity, userId))
                        throw new MSRepairsException(ExceptionsDetails.SOMETHING_WENT_WRONG_WHILE_ADDING_PART);
                }

                return "Part Added With Success!";

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }


        [Route("State")]
        [HttpPut]
        public ActionResult<string> ReparationState([FromHeader] string token, [FromHeader] string repairId, [FromHeader] string stateId, [FromHeader] string serviceTypeId, [FromHeader] int workTime, [FromHeader] string notes)
        {

            // Local variables
            string userRole;
            int userId;

            try
            {

                // Variables
                MSRepairsEngine mSRepairsEngine;
                MSRepairsToken mSRepairsToken;

                // Setting the connection string
                mSRepairsEngine = new MSRepairsEngine(Configs.ConnectionString);

                // Setting the token configuration
                mSRepairsToken = new MSRepairsToken(_configuration);

                // Token validation
                if (!mSRepairsToken.ValidateToken(token))
                    throw new Exception(ExceptionsDetails.INVALID_TOKEN);



                userId = int.Parse(mSRepairsToken.GetUserIdRoleToken(token).Split(";")[0]);


                // Get suerRole from token
                userRole = mSRepairsToken.GetUserIdRoleToken(token).Split(";")[1];

                if (userRole == "MANGR" || userRole == "ADMIN")
                {
                    if (!mSRepairsEngine.RepairMethod.ReparationState(repairId, stateId, serviceTypeId, workTime, notes, userId))
                        throw new MSRepairsException(ExceptionsDetails.SOMETHING_WENT_WRONG_WHILE_UPDATING_REPARATION);
                }

                return "Reparation Updated With Success!";

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }

    }

}
