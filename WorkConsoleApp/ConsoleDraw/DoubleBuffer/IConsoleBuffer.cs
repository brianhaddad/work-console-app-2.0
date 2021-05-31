using System;

namespace ConsoleDraw.DoubleBuffer
{
    public interface IConsoleBuffer
    {
        void FillBuffer(char fillCharacter);
        void WindowResizeEvent();
        void ResetCursorToDefault();
        void SetDefaultCursorData(int left = 0, int top = 0, bool visible = false, ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black);
        void SetColors(ConsoleColor foreground, ConsoleColor background);
        void WriteLineToBuffer(string text, int x, int y);
        void DrawBuffer();
    }
}
