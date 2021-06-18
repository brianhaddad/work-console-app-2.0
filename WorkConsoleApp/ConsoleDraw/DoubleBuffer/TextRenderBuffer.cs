using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.DoubleBuffer
{
    public class TextRenderBuffer : IConsoleBuffer
    {
        private readonly int MaxWidth = Console.LargestWindowWidth;
        private readonly int MaxHeight = Console.LargestWindowHeight;
        private readonly char[] CurrentCharacterBuffer;
        private readonly char[] PreviousCharacterBuffer;
        private readonly bool[] NeedToRedraw;
        private int Width = 0;
        private int Height = 0;
        private int CombinedColorCode;
        private SortedDictionary<int, SortedList<int, int>> LocationsByColor = new SortedDictionary<int, SortedList<int, int>>();
        private SortedDictionary<int, int> ColorByLocation = new SortedDictionary<int, int>();
        private int DefaultCursorLeft = 0;
        private int DefaultCursorTop = 0;
        private bool DefaultCursorVisible = false;
        private ConsoleColor DefaultForegroundColor = ConsoleColor.White;
        private ConsoleColor DefaultBackgroundColor = ConsoleColor.Black;

        public int GetWidth() => Width;
        public int GetHeight() => Height;

        public TextRenderBuffer()
        {
            CurrentCharacterBuffer = new char[MaxWidth * MaxHeight];
            PreviousCharacterBuffer = new char[MaxWidth * MaxHeight];
            NeedToRedraw = new bool[MaxWidth * MaxHeight];
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
            LocationsByColor = new SortedDictionary<int, SortedList<int, int>>();
            ColorByLocation = new SortedDictionary<int, int>();
            for (var i = 0; i < CurrentCharacterBuffer.Length; i++)
            {
                CurrentCharacterBuffer[i] = fillCharacter;
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
                CurrentCharacterBuffer[start + i] = text[i];
                NeedToRedraw[start + i] = PreviousCharacterBuffer[start + i] != text[i];
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
            //first check and see if we've already stored a different color for this location
            if (ColorByLocation.ContainsKey(index) && ColorByLocation[index] != CombinedColorCode)
            {
                //if so, we need to tell that color that this location won't be that color anymore and we should redraw this location.
                NeedToRedraw[index] = true;
                LocationsByColor[ColorByLocation[index]].Remove(index); //remove this location from the draw list for the previous color.
                ColorByLocation[index] = CombinedColorCode; //set the current color as the correct color for this location
            }
            else if (!ColorByLocation.ContainsKey(index))
            {
                //if not, we can tell this location what color it will be
                ColorByLocation.Add(index, CombinedColorCode);
            }
            //next check to see if we're currently tracking locations for this color combination
            if (LocationsByColor.ContainsKey(CombinedColorCode))
            {
                //if so, just add this location to that color
                LocationsByColor[CombinedColorCode].Add(index, index);
            }
            else
            {
                //if not, create a new entry and initialize the list with this location as the first entry
                LocationsByColor.Add(CombinedColorCode, new SortedList<int, int> { { index, index } });
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
                    NeedToRedraw[(y * Console.WindowWidth) + x] = false;
                    ProtectedXYWrite(x, y, ' ');
                }
            }
            Console.Clear();
        }

        public void DrawBuffer()
        {
            Console.CursorVisible = false;
            Console.SetWindowPosition(0, 0);
            foreach (var dataKVP in LocationsByColor)
            {
                Console.BackgroundColor = DecodeBackgroundColor(dataKVP.Key);
                Console.ForegroundColor = DecodeForegroundColor(dataKVP.Key);
                var arr = dataKVP.Value.Keys.ToArray();
                for (var i = 0; i < arr.Length; i++)
                {
                    if (NeedToRedraw[arr[i]])
                    {
                        ProtectedXYWrite(arr[i] % Width, arr[i] / Width, CurrentCharacterBuffer[arr[i]]);
                        NeedToRedraw[arr[i]] = false;
                    }
                }
            }
            Array.Copy(CurrentCharacterBuffer, PreviousCharacterBuffer, CurrentCharacterBuffer.Length);
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
