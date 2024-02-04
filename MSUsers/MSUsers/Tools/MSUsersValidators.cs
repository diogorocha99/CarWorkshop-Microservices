#region Usings

using System.Text.RegularExpressions;
using static MSUsers.Tools.MSUsersEnums;

#endregion

#region MSUsersValidators Class

namespace MSUsers.Tools
{
    public class MSUsersValidators
    {

        #region IsValidEmail

        /// <summary>
        /// Method to validate the email insert by user
        /// </summary>
        /// <param name="email">The users email</param>
        /// <returns>Boolean according to validation</returns>
        public static bool IsValidEmail(string email)
        {

            // Validate empty value
            if (email.Trim().Length == 0) return false;


            // Validate pattern
            if (!IsValidRegex(email, ContentType.EMAIL)) return false;


            // Is valid
            return true;

        }

        #endregion

        #region IsValidPassword

        /// <summary>
        /// Method to validate the password insert by user
        /// </summary>
        /// <param name="password">The users password</param>
        /// <returns>Boolean according to validation</returns>
        public static bool IsValidPassword(string password)
        {

            // Validate empty value
            if (password.Trim().Length == 0) return false;


            // Validate pattern
            if (!IsValidRegex(password, ContentType.PASSWORD)) return false;


            // Is valid
            return true;

        }

        #endregion

        #region IsValidName

        /// <summary>
        /// Method to validate the name insert by user
        /// </summary>
        /// <param name="name">The users name</param>
        /// <returns>Boolean according to validation</returns>
        public static bool IsValidName(string name)
        {

            // Validate empty value
            if (name.Trim().Length == 0) return false;


            // Validate pattern
            if (!IsValidRegex(name, ContentType.NAME)) return false;


            // Is valid
            return true;

        }

        #endregion

        #region IsValidLicensePlate

        /// <summary>
        /// Method to validate the license plate insert by user
        /// </summary>
        /// <param name="licenseplate">The users license plate</param>
        /// <returns></returns>
        public static bool IsvalidLicensePlate(string licenseplate)
        {


            // Validate empty value
            if (licenseplate.Trim().Length == 0) return false;


            // Validate pattern
            if (!IsValidRegex(licenseplate, ContentType.LICENSEPLATE)) return false;;

            // Is valid
            return true;

        }

        #endregion

        #region IsValidRegex

        public static bool IsValidRegex(string content, ContentType contentType)
        {
            // Local variables
            Match match;
            string pattern = "";
            
            

            // Types of content
            switch (contentType)
            {
                
                case ContentType.EMAIL:
                    pattern = @"^$|^([a-z0-9_.-]+)@([\da-z\.-]+)\.([a-z\.]{2,8})$";
                    break;
                case ContentType.PASSWORD:
                    pattern = "";
                    break;
                case ContentType.NAME:
                    pattern = "";
                    break;
                case ContentType.LICENSEPLATE:
                    pattern = @"([A-Z0-9]){2}-([A-Z0-9]){2}-([A-Z0-9]){2}$";
                    break;
                default:
                    break;

            }

            Regex regex = new Regex(pattern);

            // Validate if content respects patttern
            match = regex.Match(content);

            // Return result
            return match.Success;

        }

        #endregion

    }

}

#endregion
