namespace ConsoleDraw.Components.ComponentStyling
{
    public class LayoutDetails
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
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
        public bool Border { get; set; }

        public int InnerWidth => Width - (PaddingLeft + PaddingRight);
        public int WidthWithMarginAndBorder => Width + (MarginLeft + MarginRight) + (Border ? 2 : 0);
        //Can't do computed height here, needs to live on the component.
    }
}
