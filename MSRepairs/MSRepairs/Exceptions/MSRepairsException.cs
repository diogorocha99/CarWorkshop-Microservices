#region MSRepairsException Class

namespace MSRepairs.Exceptions
{
    public class MSRepairsException : Exception
    {
        /// <summary>
        /// MSRepairsExceptions constructor
        /// </summary>
        /// <param name="mSPaymentsExceptions">Customized exception</param>
        /// <exception cref="Exception">Exception thrown</exception>
        public MSRepairsException(string msRepairsExceptions)
        {

            throw new Exception(msRepairsExceptions);

        }

    }
}

#endregion
