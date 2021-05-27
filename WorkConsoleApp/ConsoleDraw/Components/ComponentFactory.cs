using BasicDependencyInjection;
using System;

namespace ConsoleDraw.Components
{
    public class ComponentFactory : IComponentFactory
    {
        private readonly IBasicContainer Container;

        public ComponentFactory(IBasicContainer container)
        {
            Container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public T MakeComponent<T>() where T : class, IComponent
            => Container.Create<T>();
    }
}
