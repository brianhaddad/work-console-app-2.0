using ConsoleDraw.DoubleBuffer;
using System;
using System.Collections.Generic;

namespace ConsoleDraw.Components.ComponentStyling
{
    public interface IComponentStyleAndLayout
    {
        void SetMinimumSize(int w, int h);
        void SetTargetSize(int w, int h);
        void SetBorder(bool border);
        void SetAnchorPoints(AnchorPoints[] anchorPoints);
        void SetHorizontalAlignment(HorizontalAlignment horizontalAlignment);
        void SetVerticalAlignment(VerticalAlignment verticalAlignment);
        void SetComponentColors(ConsoleColor foregroundColor, ConsoleColor backgroundColor);
        void SetHorizontalOverflow(Overflow horizontalOverflow);
        void SetVerticalOverflow(Overflow verticalOverflow);
        void SetHorizontalSpaceFilling(SpaceFilling horizontalSpaceFilling);
        void SetVerticalSpaceFilling(SpaceFilling verticalSpaceFilling);
        void SetPosition(int left, int top);
        void SetMargin(int top, int right, int bottom, int left);
        void SetMargin(int top, int sides, int bottom);
        void SetMargin(int vertical, int horizontal);
        void SetMargin(int all);
        void SetPadding(int top, int right, int bottom, int left);
        void SetPadding(int top, int sides, int bottom);
        void SetPadding(int vertical, int horizontal);
        void SetPadding(int all);
        void SetContentDimensions(IEnumerable<string> lines);
        StyleDetails GetLayout();
        int GetContentHeight();
        void WriteColors(IConsoleBuffer buffer);
    }
}
