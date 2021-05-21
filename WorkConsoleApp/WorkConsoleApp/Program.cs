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
        }
    }
}
