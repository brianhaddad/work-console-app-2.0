using BasicDependencyInjection.Enums;
using System;

namespace BasicDependencyInjection
{
    public interface IBasicContainer
    {
        void SetDefaultScope(Scope scope);
        void Register<TInterface, TImplementation>() where TImplementation : TInterface;
        void Register<T>();
        void Register(Type type);
        void Register<TInterface, TImplementation>(Scope scope) where TImplementation : TInterface;
        void Register<T>(Scope scope);
        void Register(Type type, Scope scope);
        //TODO: make a RegisterMany<T>(IEnumerable<Assembly> assemblies) and some overloads (only one assembly, with scope, etc.)
        //This will scan the assembly for anything that implements the interface or base class.
        //Must not register "concretes" that are abstract or interfaces though.
        T Get<T>() where T : class;
        void Verify();
    }
}
