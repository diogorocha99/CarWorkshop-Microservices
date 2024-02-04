#region Usings

using Microsoft.AspNetCore.Mvc;
using MSRepairs.Configurations;
using MSRepairs.Engine;
using MSRepairs.Exceptions;
using MSRepairs.Models;
using MSRepairs.Tools;

#endregion

#region StockController

namespace MSRepairs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {

        /// <summary>
        /// Variables used to get token credentials at launchSettings
        /// </summary>
        private readonly IConfiguration _configuration;


        /// <summary>
        /// StockController constructor
        /// </summary>
        /// <param name="configuration">Token credentials configuration</param>
        public StockController(IConfiguration configuration)
        {

            _configuration = configuration;

        }



        [HttpGet]
        public ActionResult<List<Stock>> GetStock([FromHeader] string token, [FromHeader] int garageId)
        {

            // Local variables
            string userRole;
            List<Stock> stock;

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

                    stock = mSRepairsEngine.StockMethod.GetStock(garageId);

                    if (stock == null)
                        throw new MSRepairsException(ExceptionsDetails.SOMETHING_WENT_WRONG_WHILE_LOADING_REPARATIONS);

                }
                else
                {
                    throw new MSRepairsException(ExceptionsDetails.INVALID_PERMISSION);
                }

                return stock;

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }


        [Route("RemoveStock")]
        [HttpPut]
        public ActionResult<string> UpdateStock([FromHeader] string token, [FromHeader] string stockId, [FromHeader] int quantity, [FromHeader] int garageId)
        {

            // Local variables
            string userRole;

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

                // Get suerRole from token
                userRole = mSRepairsToken.GetUserIdRoleToken(token).Split(";")[1];

                if (userRole == "MANGR" || userRole == "ADMIN")
                {
                    if (!mSRepairsEngine.StockMethod.RemoveStock(stockId, quantity, garageId))
                        throw new MSRepairsException(ExceptionsDetails.SOMETHING_WENT_WRONG_WHILE_REMOVING_PART);
                }

                return "Stock Updated With Success!";

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }


    }
}

#endregion
