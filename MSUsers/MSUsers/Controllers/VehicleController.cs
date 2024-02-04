#region Usings

using MSUsers.Tools;
using MSUsers.Engine;
using MSUsers.Exceptions;
using MSUsers.Configurations;
using Microsoft.AspNetCore.Mvc;
using MSUsers.Models;

#endregion

#region VehicleController

namespace MSUsers.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {

        /// <summary>
        /// Variables used to get token credentials at launchSettings
        /// </summary>
        private readonly IConfiguration _configuration;


        /// <summary>
        /// VehicleController constructor
        /// </summary>
        /// <param name="configuration">Token credentials configuration</param>
        public VehicleController(IConfiguration configuration)
        {

            _configuration = configuration;

        }

        #region ValidateLicensePlate

        [HttpGet]
        public ActionResult<bool> ValidateLicensePlate([FromHeader] int userId, [FromHeader] string licensePlate)
        {

            try
            {

                // Variables
                MSUsersEngine mSUsersEngine;

                // Setting the connection string
                mSUsersEngine = new MSUsersEngine(Configs.ConnectionString);


                if (!mSUsersEngine.VehicleMethod.ValidateLicensePlate(userId, licensePlate))
                    throw new MSUsersException(ExceptionsDetails.INVALID_LICENSEPLATE);

                return true;

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }

        #endregion

        #region VehiclesByUser

        [Route("VehiclesByUser")]
        [HttpGet]
        public ActionResult<List<Vehicle>> VehiclesByUser([FromHeader] string token)
        {

            // Local variables
            int userId;
            var vehicles = new List<Vehicle>();

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
                userId = int.Parse(mSUsersToken.GetUserIdRoleToken(token).Split(";")[0]);

                // Get user informations method
                vehicles = mSUsersEngine.VehicleMethod.GetVehiclesByUser(userId);

                return vehicles;

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }

        #endregion

        #region ValidateLicensePlate

        [HttpPost]
        public ActionResult<string> ValidateLicensePlate([FromHeader] string token, [FromBody] Vehicle vehicle)
        {
            // Local Variables
            int userId;

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
                userId = int.Parse(mSUsersToken.GetUserIdRoleToken(token).Split(";")[0]);

                if (!mSUsersEngine.VehicleMethod.AddNewVehicle(userId, vehicle))
                    throw new MSUsersException(ExceptionsDetails.SOMETHING_WENT_WRONG_WHILE_ADDING_NEW_VEHICLE);

                return "Vehicle Added With Success!";

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
