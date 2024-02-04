#region Usings

using System.Text;
using MSRequests.Exceptions;
using System.Security.Cryptography;

#endregion

#region Configs Class

namespace MSRequests.Configurations
{
    public class Configs
    {

        // State variables
        private static string connectionString = null;


        #region ConnectionString Region

        /// <summary>
        /// Connection string for database 
        /// </summary>
        public static string ConnectionString
        {

            get
            {

                if (connectionString is null) throw new MSRequestsException(ExceptionsDetails.SETTINGS_ELEMENT_UNDEFINED);
                return connectionString;

            }

            set
            {

                if (!string.IsNullOrEmpty(connectionString)) throw new MSRequestsException(ExceptionsDetails.SETTINGS_ELEMENT_ALREADY_DEFINED);
                connectionString = value;

            }

        }

        /// <summary>
        /// Define configurations
        /// </summary>
        public static void DefineSettingsWithConfig(IConfiguration configuration)
        {

            ConnectionString = configuration.GetConnectionString("MSRequestsConnectionString");

        }

        #endregion

    }

}

#endregion
