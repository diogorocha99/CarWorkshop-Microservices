using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace MSGarages.Models
{
    public class User
    {
        /// <summary>
        /// User id
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// User email
        /// </summary>
        [DataMember]
        public string? Email { get; set; }


        /// <summary>
        /// User name
        /// </summary>
        [DataMember]
        public string? Name { get; set; }

        /// <summary>
        /// User phone number
        /// </summary>
        [DataMember]
        public string? PhoneNumber { get; set; }



        /// <summary>
        /// User constructor
        /// </summary>
        /// <param name="iD">User id</param>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        /// <param name="name">User name</param>
        /// <param name="authenticationAvailableTries">User authentication available tries</param>
        public User(int userId, string email,string name, string phoneNumber)
        {

            UserId = userId;
            Email = email;
            Name = name;
            PhoneNumber = phoneNumber;
            

        }
    }
}
