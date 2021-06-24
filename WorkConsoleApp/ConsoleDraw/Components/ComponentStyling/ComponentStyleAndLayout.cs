using ConsoleDraw.DoubleBuffer;
using System;

namespace ConsoleDraw.Components.ComponentStyling
{
    public class ComponentStyleAndLayout : IComponentStyleAndLayout
    {
        private int X = 0;
        private int Y = 0;
        private int MinimumWidth = 10;
        private int Height = 0;
        private bool Border = false;
        private int MarginTop = 0;
        private int MarginRight = 0;
        private int MarginBottom = 0;
        private int MarginLeft = 0;
        private int PaddingTop = 0;
        private int PaddingRight = 0;
        private int PaddingBottom = 0;
        private int PaddingLeft = 0;
        private HorizontalAlignment TextAlign = HorizontalAlignment.Left;
        private VerticalAlignment VerticalAlign = VerticalAlignment.Top;
        private ConsoleColor BackgroundColor = ConsoleColor.Black;
        private ConsoleColor ForegroundColor = ConsoleColor.Gray;
        private Overflow HorizontalOverflowBehavior = Overflow.Wrap;
        private Overflow VerticalOverflowBehavior = Overflow.Wrap;
        private SpaceFilling HorizontalSpaceFillingBehavior = SpaceFilling.Natural;
        private SpaceFilling VerticalSpaceFillingBehavior = SpaceFilling.Natural;
        private int? NumberOfLines = null;

        public void SetAnchorPoints(AnchorPoints[] anchorPoints)
        {
            //TODO: do I need anchor points really? Maybe not.
            throw new NotImplementedException();
        }

        public void SetMargin(int top, int right, int bottom, int left)
        {
            MarginTop = top;
            MarginRight = right;
            MarginBottom = bottom;
            MarginLeft = left;
        }

        public void SetMargin(int top, int sides, int bottom) => SetMargin(top, sides, bottom, sides);

        public void SetMargin(int vertical, int horizontal) => SetMargin(vertical, horizontal, vertical, horizontal);

        public void SetMargin(int all) => SetMargin(all, all, all, all);

        public void SetPadding(int top, int right, int bottom, int left)
        {
            PaddingTop = top;
            PaddingRight = right;
            PaddingBottom = bottom;
            PaddingLeft = left;
        }

        public void SetPadding(int top, int sides, int bottom) => SetPadding(top, sides, bottom, sides);

        public void SetPadding(int vertical, int horizontal) => SetPadding(vertical, horizontal, vertical, horizontal);

        public void SetPadding(int all) => SetPadding(all, all, all, all);

        public void SetPosition(int left, int top)
        {
            X = left;
            Y = top;
        }

        public void SetMinimumSize(int w, int h)
        {
            MinimumWidth = w;
            Height = h;
        }

        public void SetTargetSize(int w, int h)
        {
            MinimumWidth = Math.Max(MinimumWidth, w);
            Height = Math.Max(Height, h);
        }

        public void SetBorder(bool border)
        {
            Border = border;
        }

        public void SetVerticalAlignment(VerticalAlignment verticalAlignment)
        {
            VerticalAlign = verticalAlignment;
        }

        public void SetHorizontalAlignment(HorizontalAlignment horizontalAlignment)
        {
            TextAlign = horizontalAlignment;
        }

        public void SetHorizontalOverflow(Overflow horizontalOverflow)
        {
            HorizontalOverflowBehavior = horizontalOverflow;
        }

        public void SetVerticalOverflow(Overflow verticalOverflow)
        {
            VerticalOverflowBehavior = verticalOverflow;
        }

        public void SetHorizontalSpaceFilling(SpaceFilling horizontalSpaceFilling)
        {
            HorizontalSpaceFillingBehavior = horizontalSpaceFilling;
        }

        public void SetVerticalSpaceFilling(SpaceFilling verticalSpaceFilling)
        {
            VerticalSpaceFillingBehavior = verticalSpaceFilling;
        }

        public StyleDetails GetLayout()
        {
            return new StyleDetails
            {
                X = X,
                Y = Y,
                SpaceToFillWidth = MinimumWidth, //This can optionally be set to something larger, but not smaller
                SpaceToFillHeight = Height, //Same as width?
                ContentLines = NumberOfLines,
                Border = Border,
                MarginBottom = MarginBottom,
                MarginLeft = MarginLeft,
                MarginRight = MarginRight,
                MarginTop = MarginTop,
                PaddingBottom = PaddingBottom,
                PaddingLeft = PaddingLeft,
                PaddingRight = PaddingRight,
                PaddingTop = PaddingTop,
                TextAlign = TextAlign,
                VerticalAlign = VerticalAlign,
                HorizontalOverflow = HorizontalOverflowBehavior,
                VerticalOverflow = VerticalOverflowBehavior,
                HorizontalSpaceFilling = HorizontalSpaceFillingBehavior,
                VerticalSpaceFilling = VerticalSpaceFillingBehavior,
            };
        }

        public void SetNumberOfLines(int numLines)
        {
            NumberOfLines = numLines;
        }

        public void SetComponentColors(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
        }

        public void WriteColors(IConsoleBuffer buffer)
        {
            buffer.SetColors(ForegroundColor, BackgroundColor);
        }

        public int GetHeight()
        {
            if (NumberOfLines.HasValue)
            {
                return NumberOfLines.Value;
            }
            return Height;
        }
    }
}
