namespace MSGarages.Exceptions
{
    public class MSGaragesException : Exception
    {

        /// <summary>
        /// MSUsersExceptions constructor
        /// </summary>
        /// <param name="mSUsersExceptions">Customized exception</param>
        /// <exception cref="Exception">Exception thrown</exception>
        public MSGaragesException(string mSGaragessExceptions)
        {

            throw new Exception(mSGaragessExceptions);

        }

    }

}
