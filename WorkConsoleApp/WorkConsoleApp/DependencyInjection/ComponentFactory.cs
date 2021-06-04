using BasicDependencyInjection;
using ConsoleDraw.Components;
using ConsoleDraw.Components.ComponentStyling;
using System;

namespace WorkConsoleApp.DependencyInjection
{
    public class ComponentFactory : IComponentFactory
    {
        private readonly IBasicContainer Container;

        public ComponentFactory(IBasicContainer container)
        {
            Container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public T MakeComponent<T>() where T : class, IComponent
            => Container.Get<T>();

        public T MakeComponent<T>(Action<ComponentStyleAndLayout> styleSetup) where T : class, IComponent
        {
            var component = MakeComponent<T>();
            component.ConfigureComponentLayout(styleSetup);
            return component;
        }

        public T MakeComponent<T>(Action<T> componentSetup) where T : class, IComponent
        {
            var component = MakeComponent<T>();
            componentSetup(component);
            return component;
        }

        public T MakeComponent<T>(Action<T> componentSetup, Action<ComponentStyleAndLayout> styleSetup) where T : class, IComponent
        {
            var component = MakeComponent<T>(styleSetup);
            componentSetup(component);
            return component;
        }
    }
}
