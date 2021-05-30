using ConsoleDraw.Components.ComponentStyling;
using System;

namespace ConsoleDraw.Components
{
    public interface IComponentFactory
    {
        T MakeComponent<T>() where T : class, IComponent;
        T MakeComponent<T>(Action<ComponentStyleAndLayout> styleSetup) where T : class, IComponent;
        T MakeComponent<T>(Action<T> componentSetup) where T : class, IComponent;
        T MakeComponent<T>(Action<T> componentSetup, Action<ComponentStyleAndLayout> styleSetup) where T : class, IComponent;
    }
}
