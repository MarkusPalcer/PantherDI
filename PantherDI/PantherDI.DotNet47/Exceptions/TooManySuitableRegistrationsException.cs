using System;
using System.Runtime.Serialization;

namespace PantherDI.Exceptions
{
    public class TooManySuitableRegistrationsException : DependencyException
    {
        public TooManySuitableRegistrationsException()
        {
        }

        public TooManySuitableRegistrationsException(string message) : base(message)
        {
        }

        public TooManySuitableRegistrationsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TooManySuitableRegistrationsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}