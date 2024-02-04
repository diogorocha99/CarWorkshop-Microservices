using System.Text.RegularExpressions;
using static MSGarages.Tools.MSGaragesEnums;

namespace MSGarages.Tools
{
    public class MSGaragesValidator
    {
        public static bool IsValidPostalCode(string postalcode)
        {

            // Validate empty value
            if (postalcode.Trim().Length == 0) return false;


            // Validate pattern
            if (!IsValidRegex(postalcode, ContentType.POSTAL_CODE)) return false;


            // Is valid
            return true;

        }


        #region IsValidRegex

        public static bool IsValidRegex(string content, ContentType contentType)
        {
            // Local variables
            Match match;
            string pattern = "";



            // Types of content
            if(contentType == ContentType.POSTAL_CODE)
            {
                pattern = @"[0-9]{4}-[0-9]{3}$";
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
