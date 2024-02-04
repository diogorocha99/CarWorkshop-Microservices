using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MSPayments.Configurations;
using MSPayments.Engine;
using MSPayments.Exceptions;
using MSPayments.Model;
using MSPayments.Tools;

namespace MSPayments.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : Controller
    {

        /// <summary>
        /// Variables used to get token credentials at launchSettings
        /// </summary>
        private readonly IConfiguration _configuration;


        /// <summary>
        /// RepairController constructor
        /// </summary>
        /// <param name="configuration">Token credentials configuration</param>
        public PaymentsController(IConfiguration configuration)
        {

            _configuration = configuration;

        }




        [HttpPost]
        public void CreatePayment([FromHeader] int UserId, [FromHeader] string RepairId, [FromHeader] string LicensePlate, [FromHeader] string ServiceTypeId, [FromHeader] double Price)
        {

            try
            {

                MSPaymentsEngine mSPaymentsEngine;

                // Setting the connection string
                mSPaymentsEngine = new MSPaymentsEngine(Configs.ConnectionString);

                mSPaymentsEngine.PaymentMethod.VerifyPayment(UserId, RepairId, LicensePlate, ServiceTypeId, Price);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);

            }

        }


        [HttpGet]
        public ActionResult<List<Payments>> UserHistoryPayments([FromHeader] string token)
        {

            // Local variables
            string userRole;
            List<Payments> payments;
            int userId;

            try
            {

                // Variables
                MSPaymentsEngine mSPaymentsEngine;
                MSPaymentsToken mSPaymentsToken;

                // Setting the connection string
                mSPaymentsEngine = new MSPaymentsEngine(Configs.ConnectionString);

                // Setting the token configuration
                mSPaymentsToken = new MSPaymentsToken(_configuration);

                // Token validation
                if (!mSPaymentsToken.ValidateToken(token))
                    throw new MSPaymentsException(ExceptionsDetails.INVALID_TOKEN);

                userId = int.Parse(mSPaymentsToken.GetUserIdRoleToken(token).Split(";")[0]);

                // Get userRole from token


                userRole = mSPaymentsToken.GetUserIdRoleToken(token).Split(";")[1];


                payments = mSPaymentsEngine.PaymentMethod.UserHistoryPayments(userId);

                if (payments == null)
                    throw new MSPaymentsException(ExceptionsDetails.SOMETHING_WENT_WRONG);


            return payments;

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }

    }

}
