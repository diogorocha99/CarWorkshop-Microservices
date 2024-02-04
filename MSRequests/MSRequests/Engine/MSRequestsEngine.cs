#region Usings

using MSRequests.SQL;
using MSRequests.Exceptions;
using MSRequests.Database_Methods;

#endregion

#region MSRequestsEngine

namespace MSRequests.Engine
{
    public class MSRequestsEngine
    {

        // State variables
        private SQLServer sqlServer;
        private RequestMethod requestMethod;
        protected MSRequestsEngine mSUsersEngine;
        private readonly string connectionString;

        public MSRequestsEngine(string connectionString)
        {

            this.connectionString = connectionString ?? throw new MSRequestsException(ExceptionsDetails.FORBIDDEN_EMPTY_CONNECTIONSTRING);

        }

        public RequestMethod RequestMethod
        {

            get
            {

                // If first time use than create object
                if (requestMethod is null)
                    requestMethod = new RequestMethod(this);

                // Return object
                return requestMethod;

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
