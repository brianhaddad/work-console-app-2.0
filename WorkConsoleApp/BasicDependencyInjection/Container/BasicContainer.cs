using System;
using System.Collections.Generic;
using System.Linq;
using BasicDependencyInjection.Enums;
using BasicDependencyInjection.Exceptions;

namespace BasicDependencyInjection.Container
{
    public class BasicContainer : IBasicContainer
    {
        private readonly Dictionary<Type, Type> ConcreteTypeLookup = new Dictionary<Type, Type>();
        private readonly Dictionary<Type, Scope> ScopeLookup = new Dictionary<Type, Scope>();
        private readonly Dictionary<Type, object> Singletons = new Dictionary<Type, object>();
        private Scope DefaultScope = Scope.Singleton;
        private bool Verified = false;
        private bool Verifying = false;
        private bool Registering = false;

        public BasicContainer()
        {
            Register(typeof(IBasicContainer), GetType(), Scope.Singleton);
            Singletons.Add(typeof(IBasicContainer), this);
        }

        public void Register<TInterface, TImplementation>(Scope scope) where TImplementation : TInterface
            => Register(typeof(TInterface), typeof(TImplementation), scope);
        public void Register<TInterface, TImplementation>() where TImplementation : TInterface
            => Register<TInterface, TImplementation>(DefaultScope);

        public void Register<T>(Scope scope) => Register<T, T>(scope);
        public void Register<T>() => Register<T>(DefaultScope);

        public void Register(Type type, Scope scope) => Register(type, type, scope);
        public void Register(Type type) => Register(type, DefaultScope);

        private void Register(Type interfaceType, Type implementationType, Scope scope)
        {
            if (Verified || Verifying)
            {
                throw new AlreadyVerifiedException($"Illegal operation: Attempted to register {interfaceType.FullName} after verification.");
            }

            Registering = true;

            if (ConcreteTypeLookup.ContainsKey(interfaceType))
            {
                throw new AlreadyRegisteredException($"{interfaceType.FullName} has already been registered.");
            }
            ConcreteTypeLookup.Add(interfaceType, implementationType);
            ScopeLookup.Add(interfaceType, scope);
        }

        private object Create(Type type)
        {
            if (!ConcreteTypeLookup.ContainsKey(type))
            {
                throw new NoMatchingRegistrationException($"{type.FullName} not registered.");
            }
            //TODO: Does this work with generics?!?
            var concreteType = ConcreteTypeLookup[type];
            //TODO: Instead of just a default first constructor can I analyze them somehow?
            var firstConstructor = concreteType.GetConstructors()[0];
            //TODO: Verify if the default constructor requires parameters?
            var constructorParameters = firstConstructor.GetParameters();
            //TODO: Some parameters might not be classes that need to be instantiated.
            //Can we detect these and look in a separate collection where factories or generators have been registered?
            //HOW TO HANDLE?!? :)
            var get = GetType().GetMethod(nameof(BasicContainer.Get));
            var parameters = constructorParameters.Select((param) => {
                var getGeneric = get.MakeGenericMethod(param.ParameterType);
                return getGeneric.Invoke(this, null);
            }).ToArray();
            var obj = firstConstructor.Invoke(parameters);
            return obj;
        }

        public T Get<T>() where T : class
        {
            if (!Verified && !Verifying)
            {
                throw new UnverifiedContainerException("You must verify the container in order to use it.");
            }
            var t = typeof(T);

            var scope = ScopeLookup.ContainsKey(t) ? ScopeLookup[t] : DefaultScope;
            if (Verifying)
            {
                scope = Scope.PerRequest;
            }

            switch (scope)
            {
                case Scope.Singleton:
                    if (!Singletons.ContainsKey(t))
                    {
                        Singletons.Add(t, Create(t));
                    }
                    return Singletons[t] as T;

                case Scope.PerRequest:
                    return Create(t) as T;

                default:
                    throw new UnhandledScopeException(scope);
            }
        }

        /// <summary>
        /// Verification can be performed once to ensure that every dependency requested can be resolved.
        /// </summary>
        public void Verify()
        {
            if (Verified || Verifying)
            {
                throw new AlreadyVerifiedException("Cannot verify more than once.");
            }

            Verifying = true;
            var currentType = ConcreteTypeLookup.Keys.ToArray()[0];
            try
            {
                foreach (var kvp in ConcreteTypeLookup)
                {
                    if (!Singletons.ContainsKey(kvp.Key))
                    {
                        currentType = kvp.Key;
                        var result = Create(kvp.Key);
                    }
                }
            }
            catch (Exception e)
            {
                throw new ContainerVerificationException($"Something went wrong while registering {currentType.FullName}.", e, currentType);
            }
            Verifying = false;
            Verified = true;
        }

        /// <summary>
        /// Here you can change the default scope for the container.
        /// The default scope cannot be changed after the first registration, and certainly not after verification.
        /// </summary>
        /// <param name="scope"></param>
        public void SetDefaultScope(Scope scope)
        {
            if (Verified || Verifying)
            {
                throw new AlreadyVerifiedException("Cannot change the default scope after verification.");
            }
            if (Registering)
            {
                throw new IllegalContainerModificationException("Cannot change the default scope after the first registrations have been done.");
            }
            DefaultScope = scope;
        }
    }
}
