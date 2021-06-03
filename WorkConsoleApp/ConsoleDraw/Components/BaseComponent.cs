using ConsoleDraw.Components.ComponentStyling;
using System;
using System.Collections.Generic;

namespace ConsoleDraw.Components
{
    public abstract class BaseComponent : IComponent
    {
        private protected BaseComponent Parent;
        private protected List<IComponent> Children = new List<IComponent>();

        private protected readonly ComponentStyleAndLayout ComponentStyleAndLayout = new ComponentStyleAndLayout();
        private protected LayoutDetails Layout => ComponentStyleAndLayout.GetLayout();

        public int Height => ComponentStyleAndLayout.GetHeight();

        private protected string Text = "";
        private protected IEnumerable<string> Lines;

        /// <summary>
        /// This adds a child to the Children list.
        /// This method should never be called outside of the abstract base class and there is probably no reason to override it.
        /// </summary>
        /// <param name="child"></param>
        protected virtual void RegisterChild(IComponent child)
        {
            if (!Children.Contains(child))
            {
                Children.Add(child);
                ReflowComponentLayout();
            }
        }

        /// <summary>
        /// This removes a child from the Children list.
        /// This method should never be called outside of the abstract base class and there is probably no reason to override it.
        /// </summary>
        /// <param name="child"></param>
        protected virtual void RemoveChild(IComponent child)
        {
            if (Children.Contains(child))
            {
                Children.Remove(child);
                ReflowComponentLayout();
            }
        }

        /// <summary>
        /// This sets the component's parent and registers itself with its parent.
        /// </summary>
        /// <param name="parent"></param>
        public virtual void SetParent<T>(T parent) where T : BaseComponent
        {
            if (!(Parent is null))
            {
                Parent.RemoveChild(this);
            }
            Parent = parent;
            Parent.RegisterChild(this);
        }

        /// <summary>
        /// For now all components will have a Text field associated, even though it may not be used.
        /// </summary>
        /// <param name="text"></param>
        public virtual void SetText(string text)
        {
            Text = text;
        }

        /// <summary>
        /// This allows external configuration of the layout.
        /// There is probably no reason to override this, but I will allow it if you really must.
        /// </summary>
        /// <param name="action"></param>
        public virtual void ConfigureComponentLayout(Action<ComponentStyleAndLayout> action) => action(ComponentStyleAndLayout);

        /// <summary>
        /// This base Draw method activates the Draw method for all children and adjusts the origin for each.
        /// Not all components will have children, so base.Draw() doesn't always need to be called.
        /// Not all components will have visual elements, so base.Draw doesn't always need an override.
        /// </summary>
        /// <param name="originX"></param>
        /// <param name="originY"></param>
        public virtual void Draw(int originX, int originY)
        {
            //Not all components will have children, therefore not all components must call the base Draw.
            //The base draw will not work for layout components that must compute a new width and height for each component.
            foreach (var child in Children)
            {
                child.Draw(originX + Layout.X, originY + Layout.Y);
            }
        }

        /// <summary>
        /// Not all components need an update. This is probably mostly for handling logic.
        /// </summary>
        public virtual void Update()
        {
        }

        /// <summary>
        /// This is the method that will iterate over all the children and reflow them.
        /// Not all components will have to reflow, so no logic for reflow exists on the abstract base class.
        /// </summary>
        public virtual void ReflowComponentLayout()
        {
        }
    }
}
