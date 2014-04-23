using System;
using WickedFlame.InjectionMap.Expressions;

namespace WickedFlame.InjectionMap.Mapping
{
    internal class BoundExpression : BindableComponent, IBoundExpression
    {
        public BoundExpression(IComponentCollection container, IMappingComponent component)
            : base(container, component)
        {
        }

        public IMappingOption MappingOption { get; internal set; }
    }
}
