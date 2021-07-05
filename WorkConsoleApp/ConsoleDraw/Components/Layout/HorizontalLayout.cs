namespace ConsoleDraw.Components.Layout
{
    public class HorizontalLayout : BaseComponent
    {
        public override void ReflowComponentLayout()
        {
            //TODO: this flow ignores any custom formatting options.
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
                x += child.Width;
            }
        }
    }
}
