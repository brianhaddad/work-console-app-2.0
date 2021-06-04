using System;

namespace BasicDependencyInjection.Exceptions
{
    public class IllegalContainerModificationException : Exception
    {
        public IllegalContainerModificationException(string message) : base(message)
        {
        }
    }
}
