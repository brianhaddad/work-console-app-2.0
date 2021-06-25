using ConsoleDraw.Components;
using ConsoleDraw.Components.ComponentStyling;
using ConsoleDraw.Components.Layout;
using ConsoleDraw.Components.Text;
using ConsoleDraw.DoubleBuffer;
using ConsoleDraw.Services;
using System;
using System.Collections.Generic;

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

            var configData = new DataTableConfig<string>
            {
                Data = new List<List<string>>
                {
                    new List<string> { "a", "totally", "0.0" },
                    new List<string> { "b", "not", "1.0" },
                    new List<string> { "c", "a big table", "0.5" },
                    new List<string> { "", "total", "1.5" },
                },
                LastRowIsSum = true,
                OddLineForegroundColor = ConsoleColor.Black,
                OddLineBackgroundColor = ConsoleColor.Gray,
            };
            componentBuilder.BuildDataTableComponent<SimpleTextTable, VerticalLayout, string>(
                configData,
                ConsoleColor.Black,
                ConsoleColor.DarkGray,
                layoutComponent,
                border: true,
                padding: 1,
                horizontalSpaceFilling: SpaceFilling.Natural); //Natural should mean it doesn't fill the space, but it does due to the reflow process. Fix?

            layoutComponent.ReflowComponentLayout();
            layoutComponent.Draw(0, 0);

            buffer.DrawBuffer();
            Console.ReadLine();
        }
    }
}
