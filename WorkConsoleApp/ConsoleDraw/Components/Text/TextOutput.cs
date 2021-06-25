using ConsoleDraw.Components.ComponentStyling;
using ConsoleDraw.DoubleBuffer;
using System;
using System.Linq;

namespace ConsoleDraw.Components.Text
{
    public class TextOutput : BaseComponent
    {
        private readonly IConsoleBuffer ConsoleBuffer;

        public TextOutput(IConsoleBuffer consoleBuffer)
        {
            ConsoleBuffer = consoleBuffer ?? throw new ArgumentNullException(nameof(consoleBuffer));
        }

        public override void Update()
        {
            base.Update();
        }

        public override void ReflowComponentLayout()
        {
            Lines = Text.PutInWindow(Layout);
            ComponentStyleAndLayout.SetContentDimensions(Lines);
        }

        public override void Draw(int originX, int originY)
        {
            ComponentStyleAndLayout.WriteColors(ConsoleBuffer);
            var lines = Lines.ToArray();
            for (var i = 0; i < lines.Length; i++)
            {
                ConsoleBuffer.WriteLineToBuffer(lines[i], originX + Layout.X, originY + Layout.Y + i);
            }
            base.Draw(originX, originY);
        }
    }
}
