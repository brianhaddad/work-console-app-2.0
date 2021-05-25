using System;

namespace ConsoleDraw.DoubleBuffer
{
    public interface IConsoleBuffer
    {
        void FillBuffer(char fillCharacter);
        void WindowResizeEvent();
        void SetColors(ConsoleColor foreground, ConsoleColor background);
        void WriteLineToBuffer(string text, int x, int y);
        void DrawBuffer();
    }
}
