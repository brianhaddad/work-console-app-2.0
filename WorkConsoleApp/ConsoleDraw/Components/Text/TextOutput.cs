using ConsoleDraw.Components.ComponentStyling;
using ConsoleDraw.DoubleBuffer;
using System;

namespace ConsoleDraw.Components.Text
{
    public class TextOutput : IComponent
    {
        private readonly IConsoleBuffer ConsoleBuffer;
        private string Text = "";
        private int X = 0;
        private int Y = 0;

        public TextOutput(IConsoleBuffer consoleBuffer)
        {
            ConsoleBuffer = consoleBuffer ?? throw new ArgumentNullException(nameof(consoleBuffer));
        }

        public void SetText(string text)
        {
            Text = text;
        }

        public void SetAnchorPoints(AnchorPoints[] anchorPoints)
        {
            throw new NotImplementedException();
        }

        public void SetMargin(int top, int right, int bottom, int left)
        {
            throw new NotImplementedException();
        }

        public void SetMargin(int top, int sides, int bottom)
        {
            throw new NotImplementedException();
        }

        public void SetMargin(int vertical, int horizontal)
        {
            throw new NotImplementedException();
        }

        public void SetMargin(int all)
        {
            throw new NotImplementedException();
        }

        public void SetPadding(int top, int right, int bottom, int left)
        {
            throw new NotImplementedException();
        }

        public void SetPadding(int top, int sides, int bottom)
        {
            throw new NotImplementedException();
        }

        public void SetPadding(int vertical, int horizontal)
        {
            throw new NotImplementedException();
        }

        public void SetPadding(int all)
        {
            throw new NotImplementedException();
        }

        public void SetPosition(int left, int top)
        {
            X = left;
            Y = top;
        }

        public void SetSize(int w, int h)
        {
            throw new NotImplementedException();
        }

        public void SetWrap(bool wrap)
        {
            throw new NotImplementedException();
        }

        public void SetVerticalAlignment(VerticalAlignment verticalAlignment)
        {
            throw new NotImplementedException();
        }

        public void SetHorizontalAlignment(HorizontalAlignment horizontalAlignment)
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void Draw()
        {
            ConsoleBuffer.WriteLineToBuffer(Text, X, Y);
        }
    }
}
