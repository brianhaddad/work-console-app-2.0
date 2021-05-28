using System;

namespace ConsoleDraw.Components.ComponentStyling
{
    public class ComponentLayout : IComponentLayout
    {
        private int X = 0;
        private int Y = 0;
        private int Width = 0;
        private int Height = 0;

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
            Width = w;
            Height = h;
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

        public LayoutDetails GetLayout()
        {
            return new LayoutDetails
            {
                X = X,
                Y = Y,
                Width = Width,
                Height = Height,
            };
        }
    }
}
