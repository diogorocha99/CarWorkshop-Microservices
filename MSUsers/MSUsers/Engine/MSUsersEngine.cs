#region Usings

using MSUsers.SQL;
using MSUsers.Tools;
using MSUsers.Models;
using MSUsers.Exceptions;
using MSUsers.Database_Methods;

#endregion

#region MSUsersEngine

namespace MSUsers.Engine
{
    public class MSUsersEngine
    {

        // State variables
        private SQLServer sqlServer;
        private UserMethod userMethod;
        private User authenticatedUser;
        private VehicleMethod vehicleMethod;
        protected MSUsersEngine mSUsersEngine;
        private readonly string connectionString;
        private AuthenticationMethod authenticationMethod;

        public MSUsersEngine(string connectionString)
        {

            this.connectionString = connectionString ?? throw new MSUsersException(ExceptionsDetails.FORBIDDEN_EMPTY_CONNECTIONSTRING);
        
        }

        public bool Login(string email, string password)
        {

            // Validate if username or password are not empty
            if (!MSUsersValidators.IsValidEmail(email) || !MSUsersValidators.IsValidPassword(password))
                throw new MSUsersException(ExceptionsDetails.AUTHENTICATION_INVALID_CREDENTIALS);


            if (!this.AuthenticationMethod.LoginGetCount(email, password))
            {

                this.authenticatedUser = null;
                return false;
            
            }

            this.authenticatedUser = this.UserMethod.GetUserInformationsLogin(email);

            // Return authentication sucess
            return true;

        }

        public User AuthenticatedUser
        {

            get { return authenticatedUser; }

        }

        public VehicleMethod VehicleMethod
        {

            get
            {

                // If first time use than create object
                if (vehicleMethod is null)
                    vehicleMethod = new VehicleMethod(this);

                // Return object
                return vehicleMethod;

            }

        }

        public UserMethod UserMethod
        {

            get
            {

                // If first time use than create object
                if (userMethod is null)
                    userMethod = new UserMethod(this);

                // Return object
                return userMethod;

            }

        }

        public AuthenticationMethod AuthenticationMethod
        {

            get
            {

                // If first time use than create object
                if (authenticationMethod is null)
                    authenticationMethod = new AuthenticationMethod(this);

                // Return object
                return authenticationMethod;

            }

        }

        public SQLServer SQLServer
        {

            get
            {

                // if first time use then create object
                if (sqlServer is null)
                    sqlServer = new SQLServer(this.connectionString, SQLServer.SQLConnectionModes.manual);

                // return object
                return sqlServer;

            }

        }

    }

}

#endregion
