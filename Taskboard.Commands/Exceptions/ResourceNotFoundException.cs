using System;

namespace Taskboard.Commands.Exceptions
{
    public sealed class ResourceNotFoundException : Exception
    {
        private const string message = "Unable to find resource with id {0}.";

        public ResourceNotFoundException()
        {
        }

        public ResourceNotFoundException(string message) : base(message)
        {
        }

        public ResourceNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public static ResourceNotFoundException FromResourceId(string id)
        {
            return new ResourceNotFoundException(string.Format(message, id));
        }
    }
}