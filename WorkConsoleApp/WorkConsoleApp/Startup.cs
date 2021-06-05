using BasicDependencyInjection;
using BasicDependencyInjection.Container;
using BasicDependencyInjection.Enums;
using BasicDependencyInjection.Exceptions;
using ConsoleDraw.Components;
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
            container.RegisterMany<IComponent>(typeof(BaseComponent).Assembly, Scope.Transient);

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
    }
}
