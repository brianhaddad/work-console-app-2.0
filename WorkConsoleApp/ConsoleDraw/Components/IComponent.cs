using ConsoleDraw.Components.ComponentStyling;
using System;

namespace ConsoleDraw.Components
{
    public interface IComponent
    {
        void Update();
        void Draw(int originX, int originY);
        void ConfigureComponentLayout(Action<ComponentStyleAndLayout> action);
        void ReflowComponentLayout();
        void SetParent<T>(T parent) where T : BaseComponent;
        void SetText(string text);
        int Height { get; }
        int Width { get; }
    }
    public interface IDataComponent<T> : IComponent
    {
        void LoadData(DataTableConfig<T> dataTableConfig);
    }
}
