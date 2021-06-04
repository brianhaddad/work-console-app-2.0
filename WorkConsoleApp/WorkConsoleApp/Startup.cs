using BasicDependencyInjection;
using BasicDependencyInjection.Container;
using BasicDependencyInjection.Enums;
using BasicDependencyInjection.Exceptions;
using ConsoleDraw.Components;
using ConsoleDraw.Components.Layout;
using ConsoleDraw.Components.Text;
using ConsoleDraw.DoubleBuffer;
using ConsoleDraw.Services;
using System;
using WorkConsoleApp.DependencyInjection;

namespace WorkConsoleApp
{
    public static class Startup
    {
        public static IBasicContainer BuildContainer()
        {
            var container = new BasicContainer();

            container.Register<IComponentFactory, ComponentFactory>(Scope.Singleton);
            container.Register<IComponentBuilder, ComponentBuilder>(Scope.Singleton);
            RegisterComponents(container);

            container.Register<IConsoleBuffer, TextRenderBuffer>(Scope.Singleton);

            try
            {
                container.Verify();
            }
            catch(ContainerVerificationException e)
            {
                Console.WriteLine($"Verification error: {e.Message}");
                Console.WriteLine(e.InnerException.ToString());
            }

            return container;
        }

        private static void RegisterComponents(IBasicContainer container)
        {
            //TODO: use discovery to find and register all implementations of IComponent
            //Or modify the container to register based on an interface or base class they all implement
            //and allow a collection IEnumerable<T> be requested/injected where T is the shared interface.
            container.Register<TextOutput>(Scope.PerRequest);
            container.Register<VerticalLayout>(Scope.PerRequest);
        }
    }
}
