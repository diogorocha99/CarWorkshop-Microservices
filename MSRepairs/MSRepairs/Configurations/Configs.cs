#region Usings

using MSRepairs.Exceptions;

#endregion

#region Configs Class

namespace MSRepairs.Configurations
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

                if (connectionString is null) throw new MSRepairsException(ExceptionsDetails.SETTINGS_ELEMENT_UNDEFINED);
                return connectionString;

            }

            set
            {

                if (!string.IsNullOrEmpty(connectionString)) throw new MSRepairsException(ExceptionsDetails.SETTINGS_ELEMENT_ALREADY_DEFINED);
                connectionString = value;

            }

        }

        /// <summary>
        /// Define configurations
        /// </summary>
        public static void DefineSettingsWithConfig(IConfiguration configuration)
        {

            ConnectionString = configuration.GetConnectionString("MSRepairsConnectionString");

        }

        #endregion

    }
}

#endregion
