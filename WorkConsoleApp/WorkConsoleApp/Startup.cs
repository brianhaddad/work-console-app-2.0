using BasicDependencyInjection;
using BasicDependencyInjection.Container;
using ConsoleDraw.DoubleBuffer;

namespace WorkConsoleApp
{
    public static class Startup
    {
        public static IBasicContainer BuildContainer()
        {
            var container = new BasicContainer();

            //TODO: register classes and stuff here
            container.Register<IConsoleBuffer, TextRenderBuffer>();

            return container;
        }
    }
}
