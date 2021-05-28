using ConsoleDraw.Components.ComponentStyling;
using System;

namespace ConsoleDraw.Components
{
    public interface IComponent
    {
        void Update();
        void Draw(int originX, int originY);
        void ConfigureComponentLayout(Action<ComponentLayout> action);
        void SetParent(IComponent parent);
        void RegisterChild(IComponent child);
        void RemoveChild(IComponent child);
        void SetText(string text);
    }
}
