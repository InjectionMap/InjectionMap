using System;
using System.Linq.Expressions;
using WickedFlame.InjectionMap.Expressions;
using WickedFlame.InjectionMap.Internals;

namespace WickedFlame.InjectionMap.Mapping
{
    internal class BoundExpression : BindableComponent, IBoundExpression
    {
        public BoundExpression(IComponentCollection container, IMappingComponent component)
            : base(container, component)
        {
        }

        public IMappingOption MappingOption { get; internal set; }

        public IBoundExpression OnResolved<T>(Action<T> callback)
        {
            Ensure.MappingTypesMatch(Component.KeyType, typeof(T));

            var component = Component.CreateComponent<T>();
            component.OnResolvedCallback = callback;

            return component.CreateBoundExpression(Container);
        }
    }
}
