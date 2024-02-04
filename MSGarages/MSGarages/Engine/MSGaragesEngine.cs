using MSGarages.Database_Methods;
using MSGarages.Exceptions;
using MSGarages.SQL;

namespace MSGarages.Engine
{
    public class MSGaragesEngine
    {
        // State variables
        private SQLServer sqlServer;
        private GarageMethod garageMethod;
        protected MSGaragesEngine mSUsersEngine;
        private readonly string connectionString;

        public MSGaragesEngine(string connectionString)
        {

            this.connectionString = connectionString ?? throw new MSGaragesException(ExceptionsDetails.FORBIDDEN_EMPTY_CONNECTIONSTRING);

        }


        public GarageMethod GarageMethod
        {

            get
            {

                // If first time use than create object
                if (garageMethod is null)
                    garageMethod = new GarageMethod(this);

                // Return object
                return garageMethod;

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
