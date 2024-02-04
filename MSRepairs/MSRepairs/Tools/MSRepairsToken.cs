#region Usings

using System.Text;
using MSRepairs.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

#endregion

#region MSRepairsToken

namespace MSRepairs.Tools
{
    public class MSRepairsToken
    {
        /// <summary>
        /// Variables used to get token credentials at launchSettings
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// MSUsersToken constructor
        /// </summary>
        /// <param name="configuration">Token credentials configuration</param>
        public MSRepairsToken(IConfiguration configuration)
        {

            _configuration = configuration;

        }

        #region ValidateToken

        /// <summary>
        /// Method used to validate token
        /// </summary>
        /// <param name="token">Token</param>
        /// <returns>Returns a boolean value according to token validation</returns>
        public bool ValidateToken(string token)
        {

            if (token == null)
                return false;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:key"]);

            try
            {

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // Set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero

                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
                var userRole = jwtToken.Claims.First(x => x.Type == "role").Value;

                // Return true if validation successful
                return true;

            }
            catch (Exception)
            {

                // Return full if validation fails
                return false;

            }

        }

        #endregion

        #region GetUserIdToken

        /// <summary>
        /// Method used to return the user id and role from token
        /// </summary>
        /// <param name="token">Token</param>
        /// <returns>The userId and role codified on token</returns>
        /// <exception cref="MSUsersException">Exception thrown in case token is invalid</exception>
        public string GetUserIdRoleToken(string token)
        {

            if (token == null)
                throw new MSRepairsException(ExceptionsDetails.INVALID_TOKEN);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:key"]);

            try
            {

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // Set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero

                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
                var userRole = jwtToken.Claims.First(x => x.Type == "role").Value;


                if (userId == 0)
                    throw new MSRepairsException(ExceptionsDetails.INVALID_USER_ID_FROM_TOKEN);

                if (userRole == null)
                    throw new MSRepairsException(ExceptionsDetails.INVALID_USER_ROLE_FROM_TOKEN);

                // Return userId and role from JWT token if validation successful
                return $"{userId};{userRole}";

            }
            catch (Exception ex)
            {

                throw new MSRepairsException(ex.Message);

            }

        }

        #endregion
    }
}

#endregion
