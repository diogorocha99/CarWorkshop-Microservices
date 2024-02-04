using Microsoft.AspNetCore.Mvc;
using MSGarages.Configurations;
using MSGarages.Engine;
using MSGarages.Exceptions;
using MSGarages.Tools;
using MSGarages.Models;

namespace MSGarages.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GarageController : Controller
    {
        /// <summary>
        /// Variables used to get token credentials at launchSettings
        /// </summary>
        private readonly IConfiguration _configuration;


        /// <summary>
        /// RequestController constructor
        /// </summary>
        /// <param name="configuration">Token credentials configuration</param>
        public GarageController(IConfiguration configuration)
        {

            _configuration = configuration;

        }


        #region CreateGarage
        [Route("CreateGarage")]
        [HttpPost]
        public ActionResult<string> CreateGarage([FromHeader] string token, [FromHeader] string adress, [FromHeader] string name, [FromHeader] string postal_code)
        {

            // Local variables
            int userId;
            string userRole;

            try
            {

                // Variables
                MSGaragesEngine mSGaragesEngine;
                MSGaragesToken mSGaragesToken;

                // Setting the connection string
                mSGaragesEngine = new MSGaragesEngine(Configs.ConnectionString);

                // Setting the token configuration
                mSGaragesToken = new MSGaragesToken(_configuration);



                // Get userId from token
                userId = int.Parse(mSGaragesToken.GetUserIdRoleToken(token).Split(";")[0]);

                // Get userRole from token
                userRole = mSGaragesToken.GetUserIdRoleToken(token).Split(";")[1];

                if (userRole == "ADMIN")
                {

                    // Verify if garage already exists exists
                    if (!mSGaragesEngine.GarageMethod.GarageExists(adress, name))
                        throw new MSGaragesException(ExceptionsDetails.GARAGE_ALREADY_EXISTS);

                    if (!mSGaragesEngine.GarageMethod.CreateGarage(userId, adress, name, postal_code))
                        throw new MSGaragesException(ExceptionsDetails.SOMETHING_WENT_WRONG_WHILE_CREATING_GARAGE);

                }
                else throw new MSGaragesException(ExceptionsDetails.INVALID_PERMISSION);

                return StatusCode(StatusCodes.Status200OK, "Garage Added with Success!");

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }
        #endregion


        [Route("AddEmployeeGarage")]
        [HttpPost]
        public ActionResult<string> AddEmployeeGarage([FromHeader] string token, [FromHeader] string email, [FromHeader] int garageid)
        {

            // Local variables
            int userId;
            string userRole;

            try
            {

                // Variables
                MSGaragesEngine mSGaragesEngine;
                MSGaragesToken mSGaragesToken;

                // Setting the connection string
                mSGaragesEngine = new MSGaragesEngine(Configs.ConnectionString);

                // Setting the token configuration
                mSGaragesToken = new MSGaragesToken(_configuration);



                // Get userId from token
                userId = int.Parse(mSGaragesToken.GetUserIdRoleToken(token).Split(";")[0]);

                // Get userRole from token
                userRole = mSGaragesToken.GetUserIdRoleToken(token).Split(";")[1];

                if (userRole == "ADMIN")
                {

                    if (!mSGaragesEngine.GarageMethod.AddEmployeeMethod(email, garageid))
                        throw new MSGaragesException(ExceptionsDetails.SOMETHING_WENT_WRONG_WHILE_ADDING_EMPLOYEE);

                }
                else throw new MSGaragesException(ExceptionsDetails.INVALID_PERMISSION);

                return StatusCode(StatusCodes.Status200OK, "Employee Added with Success!");

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }

        [Route("CheckEmployeeGarage")]
        [HttpGet]
        public ActionResult<int> CheckEmployeeGarage([FromHeader] string token)
        {
            int garageid;
            int userId;
            string userRole;

            try
            {
                // Variables
                MSGaragesEngine mSGaragesEngine;
                MSGaragesToken mSGaragesToken;

                // Setting the connection string
                mSGaragesEngine = new MSGaragesEngine(Configs.ConnectionString);

                // Setting the token configuration
                mSGaragesToken = new MSGaragesToken(_configuration);



                // Get userId from token
                userId = int.Parse(mSGaragesToken.GetUserIdRoleToken(token).Split(";")[0]);

                // Get userRole from token
                userRole = mSGaragesToken.GetUserIdRoleToken(token).Split(";")[1];

                garageid = mSGaragesEngine.GarageMethod.EmployeeGarage(userId);


                return garageid;

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

        //[Route("EmployersByGarage")]
        //[HttpGet]
        //public ActionResult<List<User>> EmployersByGarage([FromHeader] string token, [FromHeader] int garagId)
        //{
        //    int garageid;
        //    int userId;
        //    string userRole;
        //    List<User> users;

        //    try
        //    {
        //        // Variables
        //        MSGaragesEngine mSGaragesEngine;
        //        MSGaragesToken mSGaragesToken;

        //        // Setting the connection string
        //        mSGaragesEngine = new MSGaragesEngine(Configs.ConnectionString);

        //        // Setting the token configuration
        //        mSGaragesToken = new MSGaragesToken(_configuration);



        //        // Get userId from token
        //        userId = int.Parse(mSGaragesToken.GetUserIdRoleToken(token).Split(";")[0]);

        //        // Get userRole from token
        //        userRole = mSGaragesToken.GetUserIdRoleToken(token).Split(";")[1];

        //        if (userRole == "ADMIN")
        //        {
        //            users = mSGaragesEngine.GarageMethod.GarageEmployees(garageid);
        //        }
        //        else throw new MSGaragesException(ExceptionsDetails.INVALID_PERMISSION);



        //        return users;

        //        catch (Exception ex)
        //    {

        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

        //    }

        //}

    }

}

