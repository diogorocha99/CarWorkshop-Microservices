#region MSRequestsException Class

namespace MSRequests.Exceptions
{

    public class MSRequestsException : Exception
    {

        /// <summary>
        /// MSRequestsExceptions constructor
        /// </summary>
        /// <param name="mSRequestsExceptions">Customized exception</param>
        /// <exception cref="Exception">Exception thrown</exception>
        public MSRequestsException(string mSRequestsExceptions) 
        {

            throw new Exception(mSRequestsExceptions);

        }

    }

}

#endregion
