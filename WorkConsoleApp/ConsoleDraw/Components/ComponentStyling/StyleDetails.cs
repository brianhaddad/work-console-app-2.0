namespace ConsoleDraw.Components.ComponentStyling
{
    public class StyleDetails
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int SpaceToFillWidth { get; set; }
        public int SpaceToFillHeight { get; set; }
        public int? ContentLines { get; set; } //Do I need? How to use?
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
        public Overflow HorizontalOverflow { get; set; }
        public Overflow VerticalOverflow { get; set; }
        public SpaceFilling HorizontalSpaceFilling { get; set; }
        public SpaceFilling VerticalSpaceFilling { get; set; }
        public bool Border { get; set; }

        public int HorizontalBorderThickness => Border ? 1 : 0;
        public int TopBorderThickness => Border ? 1 : 0; //TODO: instead of 1 we need to compute based on whether there is a title or not
        public int BottomBorderThickness => Border ? 1 : 0;

        private int TotalHorizontalBorderThickness => HorizontalBorderThickness * 2;
        private int TotalVerticalBorderThickness => TopBorderThickness + BottomBorderThickness;

        private int TotalPaddingWidth => PaddingLeft + PaddingRight;
        private int TotalPaddingHeight => PaddingTop + PaddingBottom;

        private int TotalMarginWidth => MarginRight + MarginLeft;
        private int TotalMarginHeight => MarginTop + MarginBottom;

        public int InnerWidthInsidePadding => SpaceToFillWidth - TotalPaddingWidth - TotalMarginWidth - TotalHorizontalBorderThickness;
        public int InnerHeightInsidePadding => SpaceToFillHeight - TotalPaddingHeight - TotalVerticalBorderThickness;

        public int InnerWidthInsideBorder => SpaceToFillWidth - TotalMarginWidth - TotalHorizontalBorderThickness;
        public int InnerHeightInsideBorder => SpaceToFillHeight - TotalMarginHeight - TotalVerticalBorderThickness;
    }
}
