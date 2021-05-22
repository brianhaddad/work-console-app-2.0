using System;

namespace BasicDependencyInjection.Exceptions
{
    public class AlreadyRegisteredException : Exception
    {
        public AlreadyRegisteredException(string message) : base(message) { }
    }
}
