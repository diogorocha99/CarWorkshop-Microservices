#region AuthenticationResponse Class

namespace MSUsers.Models
{

    public class AuthenticationResponse
    {

        /// <summary>
        /// User
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Authentication response construtctor
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="token">Token</param>
        public AuthenticationResponse(User user, string token )
        {

            User = user;
            Token = token;

        }

    }

}

#endregion
