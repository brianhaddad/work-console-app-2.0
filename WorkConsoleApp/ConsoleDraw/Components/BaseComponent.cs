using ConsoleDraw.Components.ComponentStyling;
using System;
using System.Collections.Generic;

namespace ConsoleDraw.Components
{
    public abstract class BaseComponent : IComponent
    {
        private protected IComponent Parent;
        private protected List<IComponent> Children = new List<IComponent>();

        private protected readonly ComponentLayout ComponentLayout = new ComponentLayout();
        private protected LayoutDetails Layout => ComponentLayout.GetLayout();

        private protected string Text = "";

        public virtual void Draw(int originX, int originY)
        {
            foreach (var child in Children)
            {
                child.Draw(originX + Layout.X, originY + Layout.Y);
            }
        }

        public virtual void Update()
        {
            throw new NotImplementedException();
        }

        public void RegisterChild(IComponent child)
        {
            if (!Children.Contains(child))
            {
                Children.Add(child);
            }
        }

        public void RemoveChild(IComponent child)
        {
            if (Children.Contains(child))
            {
                Children.Remove(child);
            }
        }

        public void SetParent(IComponent parent)
        {
            if (!(Parent is null))
            {
                Parent.RemoveChild(this);
            }
            Parent = parent;
            Parent.RegisterChild(this);
        }

        public virtual void SetText(string text)
        {
            Text = text;
        }

        public void ConfigureComponentLayout(Action<ComponentLayout> action) => action(ComponentLayout);
    }
}
