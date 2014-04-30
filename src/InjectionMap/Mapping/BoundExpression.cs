using System;
using InjectionMap.Expressions;
using InjectionMap.Extensions;
using InjectionMap.Internals;

namespace InjectionMap.Mapping
{
    internal class BoundExpression<T> : BindableComponent, IBoundExpression<T>
    {
        public BoundExpression(IComponentCollection container, IMappingComponent component)
            : base(container, component)
        {
        }

        /// <summary>
        /// Gets the configuration for the mapping
        /// </summary>
        public IMappingConfiguration MappingConfiguration { get; internal set; }

        public IBoundExpression<T> OnResolved(Action<T> callback)
        {
            Ensure.MappingTypesMatch(Component.KeyType, typeof(T));

            var component = Component.CreateComponent<T>();
            component.OnResolvedCallback = callback;

            return component.CreateBoundExpression<T>(Container);
        }
    }
}
