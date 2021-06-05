using System;

namespace BasicDependencyInjection.Exceptions
{
    public class NoImplementationsFoundException : Exception
    {
        public NoImplementationsFoundException(string message) : base(message)
        {
        }
    }
}
