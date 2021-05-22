using System;

namespace BasicDependencyInjection.Exceptions
{
    public class UnverifiedContainerException : Exception
    {
        public UnverifiedContainerException(string message) : base(message) { }
    }
}
