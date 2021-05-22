using System;

namespace BasicDependencyInjection.Exceptions
{
    public class NoMatchingRegistrationException : Exception
    {
        public NoMatchingRegistrationException(string message) : base(message) { }
    }
}
