using BasicDependencyInjection.Enums;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BasicDependencyInjection
{
    public interface IBasicContainer
    {
        void SetDefaultScope(Scope scope);
        void Register<TInterface, TImplementation>() where TImplementation : class, TInterface;
        void Register<T>() where T : class;
        void Register(Type type);
        void Register<TInterface, TImplementation>(Scope scope) where TImplementation : class, TInterface;
        void Register<T>(Scope scope) where T : class;
        void Register(Type type, Scope scope);
        void RegisterSingleton<T>(T obj) where T : class;
        void RegisterMany<T>(Assembly assembly) where T : class;
        void RegisterMany<T>(Assembly assembly, Scope scope) where T : class;
        void RegisterMany<T>(IEnumerable<Assembly> assemblies) where T : class;
        void RegisterMany<T>(IEnumerable<Assembly> assemblies, Scope scope) where T : class;
        T Get<T>() where T : class;
        void Verify();
    }
}
