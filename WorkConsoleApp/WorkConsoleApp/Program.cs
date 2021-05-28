using ConsoleDraw.Components;
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

            //TODO: need more factories to do initial setup on these guys, or maybe some kind of settings object.
            var buffer = container.Get<IConsoleBuffer>();
            buffer.WindowResizeEvent();
            buffer.SetColors(ConsoleColor.Red, ConsoleColor.Black);
            var componentFactory = container.Get<IComponentFactory>();
            var text1 = componentFactory.MakeComponent<TextOutput>();
            text1.ConfigureComponentLayout(layout => layout.SetPosition(0, 0));
            text1.SetText("Hello");
            var text2 = componentFactory.MakeComponent<TextOutput>();
            text2.ConfigureComponentLayout(layout => layout.SetPosition(10, 10));
            text2.SetText("World");
            text1.Draw(0, 0);
            text2.Draw(0, 0);
            buffer.DrawBuffer();
            Console.ReadLine();
        }
    }
}
