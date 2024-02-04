#region Usings

using MSRepairs.SQL;
using MSRepairs.Exceptions;
using MSRepairs.Database_Methods;

#endregion

#region MSRepairEngine

namespace MSRepairs.Engine
{

    public class MSRepairsEngine
    {

        // State variables
        private SQLServer sqlServer;
        private RepairMethod repairMethod;
        private StockMethod stockMethod;
        private PartMethod partsMethod;
        protected MSRepairsEngine mSRepairsEngine;
        private readonly string connectionstring;

        public MSRepairsEngine(string connectionString)
        {

            this.connectionstring = connectionString ?? throw new MSRepairsException(ExceptionsDetails.FORBIDDEN_EMPTY_CONNECTIONSTRING);

        }

        public RepairMethod RepairMethod
        {

            get
            {

                // If first time use than create object
                if (repairMethod is null)
                    repairMethod = new RepairMethod(this);

                // Return object
                return repairMethod;

            }

        }


        public PartMethod PartsMethod
        {

            get
            {

                // If first time use than create object
                if (partsMethod is null)
                    partsMethod = new PartMethod(this);

                // Return object
                return partsMethod;

            }

        }

        public StockMethod StockMethod
        {

            get
            {

                // If first time use than create object
                if (stockMethod is null)
                    stockMethod = new StockMethod(this);

                // Return object
                return stockMethod;

            }

        }



        public SQLServer SQLServer
        {

            get
            {

                // if first time use then create object
                if (sqlServer is null)
                    sqlServer = new SQLServer(this.connectionstring, SQLServer.SQLConnectionModes.manual);

                // return object
                return sqlServer;

            }

        }

    }

}

#endregion
