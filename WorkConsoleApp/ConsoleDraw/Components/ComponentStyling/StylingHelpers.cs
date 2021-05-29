using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleDraw.Components.ComponentStyling
{
    public static class StylingHelpers
    {

        private const char OverlayLeft = '\u2561';
        private const char OverlayRight = '\u255E';
        private const char OverlayHorizontal = '\u2500';
        private const char OverlayVertical = '\u2502';
        private const char OverlayUpperLeft = '\u250C';
        private const char OverlayUpperRight = '\u2510';
        private const char OverlayLowerLeft = '\u2514';
        private const char OverlayLowerRight = '\u2518';
        private const char WinUpperLeft = '\u2554';
        private const char WinHorizontal = '\u2550';
        private const char WinUpperRight = '\u2557';
        private const char WinVertical = '\u2551';
        private const char WinLowerLeft = '\u255A';
        private const char WinLowerRight = '\u255D';

        public static string[] PutInWindow(this string line, LayoutDetails layoutDetails, string winTitle = "")
        {
            var needsWrap = line.Length > layoutDetails.InnerWidth;
            var lines = needsWrap ? line.WordWrapLine(layoutDetails.InnerWidth) : new[] { line };
            return lines.PutInWindow(layoutDetails, winTitle);
        }

        public static string[] PutInWindow(this string[] lines, LayoutDetails layoutDetails, string winTitle = "")
        {
            var newLines = new List<string>();
            
            if (layoutDetails.MarginTop > 0)
            {
                for (var i = 0; i < layoutDetails.MarginTop; i++)
                {
                    newLines.Add(RepeatCharacters(' ', layoutDetails.WidthWithMarginAndBorder));
                }
            }
            //header area
            if (!winTitle.IsNullEmptyOrWhitespace())
            {
                //add the header using the overlay stuff
            }
            else if (layoutDetails.Border)
            {
                var firstLine = WinUpperLeft + RepeatCharacters(WinHorizontal, layoutDetails.Width) + WinUpperRight;
                newLines.Add(firstLine.AlignLine(HorizontalAlignment.Center, layoutDetails.WidthWithMarginAndBorder));
            }

            //body area (need to use vertical alignment from the layout details to insert blank lines before and/or after this)
            for (var i = 0; i < lines.Length; i++)
            {
                var innerAlignedLine = lines[i]
                    .AlignLine(layoutDetails.TextAlign, layoutDetails.InnerWidth)
                    .AlignLine(HorizontalAlignment.Center, layoutDetails.Width);
                var newLine = layoutDetails.Border ? WinVertical + innerAlignedLine + WinVertical : innerAlignedLine;
                newLines.Add(newLine.AlignLine(HorizontalAlignment.Center, layoutDetails.WidthWithMarginAndBorder));
            }

            //last line
            if (layoutDetails.Border)
            {
                var lastLine = WinLowerLeft + RepeatCharacters(WinHorizontal, layoutDetails.Width) + WinLowerRight;
                newLines.Add(lastLine.AlignLine(HorizontalAlignment.Center, layoutDetails.WidthWithMarginAndBorder));
            }

            return newLines.ToArray();
        }

        public static string[] WordWrapLine(this string line, int maxLineWidth)
        {
            var newLines = new List<string>();
            var words = line.Split(" ");
            var i = 0;
            var lineCount = 1;
            while (i < words.Length)
            {
                var nextLine = words[i];
                while (i < words.Length - 1 && nextLine.Length + words[i + 1].Length + 1 <= maxLineWidth)
                {
                    i++;
                    nextLine += " " + words[i];
                }
                newLines.Add(nextLine);
                lineCount++;
                i++;
            }
            return newLines.ToArray();
        }

        public static string AlignLine(this string line, HorizontalAlignment alignment, int maxLineWidth, char fillChar = ' ')
        {
            var numSpaces = maxLineWidth - line.Length;
            if (numSpaces < 1)
            {
                return line;
            }
            var beforeSpace = "";
            var afterSpace = "";
            switch (alignment)
            {
                case HorizontalAlignment.Left:
                    afterSpace = RepeatCharacters(fillChar, numSpaces);
                    break;

                case HorizontalAlignment.Right:
                    beforeSpace = RepeatCharacters(fillChar, numSpaces);
                    break;

                case HorizontalAlignment.Center:
                    beforeSpace = RepeatCharacters(fillChar, numSpaces / 2);
                    afterSpace = RepeatCharacters(fillChar, numSpaces - beforeSpace.Length);
                    break;
            }
            return beforeSpace + line + afterSpace;
        }

        private static string RepeatCharacters(char character, int numTimes)
        {
            var result = "";
            for (var i = 0; i < numTimes; i++)
            {
                result += character;
            }
            return result;
        }

        private static bool IsNullEmptyOrWhitespace(this string line)
            => (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line));
    }
}
