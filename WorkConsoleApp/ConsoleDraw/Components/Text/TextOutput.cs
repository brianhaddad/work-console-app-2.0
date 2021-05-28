using ConsoleDraw.DoubleBuffer;
using System;

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

        public override void Draw(int originX, int originY)
        {
            ConsoleBuffer.WriteLineToBuffer(Text, Layout.X, Layout.Y);
            base.Draw(originX, originY);
        }
    }
}
