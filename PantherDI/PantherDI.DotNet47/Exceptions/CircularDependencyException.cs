using System;
using System.Runtime.Serialization;

namespace PantherDI.Exceptions
{
    public class CircularDependencyException : DependencyException
    {
        public CircularDependencyException()
        {
        }

        public CircularDependencyException(string message) : base(message)
        {
        }

        public CircularDependencyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CircularDependencyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}