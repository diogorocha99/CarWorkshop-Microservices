#region Usings

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

#endregion

#region User Class

namespace MSUsers.Models
{

    [DataContract]
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
        /// User password
        /// Ignored by json
        /// </summary>
        [DataMember]
        public string? Password { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        [DataMember]
        public string? Name { get; set; }

        /// <summary>
        /// User address
        /// </summary>
        [DataMember]
        public string? Address { get; set; }

        /// <summary>
        /// User phone number
        /// </summary>
        [DataMember]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// User authentication available tries
        /// Ignored by json
        /// </summary>
        [DataMember]
        [JsonIgnore]
        public int AuthenticationAvailableTries { get; set; }

        /// <summary>
        /// User authentication role
        /// Ignored by json
        /// </summary>
        [DataMember]
        [JsonIgnore]
        public string? Role { get; set; }


        /// <summary>
        /// User constructor
        /// </summary>
        /// <param name="iD">User id</param>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        /// <param name="name">User name</param>
        /// <param name="authenticationAvailableTries">User authentication available tries</param>
        public User(int userId, string email, string password, string name, string address, string phoneNumber, int authenticationAvailableTries, string role)
        {

            UserId = userId;
            Email = email;
            Password = password;
            Name = name;
            Address = address;
            PhoneNumber = phoneNumber;
            AuthenticationAvailableTries = authenticationAvailableTries;
            Role = role;

        }

    }

}

#endregion
