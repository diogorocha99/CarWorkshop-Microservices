#region MSPaymentsException Class

namespace MSPayments.Exceptions
{

    public class MSPaymentsException : Exception
    {

        /// <summary>
        /// MSPaymentsExceptions constructor
        /// </summary>
        /// <param name="mSPaymentsExceptions">Customized exception</param>
        /// <exception cref="Exception">Exception thrown</exception>
        public MSPaymentsException(string mSPaymentsExceptions) 
        {

            throw new Exception(mSPaymentsExceptions);

        }

    }

}

#endregion
