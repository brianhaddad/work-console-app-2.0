using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.DoubleBuffer
{
    public class TextRenderBuffer : IConsoleBuffer
    {
        private readonly int MaxWidth = Console.LargestWindowWidth;
        private readonly int MaxHeight = Console.LargestWindowHeight;
        private readonly char[] TextBuffer;
        private int Width = 0;
        private int Height = 0;
        private int CombinedColorCode;
        private SortedDictionary<int, SortedList<int, int>> DrawDictionary = new SortedDictionary<int, SortedList<int, int>>();
        private SortedDictionary<int, int> DrawDictionaryReverseLookup = new SortedDictionary<int, int>();
        private int DefaultCursorLeft = 0;
        private int DefaultCursorTop = 0;
        private bool DefaultCursorVisible = false;
        private ConsoleColor DefaultForegroundColor = ConsoleColor.White;
        private ConsoleColor DefaultBackgroundColor = ConsoleColor.Black;

        public int GetWidth() => Width;
        public int GetHeight() => Height;

        public TextRenderBuffer()
        {
            TextBuffer = new char[MaxWidth * MaxHeight];
        }

        public void SetDefaultCursorData(int left = 0, int top = 0, bool visible = false, ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            DefaultCursorLeft = left;
            DefaultCursorTop = top;
            DefaultCursorVisible = visible;
            DefaultForegroundColor = foregroundColor;
            DefaultBackgroundColor = backgroundColor;
        }

        public void ResetCursorToDefault()
        {
            Console.ForegroundColor = DefaultForegroundColor;
            Console.BackgroundColor = DefaultBackgroundColor;
            Console.SetWindowPosition(0, 0);
            if (DefaultCursorLeft < Console.WindowWidth && DefaultCursorTop < Console.WindowHeight)
            {
                Console.CursorLeft = DefaultCursorLeft;
                Console.CursorTop = DefaultCursorTop;
                Console.CursorVisible = DefaultCursorVisible;
            }
            else
            {
                Console.CursorVisible = false;
            }
        }

        public void FillBuffer(char fillCharacter = ' ')
        {
            DrawDictionary = new SortedDictionary<int, SortedList<int, int>>();
            DrawDictionaryReverseLookup = new SortedDictionary<int, int>();
            for (var i = 0; i < TextBuffer.Length; i++)
            {
                TextBuffer[i] = fillCharacter;
            }
        }

        public void WindowResizeEvent()
        {
            var width = Console.WindowWidth;
            var height = Console.WindowHeight;
            var oldWidth = Width;
            var oldHeight = Height;
            Width = Math.Min(width, MaxWidth);
            Height = Math.Min(height, MaxHeight);
            if (Width != oldWidth || Height != oldHeight)
            {
                Console.CursorVisible = false;
                ForceClear();
                ResetCursorToDefault();
            }
        }

        public void SetColors(ConsoleColor foreground, ConsoleColor background)
            => CombinedColorCode = EncodeCombinedColor(foreground, background);

        public void WriteLineToBuffer(string text, int x, int y)
        {
            var start = (y * Width) + x;
            for (var i = 0; i < text.Length; i++)
            {
                TextBuffer[start + i] = text[i];
                LogColorPosition(start + i);
            }
        }

        private int EncodeCombinedColor(ConsoleColor foreground, ConsoleColor background)
            => ((int)background << 4) | (int)foreground;

        private ConsoleColor DecodeForegroundColor(int combinedColorCode)
            => (ConsoleColor)(combinedColorCode & 0b00001111);

        private ConsoleColor DecodeBackgroundColor(int combinedColorCode)
            => (ConsoleColor)(combinedColorCode >> 4);

        private void LogColorPosition(int index)
        {
            if (DrawDictionaryReverseLookup.ContainsKey(index))
            {
                DrawDictionary[DrawDictionaryReverseLookup[index]].Remove(index);
                DrawDictionaryReverseLookup[index] = CombinedColorCode;
            }
            else
            {
                DrawDictionaryReverseLookup.Add(index, CombinedColorCode);
            }
            if (DrawDictionary.ContainsKey(CombinedColorCode))
            {
                DrawDictionary[CombinedColorCode].Add(index, index);
            }
            else
            {
                DrawDictionary.Add(CombinedColorCode, new SortedList<int, int> { { index, index } });
            }
        }

        private void ForceClear()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            for (var y = 0; y <= Console.WindowHeight + 1; y++)
            {
                for (var x = 0; x < Console.WindowWidth; x++)
                {
                    ProtectedXYWrite(x, y, ' ');
                }
            }
            Console.Clear();
        }

        //Stream writer flush method:
        //https://blog.anarks2.com/Buffered-dotnet-console-1/
        //Doesn't support setting the position or changing colors though. :(
        //Also Console.OpenStandardInput() doesn't get a stream that can be written to here for some reason.
        public void DrawBuffer()
        {
            Console.CursorVisible = false;
            Console.SetWindowPosition(0, 0);
            foreach (var dataKVP in DrawDictionary)
            {
                Console.BackgroundColor = DecodeBackgroundColor(dataKVP.Key);
                Console.ForegroundColor = DecodeForegroundColor(dataKVP.Key);
                var arr = dataKVP.Value.Keys.ToArray();
                for (var i = 0; i < arr.Length; i++)
                {
                    ProtectedXYWrite(arr[i] % Width, arr[i] / Width, TextBuffer[arr[i]]);
                }
            }
            ResetCursorToDefault();
        }

        private void ProtectedXYWrite(int x, int y, char character)
        {
            if (Console.WindowWidth >= Width
                && Console.WindowHeight >= Height
                && x < Console.WindowWidth
                && y <= Console.WindowHeight)
            {
                Console.CursorLeft = x;
                Console.CursorTop = y;
                Console.Write(character);
            }
        }
    }
}
