using System;

namespace Taskboard.Commands.Exceptions
{
    public sealed class DataAccessException : Exception
    {
        private const string message = "Unexpected data access error. See inner exception for details.";

        public DataAccessException()
        {
        }

        public DataAccessException(string message) : base(message)
        {
        }

        public DataAccessException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public static DataAccessException FromInnerException(Exception innerException)
        {
            return new DataAccessException(message, innerException);
        }
    }
}