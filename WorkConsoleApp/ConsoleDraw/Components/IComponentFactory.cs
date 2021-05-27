namespace ConsoleDraw.Components
{
    public interface IComponentFactory
    {
        T MakeComponent<T>() where T : class, IComponent;
    }
}
