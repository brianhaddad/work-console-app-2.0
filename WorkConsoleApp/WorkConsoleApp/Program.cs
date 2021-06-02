using ConsoleDraw.Components;
using ConsoleDraw.Components.ComponentStyling;
using ConsoleDraw.Components.Text;
using ConsoleDraw.DoubleBuffer;
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
            var text1 = componentFactory.MakeComponent<TextOutput>(
                (component) =>
                {
                    component.SetText("This sentence is longer and more interesting.");
                },
                (layout) =>
                {
                    layout.SetPosition(0, 0);
                    layout.SetPadding(1);
                    layout.SetMargin(1);
                    layout.SetBorder(true);
                    layout.SetHorizontalAlignment(HorizontalAlignment.Center);
                    layout.SetMinimumSize(25, 0);
                    layout.SetComponentColors(ConsoleColor.Blue, ConsoleColor.White);
                });
            var text2 = componentFactory.MakeComponent<TextOutput>(
                (component) =>
                {
                    component.SetText("WorldWideWeb");
                }, 
                (layout) =>
                {
                    layout.SetPosition(25, 10);
                    layout.SetPadding(0);
                    layout.SetMargin(0);
                    layout.SetBorder(true);
                    layout.SetHorizontalAlignment(HorizontalAlignment.Right);
                    layout.SetMinimumSize(20, 0);
                    layout.SetComponentColors(ConsoleColor.Green, ConsoleColor.Black);
                });
            text1.Draw(0, 0, 0, 0);
            text2.Draw(0, 0, 0, 0);
            buffer.DrawBuffer();
            Console.ReadLine();
        }
    }
}
