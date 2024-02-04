#region Usings

using MSPayments.SQL;
using MSPayments.Exceptions;
using MSPayments.Database_Methods;

#endregion

#region MSPaymentsEngine

namespace MSPayments.Engine
{
    public class MSPaymentsEngine
    {

        // State variables
        private SQLServer sqlServer;
        private PaymentMethod paymentMethod;
        protected MSPaymentsEngine mSUsersEngine;
        private readonly string connectionString;

        public MSPaymentsEngine(string connectionString)
        {

            this.connectionString = connectionString ?? throw new MSPaymentsException(ExceptionsDetails.FORBIDDEN_EMPTY_CONNECTIONSTRING);

        }

        public PaymentMethod PaymentMethod
        {

            get
            {

                // If first time use than create object
                if (paymentMethod is null)
                    paymentMethod = new PaymentMethod(this);

                // Return object
                return paymentMethod;

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
