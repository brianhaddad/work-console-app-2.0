using ConsoleDraw.Components.ComponentStyling;
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

        public override void Draw(int originX, int originY, int fillWidth, int fillHeight)
        {
            ComponentStyleAndLayout.SetTargetSize(fillWidth, fillHeight);
            var lines = Text.PutInWindow(Layout);
            ComponentStyleAndLayout.SetComputedHeight(lines.Length);
            ComponentStyleAndLayout.WriteColors(ConsoleBuffer);
            for (var i = 0; i < lines.Length; i++)
            {
                ConsoleBuffer.WriteLineToBuffer(lines[i], Layout.X, Layout.Y + i);
            }
            base.Draw(originX, originY, fillWidth, fillHeight);
        }
    }
}
