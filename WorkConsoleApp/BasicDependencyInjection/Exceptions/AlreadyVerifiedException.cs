using System;

namespace BasicDependencyInjection.Exceptions
{
    public class AlreadyVerifiedException : Exception
    {
        public AlreadyVerifiedException(string message) : base(message)
        {
        }
    }
}
