using InjectionMap.Internals;

namespace InjectionMap.Components
{
    internal abstract class ComponentExpression
    {
        public ComponentExpression(IComponentCollection container, IMappingComponent component)
        {
            Ensure.ArgumentNotNull(container, "container");
            Ensure.ArgumentNotNull(component, "component");

            _container = container;
            _component = component;
        }

        readonly IComponentCollection _container;
        public IComponentCollection Container
        {
            get
            {
                return _container;
            }
        }

        readonly IMappingComponent _component;
        public IMappingComponent Component
        {
            get
            {
                return _component;
            }
        }
    }
}
