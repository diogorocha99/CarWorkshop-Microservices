#region Usings

using System.ComponentModel.DataAnnotations;

#endregion

#region AuthenticationRequest Class

namespace MSUsers.Models
{

    public class AuthenticationRequest
    {

        /// <summary>
        /// User email
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        [Required]
        public string Password { get; set; }

    }

}

#endregion
