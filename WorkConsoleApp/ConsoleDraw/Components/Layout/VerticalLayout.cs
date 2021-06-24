namespace ConsoleDraw.Components.Layout
{
    public class VerticalLayout : BaseComponent
    {
        public override void ReflowComponentLayout()
        {
            var x = Layout.HorizontalBorderThickness;
            var y = Layout.TopBorderThickness;
            foreach (var child in Children)
            {
                child.ConfigureComponentLayout((config) =>
                {
                    config.SetPosition(x, y);
                    config.SetTargetSize(Layout.InnerWidthInsidePadding, Layout.InnerHeightInsidePadding);
                });
                child.ReflowComponentLayout();
                y += child.Height;
            }
        }
    }
}
