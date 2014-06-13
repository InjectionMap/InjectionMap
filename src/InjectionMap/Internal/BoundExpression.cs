using System;
using InjectionMap.Expressions;
using InjectionMap.Extensions;

namespace InjectionMap.Internal
{
    internal class BoundExpression<T> : ComponentExpression, IBoundExpression<T>, IComponentExpression
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
            Component.KeyType.EnsureMappingTypeMatches(typeof(T));

            var component = Component.CreateComponent<T>();
            component.OnResolvedCallback = callback;

            return component.CreateBoundExpression<T>(Container);
        }
    }
}
