using MSGarages.Exceptions;
using System.Text;

namespace MSGarages.Configurations
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

                if (connectionString is null) throw new MSGaragesException(ExceptionsDetails.SETTINGS_ELEMENT_UNDEFINED);
                return connectionString;

            }

            set
            {

                if (!string.IsNullOrEmpty(connectionString)) throw new MSGaragesException(ExceptionsDetails.SETTINGS_ELEMENT_ALREADY_DEFINED);
                connectionString = value;

            }

        }

        /// <summary>
        /// Define configurations
        /// </summary>
        public static void DefineSettingsWithConfig(IConfiguration configuration)
        {

            ConnectionString = configuration.GetConnectionString("MSGaragesConnectionString");

        }

        #endregion
    }
}
