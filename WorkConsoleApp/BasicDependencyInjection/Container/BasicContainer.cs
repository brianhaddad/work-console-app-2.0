using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BasicDependencyInjection.Enums;
using BasicDependencyInjection.Exceptions;

namespace BasicDependencyInjection.Container
{
    public class BasicContainer : IBasicContainer
    {
        //TODO: dictionary of FUNC to contain instantiating methods? Is this even possible?
        private readonly Dictionary<Type, Type> ConcreteTypeLookup = new Dictionary<Type, Type>();
        private readonly Dictionary<Type, IEnumerable<Type>> CollectionTypeLookup = new Dictionary<Type, IEnumerable<Type>>();
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

        public void Register<TInterface, TImplementation>(Scope scope) where TImplementation : class, TInterface
            => Register(typeof(TInterface), typeof(TImplementation), scope);
        public void Register<TInterface, TImplementation>() where TImplementation : class, TInterface
            => Register<TInterface, TImplementation>(DefaultScope);

        public void Register<T>(Scope scope) where T : class => Register<T, T>(scope);
        public void Register<T>() where T : class => Register<T>(DefaultScope);

        public void Register(Type type, Scope scope) => Register(type, type, scope);
        public void Register(Type type) => Register(type, DefaultScope);

        public void RegisterMany<T>(Assembly assembly) where T : class
            => RegisterMany<T>(assembly, DefaultScope);
        public void RegisterMany<T>(Assembly assembly, Scope scope) where T : class
            => RegisterMany<T>(new[] { assembly }, scope);
        public void RegisterMany<T>(IEnumerable<Assembly> assemblies) where T : class
            => RegisterMany<T>(assemblies, DefaultScope);
        public void RegisterMany<T>(IEnumerable<Assembly> assemblies, Scope scope) where T : class
            => RegisterMany(typeof(T), assemblies, scope);

        public void RegisterSingleton<T>(T obj) where T : class
        {
            var t = typeof(T);
            PreRegistrationCheck(t.FullName);
            Singletons.Add(t, obj);
            ScopeLookup.Add(t, Scope.Singleton);
        }

        private void RegisterMany(Type interfaceType, IEnumerable<Assembly> assemblies, Scope scope)
        {
            PreRegistrationCheck(interfaceType.FullName);

            var enumerableType = typeof(IEnumerable<>);
            var typeToRegister = enumerableType.MakeGenericType(interfaceType);
            var registrations = new List<Type>();

            foreach (var assembly in assemblies)
            {
                var found = assembly.GetTypes().Where(t => !(t.IsInterface || t.IsAbstract) && t.GetInterfaces().Contains(interfaceType));
                foreach (var t in found)
                {
                    Register(t, scope);
                }
                registrations.AddRange(found);
            }

            if (!registrations.Any())
            {
                throw new NoImplementationsFoundException($"No implementations of {interfaceType.FullName} found in the provided assemblies.");
            }

            CollectionTypeLookup.Add(typeToRegister, registrations);
            ScopeLookup.Add(typeToRegister, scope);
        }

        private void Register(Type interfaceType, Type implementationType, Scope scope)
        {
            PreRegistrationCheck(interfaceType.FullName);

            if (ConcreteTypeLookup.ContainsKey(interfaceType))
            {
                throw new AlreadyRegisteredException($"{interfaceType.FullName} has already been registered. Did you want to use RegisterMany?");
            }
            ConcreteTypeLookup.Add(interfaceType, implementationType);
            ScopeLookup.Add(interfaceType, scope);
        }

        private void PreRegistrationCheck(string typeName)
        {
            if (Verified || Verifying)
            {
                throw new AlreadyVerifiedException($"Illegal operation: Attempted to register {typeName} after verification.");
            }

            Registering = true;
        }

        private object Create(Type type)
        {
            if (ConcreteTypeLookup.ContainsKey(type))
            {
                var concreteType = ConcreteTypeLookup[type];
                //TODO: Instead of just a default first constructor can I analyze them somehow? Would there be any benefit?
                var firstConstructor = concreteType.GetConstructors()[0];
                var constructorParameters = firstConstructor.GetParameters();
                var parameters = constructorParameters.Select(param => GetObject(param.ParameterType)).ToArray();
                var obj = firstConstructor.Invoke(parameters);
                return obj;
            }
            if (CollectionTypeLookup.ContainsKey(type))
            {
                var subtype = type.GetGenericArguments().First();
                var makeCollection = GetType().GetMethod(nameof(BasicContainer.MakeTypeCollection), BindingFlags.NonPublic | BindingFlags.Instance);
                var makeGeneric = makeCollection.MakeGenericMethod(new[] { type, subtype });
                var result = makeGeneric.Invoke(this, null);
                return result;
            }
            throw new NoMatchingRegistrationException($"{type.FullName} not registered.");
        }

        private T MakeTypeCollection<T, ST>() where T : class where ST : class
        {
            var objs = new List<ST>();
            var type = typeof(T);
            foreach (var t in CollectionTypeLookup[type])
            {
                objs.Add(GetObject<ST>(t));
            }
            return objs as T;
        }

        private object GetObject(Type t)
        {
            var get = GetType().GetMethod(nameof(BasicContainer.Get));
            var getGeneric = get.MakeGenericMethod(t);
            return getGeneric.Invoke(this, null);
        }

        private T GetObject<T>(Type t) where T : class => GetObject(t) as T;

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
                scope = Scope.Transient;
            }

            switch (scope)
            {
                case Scope.Singleton:
                    if (!Singletons.ContainsKey(t))
                    {
                        Singletons.Add(t, Create(t));
                    }
                    return Singletons[t] as T;

                case Scope.Transient:
                    var result = Create(t);
                    return result as T;

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
            Type currentType = null;
            try
            {
                VerifyConcreteTypes(ref currentType);
                //Verifying the collection types might be a bit redundant since each element is also registered in the concrete types.
                //But what can I say? I'm very thorough and since verification only happens once I'm not worried about optimizations.
                //It also serves to verify that nothing was registered that I can't resolve. :)
                VerifyCollectionTypes(ref currentType);
            }
            catch (Exception e)
            {
                throw new ContainerVerificationException($"Something went wrong while registering {currentType.FullName}.", e, currentType);
            }
            Verifying = false;
            Verified = true;
        }

        private void VerifyConcreteTypes(ref Type currentType)
        {
            currentType = ConcreteTypeLookup.Keys.ToArray()[0];
            foreach (var kvp in ConcreteTypeLookup)
            {
                if (!Singletons.ContainsKey(kvp.Key))
                {
                    currentType = kvp.Key;
                    var result = Create(kvp.Key);
                }
            }
        }

        private void VerifyCollectionTypes(ref Type currentType)
        {
            currentType = CollectionTypeLookup.Keys.ToArray()[0];
            foreach (var kvp in CollectionTypeLookup)
            {
                currentType = kvp.Key;
                var result = Create(kvp.Key);
            }
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
