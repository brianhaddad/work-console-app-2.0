using System;

namespace BasicDependencyInjection.Exceptions
{
    public class ContainerVerificationException : Exception
    {
        public Type Type { get; set; }
        public ContainerVerificationException(string message, Exception innerException, Type type) : base(message, innerException)
        {
            Type = type;
        } 
    }
}
