using ConsoleDraw.Components;
using ConsoleDraw.Components.ComponentStyling;
using ConsoleDraw.Components.Text;
using System;

namespace ConsoleDraw.Services
{
    public class ComponentBuilder : IComponentBuilder
    {
        private readonly IComponentFactory ComponentFactory;

        public ComponentBuilder(IComponentFactory componentFactory)
        {
            ComponentFactory = componentFactory ?? throw new ArgumentNullException(nameof(componentFactory));
        }

        public IComponent BuildTextComponent<T>(
            string text,
            ConsoleColor foregroundColor,
            ConsoleColor backgroundColor,
            T parent,
            bool border = false,
            int padding = 0,
            int margin = 0,
            int width = 10,
            int height = 0,
            HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left) where T : BaseComponent
        {
            return ComponentFactory.MakeComponent<TextOutput>((component) =>
                {
                    component.SetText(text);
                    component.SetParent(parent);
                    component.ConfigureComponentLayout((layout) =>
                    {
                        layout.SetPadding(padding);
                        layout.SetMargin(margin);
                        layout.SetBorder(border);
                        layout.SetHorizontalAlignment(horizontalAlignment);
                        layout.SetMinimumSize(width, height);
                        layout.SetComponentColors(foregroundColor, backgroundColor);
                    });
                });
        }

        public IDataComponent<T> BuildDataTableComponent<ConcreteType, ParentType, T>(
            DataTableConfig<T> data,
            ConsoleColor foregroundColor,
            ConsoleColor backgroundColor,
            ParentType parent,
            bool border = false,
            int padding = 0,
            int margin = 0,
            int width = 10,
            int height = 0,
            HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left,
            SpaceFilling horizontalSpaceFilling = SpaceFilling.Natural)
            where ConcreteType : BaseComponent, IDataComponent<T> where ParentType : BaseComponent
        {
            var component = ComponentFactory.MakeComponent<ConcreteType>((c) =>
            {
                c.SetParent(parent);
                c.ConfigureComponentLayout((layout) =>
                {
                    layout.SetPadding(padding);
                    layout.SetMargin(margin);
                    layout.SetBorder(border);
                    layout.SetHorizontalAlignment(horizontalAlignment);
                    layout.SetMinimumSize(width, height);
                    layout.SetComponentColors(foregroundColor, backgroundColor);
                });
            });
            component.LoadData(data);
            return component;
        }
    }
}
