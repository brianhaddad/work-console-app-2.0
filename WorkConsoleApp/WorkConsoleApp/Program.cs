using ConsoleDraw.Components;
using ConsoleDraw.Components.ComponentStyling;
using ConsoleDraw.Components.Layout;
using ConsoleDraw.DoubleBuffer;
using ConsoleDraw.Services;
using System;

namespace WorkConsoleApp
{
    class Program
    {
        //building out a drawing and interface design using code from:
        //https://github.com/brianhaddad/ChildrensFortuneTeller/tree/main/SimpleFortuneTeller/SimpleFortuneTeller
        static void Main(string[] args)
        {
            var container = Startup.BuildContainer();
            /*
             * Parts:
             * TextRenderBuffer (ITextRenderBuffer)
             * gets data from
             * Screen Manager (IScreenManager)
             * has stack of layers, layer interface has "is full screen" concept
             * screen manager sends the topmost full screen layer plus any popups
             * 
             * what about user input?
             * */

            var buffer = container.Get<IConsoleBuffer>();
            buffer.WindowResizeEvent();
            var componentFactory = container.Get<IComponentFactory>();
            var componentBuilder = container.Get<IComponentBuilder>();

            var layoutComponent = componentFactory.MakeComponent<VerticalLayout>((layout) =>
            {
                layout.SetPosition(0, 0);
                layout.SetTargetSize(Console.WindowWidth, Console.WindowHeight);
                layout.SetPadding(0);
                layout.SetMargin(0);
                layout.SetBorder(true);
            });

            componentBuilder.BuildTextComponent("This sentence is longer and more interesting.", ConsoleColor.Red, ConsoleColor.Black, layoutComponent);
            componentBuilder.BuildTextComponent("Something witty.", ConsoleColor.White, ConsoleColor.DarkBlue, layoutComponent, border: true, horizontalAlignment: HorizontalAlignment.Right);

            layoutComponent.ReflowComponentLayout();
            layoutComponent.Draw(0, 0);

            buffer.DrawBuffer();
            Console.ReadLine();
        }
    }
}
