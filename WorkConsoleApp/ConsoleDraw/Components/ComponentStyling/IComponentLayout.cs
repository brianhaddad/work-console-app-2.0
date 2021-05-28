namespace ConsoleDraw.Components.ComponentStyling
{
    public interface IComponentLayout
    {
        void SetSize(int w, int h);
        void SetWrap(bool wrap);
        void SetAnchorPoints(AnchorPoints[] anchorPoints);
        void SetHorizontalAlignment(HorizontalAlignment horizontalAlignment);
        void SetVerticalAlignment(VerticalAlignment verticalAlignment);
        void SetPosition(int left, int top);
        void SetMargin(int top, int right, int bottom, int left);
        void SetMargin(int top, int sides, int bottom);
        void SetMargin(int vertical, int horizontal);
        void SetMargin(int all);
        void SetPadding(int top, int right, int bottom, int left);
        void SetPadding(int top, int sides, int bottom);
        void SetPadding(int vertical, int horizontal);
        void SetPadding(int all);
        LayoutDetails GetLayout();
    }
}
