#region Usings

using System.Text;
using MSUsers.Exceptions;
using System.Security.Cryptography;

#endregion

#region Configs Class

namespace MSUsers.Configurations
{

    public class Configs
    {

        // State variables
        public static int tokenExpirationMinutes = 120;
        private static string connectionString = null;
        private static string encryptionKey = "5468576D5A7134743777217A24432646";


        #region ConnectionString Region

        /// <summary>
        /// Connection string for database 
        /// </summary>
        public static string ConnectionString
        {

            get
            {

                if (connectionString is null) throw new MSUsersException(ExceptionsDetails.SETTINGS_ELEMENT_UNDEFINED);
                return connectionString;

            }

            set
            {

                if (!string.IsNullOrEmpty(connectionString)) throw new MSUsersException(ExceptionsDetails.SETTINGS_ELEMENT_ALREADY_DEFINED);
                connectionString = value;

            }

        }

        /// <summary>
        /// Define configurations
        /// </summary>
        public static void DefineSettingsWithConfig(IConfiguration configuration)
        {

            ConnectionString = configuration.GetConnectionString("MSUsersConnectionString");

        }

        #endregion

        #region Encryption Region

        #region Encyption

        /// <summary>
        /// Method used to encrypt password
        /// </summary>
        /// <param name="password">Password</param>
        /// <returns>Password encrypted</returns>
        public static string Encrypt(string password)
        {

            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {

                aes.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                aes.IV = iv;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {

                    using (CryptoStream cryptoStream = new CryptoStream((Stream)ms, encryptor, CryptoStreamMode.Write))
                    {

                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {

                            streamWriter.Write(password);
                        }

                        array = ms.ToArray();
                    }

                }

            }

            return Convert.ToBase64String(array);

        }

        #endregion

        #region Decrypt

        /// <summary>
        /// Method used to decrypt password
        /// </summary>
        /// <param name="password">Password</param>
        /// <returns>Password decrypted</returns>
        public static string Decrypt(string password)
        {

            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(password);

            using (Aes aes = Aes.Create())
            {

                aes.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream(buffer))
                {

                    using (CryptoStream cryptoStream = new CryptoStream((Stream)ms, decryptor, CryptoStreamMode.Read))
                    {

                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {

                            return streamReader.ReadToEnd();

                        }

                    }

                };

            }

        }

        #endregion

        public static string EncryptionKey
        {

            get { return encryptionKey; }

            set { encryptionKey = value; }

        }

        #endregion

    }

}

#endregion
