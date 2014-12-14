using System;
using System.Linq.Expressions;
using InjectionMap.Extensions;

namespace InjectionMap.Internal
{
    internal class MappingExpression<T> : ComponentExpression, IMappingExpression<T>, IComponentExpression
    {
        public MappingExpression(IComponentCollection context, IMappingComponent component)
            : base(context, component)
        {
        }

        #region IMappingExpression Implementation

        /// <summary>
        /// Creates a mapping to the type TMap that gets composed when resolving
        /// </summary>
        /// <typeparam name="TMap">The mapped type</typeparam>
        /// <returns>A IBindingExpression of TMap</returns>
        public IBindingExpression<TMap> For<TMap>()
        {
            return CreateBinding<TMap>(null);
        }

        /// <summary>
        /// Creates a mapping to the value passed as parameter
        /// </summary>
        /// <typeparam name="TMap">The mapped type</typeparam>
        /// <param name="value">The value that gets mapped</param>
        /// <returns>A IBindingExpression of TMap</returns>
        public IBindingExpression<TMap> For<TMap>(TMap value)
        {
            return CreateBinding<TMap>(() => value);
        }

        /// <summary>
        /// Creates a mapping to the return value of the expression
        /// </summary>
        /// <typeparam name="TMap">The mapped type</typeparam>
        /// <param name="predicate">Expression that gets called to return the value for the mapping. The expression gets called everytime the mapping is resolved</param>
        /// <returns>A IBindingExpression of TMap</returns>
        public IBindingExpression<TMap> For<TMap>(Expression<Func<TMap>> predicate)
        {
            return CreateBinding<TMap>(predicate);
        }

        /// <summary>
        /// Creates a binding to the key type
        /// </summary>
        /// <returns>A IBindingExpression of T</returns>
        public IBindingExpression<T> ToSelf()
        {
            return CreateBinding();
        }

        /// <summary>
        /// Maps a expression that gets executed when the component has been resolved
        /// </summary>
        /// <param name="callback">The Action to execute after resolving</param>
        /// <returns>A IMappingExpression of T</returns>
        public IMappingExpression<T> OnResolved(Action<T> callback)
        {
            var component = Component.CreateComponent<T>();
            component.OnResolvedCallback = callback;

            return component.CreateMappingExpression<T>(Context);
        }

        /// <summary>
        /// Adds a substitute mapping for the mapping
        /// </summary>
        /// <typeparam name="TMap">The implementing type of the substitue</typeparam>
        /// <returns>New IBindingExpression with the substitute</returns>
        public IBindingExpression<TMap> Substitute<TMap>()
        {
            Component.KeyType.EnsureMappingTypeMatches(typeof(TMap));

            var component = Component.CreateComponent<TMap>();
            component.IsSubstitute = true;

            return component.CreateBindingExpression<TMap>(Context);
        }

        /// <summary>
        /// Adds a substitute mapping for the mapping
        /// </summary>
        /// <typeparam name="T">Key type to create a substitute for</typeparam>
        /// <param name="predicate">Callback expression to generate substitute</param>
        /// <returns>New IBindingExpression with the substitute</returns>
        public IBindingExpression<T> Substitute(Expression<Func<T>> predicate)
        {
            Component.KeyType.EnsureMappingTypeMatches(typeof(T));

            var component = Component.CreateComponent<T>();
            component.IsSubstitute = true;
            component.ValueCallback = predicate;

            return component.CreateBindingExpression<T>(Context);
        }


        #endregion

        #region Implementation

        internal IBindingExpression<TMap> CreateBinding<TMap>(Expression<Func<TMap>> predicate)
        {
            Component.KeyType.EnsureMappingTypeMatches(typeof(T));

            var component = Component.CreateComponent<TMap>();
            component.ValueCallback = predicate;
            return component.CreateBindingExpression<TMap>(Context);
        }

        internal IBindingExpression<T> CreateBinding()
        {
            Component.KeyType.EnsureMappingTypeMatches(typeof(T));

            var component = Component.CreateComponent<T>();
            return component.CreateBindingExpression<T>(Context);
        }

        #endregion
    }
}
