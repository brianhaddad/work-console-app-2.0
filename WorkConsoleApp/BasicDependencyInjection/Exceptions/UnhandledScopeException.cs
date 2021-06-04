using BasicDependencyInjection.Enums;
using System;

namespace BasicDependencyInjection.Exceptions
{
    public class UnhandledScopeException : Exception
    {
        public UnhandledScopeException(Scope scope)
            : base($"The {scope.ToString()} scope is unhandled at this time.")
        {
        }
    }
}
