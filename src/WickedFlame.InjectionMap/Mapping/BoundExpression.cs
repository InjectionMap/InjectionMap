using System;
using System.Linq.Expressions;
using WickedFlame.InjectionMap.Expressions;
using WickedFlame.InjectionMap.Internals;

namespace WickedFlame.InjectionMap.Mapping
{
    internal class BoundExpression<T> : BindableComponent, IBoundExpression<T>
    {
        public BoundExpression(IComponentCollection container, IMappingComponent component)
            : base(container, component)
        {
        }

        public IMappingOption MappingOption { get; internal set; }

        public IBoundExpression<T> OnResolved(Action<T> callback)
        {
            Ensure.MappingTypesMatch(Component.KeyType, typeof(T));

            var component = Component.CreateComponent<T>();
            component.OnResolvedCallback = callback;

            return component.CreateBoundExpression<T>(Container);
        }
    }
}
