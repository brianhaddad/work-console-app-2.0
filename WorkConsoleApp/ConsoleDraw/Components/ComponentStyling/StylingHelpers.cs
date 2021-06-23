using System.Collections.Generic;

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
            //TODO: Use layoutDetails.HorizontalOverflow to determine whether we're goint to wrap things
            var needsWrap = line.Length > layoutDetails.InnerWidthInsidePadding;
            var lines = needsWrap ? line.WordWrapLine(layoutDetails.InnerWidthInsidePadding) : new[] { line };
            return lines.PutInWindow(layoutDetails, winTitle);
        }
        
        //TODO: factor out the border drawing or refactor this to work with empty text and a set size?
        public static string[] PutInWindow(this string[] lines, LayoutDetails layoutDetails, string winTitle = "")
        {
            var newLines = new List<string>();

            if (layoutDetails.MarginTop > 0)
            {
                for (var i = 0; i < layoutDetails.MarginTop; i++)
                {
                    newLines.Add(RepeatCharacters(' ', layoutDetails.FillWidth));
                }
            }
            //header area
            if (!winTitle.IsNullEmptyOrWhitespace())
            {
                //TODO: add the header using the overlay stuff
                if (layoutDetails.Border)
                {
                    //need to do new stuff
                }
                else
                {
                    //need to position the title text
                }
            }
            else if (layoutDetails.Border)
            {
                var firstLine = WinUpperLeft + RepeatCharacters(WinHorizontal, layoutDetails.InnerWidthInsideBorder) + WinUpperRight;
                newLines.Add(firstLine.AlignLine(HorizontalAlignment.Center, layoutDetails.FillWidth));
            }

            //TODO: padding should be calculated based on the number of content lines and the inner vertical height
            //For now, just throw in a blank line before and after. That will be perfect, thanks.
            if (layoutDetails.PaddingTop > 0)
            {
                for (var i = 0; i < layoutDetails.PaddingTop; i++)
                {
                    if (layoutDetails.Border)
                    {
                        var newLine = WinVertical + RepeatCharacters(' ', layoutDetails.InnerWidthInsideBorder) + WinVertical;
                        newLines.Add(newLine.AlignLine(HorizontalAlignment.Center, layoutDetails.FillWidth));
                    }
                    else
                    {
                        newLines.Add(RepeatCharacters(' ', layoutDetails.FillWidth));
                    }
                }
            }

            //body area (need to use vertical alignment from the layout details to insert blank lines before and/or after this)
            for (var i = 0; i < lines.Length; i++)
            {
                var innerAlignedLine = lines[i]
                    .AlignLine(layoutDetails.TextAlign, layoutDetails.InnerWidthInsidePadding)
                    .AlignLine(HorizontalAlignment.Center, layoutDetails.InnerWidthInsideBorder);
                var newLine = layoutDetails.Border ? WinVertical + innerAlignedLine + WinVertical : innerAlignedLine;
                newLines.Add(newLine.AlignLine(HorizontalAlignment.Center, layoutDetails.FillWidth));
            }

            //TODO: padding should be calculated based on the number of content lines and the inner vertical height
            //For now, just throw in a blank line before and after. That will be perfect, thanks.
            if (layoutDetails.PaddingBottom > 0)
            {
                for (var i = 0; i < layoutDetails.PaddingBottom; i++)
                {
                    if (layoutDetails.Border)
                    {
                        var newLine = WinVertical + RepeatCharacters(' ', layoutDetails.InnerWidthInsideBorder) + WinVertical;
                        newLines.Add(newLine.AlignLine(HorizontalAlignment.Center, layoutDetails.FillWidth));
                    }
                    else
                    {
                        newLines.Add(RepeatCharacters(' ', layoutDetails.FillWidth));
                    }
                }
            }

            //last line
            if (layoutDetails.Border)
            {
                var lastLine = WinLowerLeft + RepeatCharacters(WinHorizontal, layoutDetails.InnerWidthInsideBorder) + WinLowerRight;
                newLines.Add(lastLine.AlignLine(HorizontalAlignment.Center, layoutDetails.FillWidth));
            }

            if (layoutDetails.MarginBottom > 0)
            {
                for (var i = 0; i < layoutDetails.MarginBottom; i++)
                {
                    newLines.Add(RepeatCharacters(' ', layoutDetails.FillWidth));
                }
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
