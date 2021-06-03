using ConsoleDraw.Components;
using ConsoleDraw.Components.ComponentStyling;
using System;

namespace ConsoleDraw.Services
{
    public interface IComponentBuilder
    {
        IComponent BuildTextComponent<T>(
            string text,
            ConsoleColor foregroundColor,
            ConsoleColor backgroundColor,
            T parent,
            bool border = false,
            int padding = 0,
            int margin = 0,
            int width = 0,
            int height = 0,
            HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left) where T : BaseComponent;
    }
}
