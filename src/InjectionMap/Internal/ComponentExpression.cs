using InjectionMap.Extensions;

namespace InjectionMap.Internal
{
    internal abstract class ComponentExpression : IComponentExpression
    {
        public ComponentExpression(IComponentCollection context, IMappingComponent component)
        {
            context.EnsureArgumentNotNull("context");
            component.EnsureArgumentNotNull("component");

            _context = context;
            _component = component;
        }

        readonly IComponentCollection _context;
        public virtual IComponentCollection Context
        {
            get
            {
                return _context;
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
