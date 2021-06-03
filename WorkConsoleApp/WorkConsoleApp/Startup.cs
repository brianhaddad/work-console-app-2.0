using BasicDependencyInjection;
using BasicDependencyInjection.Container;
using ConsoleDraw.Components;
using ConsoleDraw.Components.Layout;
using ConsoleDraw.Components.Text;
using ConsoleDraw.DoubleBuffer;
using ConsoleDraw.Services;
using WorkConsoleApp.DependencyInjection;

namespace WorkConsoleApp
{
    public static class Startup
    {
        public static IBasicContainer BuildContainer()
        {
            var container = new BasicContainer();

            container.Register<IComponentFactory, ComponentFactory>();
            container.Register<IComponentBuilder, ComponentBuilder>();
            RegisterComponents(container);

            container.Register<IConsoleBuffer, TextRenderBuffer>();

            container.Verify();
            return container;
        }

        private static void RegisterComponents(IBasicContainer container)
        {
            //TODO: use discovery to find and register all implementations of IComponent
            container.Register<TextOutput>();
            container.Register<VerticalLayout>();
        }
    }
}
