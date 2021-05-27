﻿using System;
using System.Collections.Generic;
using System.Linq;
using BasicDependencyInjection.Exceptions;

namespace BasicDependencyInjection.Container
{
    public class BasicContainer : IBasicContainer
    {
        private readonly Dictionary<Type, Type> ConcreteTypeLookup = new Dictionary<Type, Type>();
        private readonly Dictionary<Type, object> TypeObjects = new Dictionary<Type, object>();
        private bool Verified = false;
        private bool Verifying = false;

        public BasicContainer()
        {
            Register(typeof(IBasicContainer), GetType());
            TypeObjects.Add(typeof(IBasicContainer), this);
        }

        public void Register<TInterface, TImplementation>() where TImplementation : TInterface
            => Register(typeof(TInterface), typeof(TImplementation));
        public void Register<T>() => Register<T, T>();
        public void Register(Type type) => Register(type, type);

        private void Register(Type interfaceType, Type implementationType)
        {
            if (ConcreteTypeLookup.ContainsKey(interfaceType))
            {
                throw new AlreadyRegisteredException($"{interfaceType.FullName} has already been registered.");
            }
            ConcreteTypeLookup.Add(interfaceType, implementationType);
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
            if (!TypeObjects.ContainsKey(t))
            {
                TypeObjects.Add(t, Create(t));
            }
            return TypeObjects[t] as T;
        }


        public T Create<T>() where T : class
            => (T)Create(typeof(T));

        public void Verify()
        {
            Verifying = true;
            var currentType = "";
            try
            {
                foreach (var kvp in ConcreteTypeLookup)
                {
                    if (!TypeObjects.ContainsKey(kvp.Key))
                    {
                        currentType = kvp.Key.FullName;
                        var result = Create(kvp.Key);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Something went wrong while registering {currentType}.");
                Console.WriteLine(e);
            }
            Verifying = false;
            Verified = true;
        }
    }
}
