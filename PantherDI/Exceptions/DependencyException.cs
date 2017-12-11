using System;

namespace PantherDI.Exceptions
{
    public class DependencyException : Exception
    {
        protected DependencyException() { }

        public DependencyException(string message) : base(message)
        {
        }
    }
}