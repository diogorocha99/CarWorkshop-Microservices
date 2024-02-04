#region MSUsersException Class

namespace MSUsers.Exceptions
{

    public class MSUsersException : Exception
    {

        /// <summary>
        /// MSUsersExceptions constructor
        /// </summary>
        /// <param name="mSUsersExceptions">Customized exception</param>
        /// <exception cref="Exception">Exception thrown</exception>
        public MSUsersException(string mSUsersExceptions) 
        {

            throw new Exception(mSUsersExceptions);

        }

    }

}

#endregion
