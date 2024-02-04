#region ExceptionsDetails Class

namespace MSPayments.Exceptions
{

    public class ExceptionsDetails
    {

        public static string AUTHENTICATION_INVALID_CREDENTIALS
        {

            get { return "Invalid credentials!"; }

        }

        public static string FORBIDDEN_EMPTY_CONNECTIONSTRING
        {

            get { return "Empty Connection String!"; }

        }

        public static string SPECS_PREFIX_INVALID_VALUE
        {

            get { return "Prefix Invalid Value!"; }

        }

        public static string SQLSERVER_PREFIX_INTERNAL_ERROR
        {

            get { return "SQL Internal Error!"; }

        }

        public static string OTHERS_EXCEPTION_PREFIX_ERROR
        {

            get { return "Prefix Error!"; }

        }

        public static string SETTINGS_ELEMENT_UNDEFINED
        {

            get { return "Settings Element Undefined!"; }

        }

        public static string SETTINGS_ELEMENT_ALREADY_DEFINED
        {

            get { return "Settings Element Already Defined!"; }

        }

        public static string INVALID_TOKEN
        {

            get { return "Token is Invalid!"; }

        }

        public static string SOMETHING_WENT_WRONG
        {

            get { return "Something Went Wrong!"; }

        }

        public static string ACCOUNT_ALREADY_EXISTS
        {

            get { return "Account Already Exists!"; }

        }

        public static string SOMETHING_WENT_WRONG_WHILE_DELETING_ACCOUNT
        {

            get { return "Something Went Wrong While Deleting Account!"; }

        }


        public static string SOMETHING_WENT_WRONG_WHILE_CHANGING_PASSWORD
        {

            get { return "Something Went Wrong While Changing Password!"; }

        }


        public static string WRONG_PASSWORD
        {

            get { return "Wrong Password!"; }

        }


        public static string INVALID_USER_ID_FROM_TOKEN
        {

            get { return "Invalid UserId From Token!"; }

        }


        public static string INVALID_USER_ROLE_FROM_TOKEN
        {

            get { return "Invalid UserRole From Token!"; }

        }


        public static string SQLSERVER_CONNECTIONS_NO_CONNECTION
        {

            get { return "SQLServer Prefix Internal Error!"; }

        }


        public static string ACCOUNT_IS_DISABLED
        {

            get { return "Account is Disabled!"; }

        }


        public static string LOGIN_IS_CURRENTLY_DISABLED_DUE_TRIES
        {

            get { return "Login is Temporarily Disabled!"; }

        }


        public static string SQLSERVER_DESTRUCTION_CONNECTION_STILL_OPEN
        {

            get { return "SQLServer Connection Still Open!"; }

        }

        public static string INVALID_PERMISSION
        {

            get { return "You Have No Permission!"; }

        }

        public static string SOMETHING_WENT_WRONG_WHILE_CREATING_REQUEST
        {

            get { return "Something Went Wrong While Creating Request!"; }

        }

        public static string SOMETHING_WENT_WRONG_WHILE_VALIDATING_REQUEST
        {

            get { return "Something Went Wrong While Validating Request!"; }

        }

    }

}

#endregion
