using ConsoleDraw.Components.ComponentStyling;
using System;

namespace ConsoleDraw.Components
{
    public interface IComponent
    {
        void Update();
        void Draw(int originX, int originY, int fillWidth, int fillHeight);
        void ConfigureComponentLayout(Action<ComponentStyleAndLayout> action);
        void ReflowComponentLayout();
        void SetParent<T>(T parent) where T : BaseComponent;
        void SetText(string text);
    }
}
