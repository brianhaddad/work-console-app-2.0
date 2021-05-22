using System;

namespace BasicDependencyInjection
{
    public interface IBasicContainer
    {
        void Register<TInterface, TImplementation>() where TImplementation : TInterface;
        void Register<T>();
        void Register(Type type);
        T Get<T>() where T : class;
        void Verify();
    }
}
