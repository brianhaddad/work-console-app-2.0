namespace ConsoleDraw.Components.ComponentStyling
{
    public class LayoutDetails
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int FillWidth { get; set; }
        public int FillHeight { get; set; }
        public int MarginTop { get; set; }
        public int MarginRight { get; set; }
        public int MarginBottom { get; set; }
        public int MarginLeft { get; set; }
        public int PaddingTop { get; set; }
        public int PaddingRight { get; set; }
        public int PaddingBottom { get; set; }
        public int PaddingLeft { get; set; }
        public HorizontalAlignment TextAlign { get; set; }
        public VerticalAlignment VerticalAlign { get; set; }
        public HorizontalOverflow HorizontalOverflow { get; set; }
        public bool Border { get; set; }

        private int TotalBorderThickness => (Border ? 2 : 0);

        private int TotalPaddingWidth => PaddingLeft + PaddingRight;
        private int TotalPaddingHeight => PaddingTop + PaddingBottom;

        private int TotalMarginWidth => MarginRight + MarginLeft;
        private int TotalMarginHeight => MarginTop + MarginBottom;

        public int InnerWidthInsidePadding => FillWidth - TotalPaddingWidth - TotalMarginWidth - TotalBorderThickness;
        public int InnerHeightInsidePadding => FillHeight - TotalPaddingHeight - TotalBorderThickness;

        public int InnerWidthInsideBorder => FillWidth - TotalMarginWidth - TotalBorderThickness;
        public int InnerHeightInsideBorder => FillHeight - TotalMarginHeight - TotalBorderThickness;
        //Can't do computed height here, needs to live on the component.
    }
}
