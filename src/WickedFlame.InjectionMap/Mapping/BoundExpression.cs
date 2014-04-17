using System;

namespace WickedFlame.InjectionMap.Mapping
{
    internal class BoundExpression : BindableComponent, IBoundExpression
    {
        public BoundExpression(IComponentContainer container, IMappingComponent component)
            : base(container, component)
        {
        }

        public IMappingOption MappingOption { get; internal set; }
    }
}
