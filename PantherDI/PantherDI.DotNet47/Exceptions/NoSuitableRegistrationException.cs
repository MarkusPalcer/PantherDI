using System;
using System.Runtime.Serialization;

namespace PantherDI.Exceptions
{
    public class NoSuitableRegistrationException : DependencyException
    {
        public NoSuitableRegistrationException()
        {
        }

        public NoSuitableRegistrationException(string message) : base(message)
        {
        }

        public NoSuitableRegistrationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoSuitableRegistrationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}