﻿using System;

namespace ConsoleDraw.Components.ComponentStyling
{
    public class ComponentLayout : IComponentLayout
    {
        private int X = 0;
        private int Y = 0;
        private int Width = 10;
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
        private int? ComputedHeight = null;

        public void SetAnchorPoints(AnchorPoints[] anchorPoints)
        {
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

        public void SetSize(int w, int h)
        {
            Width = w;
            Height = h;
        }

        public void SetWrap(bool wrap)
        {
            throw new NotImplementedException();
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

        public LayoutDetails GetLayout()
        {
            return new LayoutDetails
            {
                X = X,
                Y = Y,
                Width = Width,
                Height = Height,
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
            };
        }

        public void SetComputedHeight(int height)
        {
            ComputedHeight = height;
        }
    }
}
