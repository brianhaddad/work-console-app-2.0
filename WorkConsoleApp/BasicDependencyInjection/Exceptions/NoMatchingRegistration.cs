using System;

namespace BasicDependencyInjection.Exceptions
{
    public class NoMatchingRegistration : Exception
    {
        public NoMatchingRegistration(string message) : base(message)
        {
        }
    }
}
