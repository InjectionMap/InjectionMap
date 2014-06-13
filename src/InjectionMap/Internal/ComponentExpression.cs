using InjectionMap.Expressions;
using InjectionMap.Extensions;

namespace InjectionMap.Internal
{
    internal abstract class ComponentExpression : IComponentExpression
    {
        public ComponentExpression(IComponentCollection container, IMappingComponent component)
        {
            container.EnsureArgumentNotNull("container");
            component.EnsureArgumentNotNull("component");

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
