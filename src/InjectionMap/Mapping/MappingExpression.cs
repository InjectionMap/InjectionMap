using System;
using System.Linq.Expressions;
using InjectionMap.Exceptions;
using InjectionMap.Expressions;
using InjectionMap.Internals;
using InjectionMap.Extensions;

namespace InjectionMap.Mapping
{
    internal class MappingExpression<T> : BindableComponent, IMappingExpression<T>
    {
        public MappingExpression(IComponentCollection container, IMappingComponent component)
            : base(container, component)
        {
        }

        #region IMappingExpression Implementation

        public IBindingExpression<TMap> For<TMap>()
        {
            return CreateBinding<TMap>(null);
        }

        public IBindingExpression<TMap> For<TMap>(TMap value)
        {
            return CreateBinding<TMap>(() => value);
        }

        public IBindingExpression<TMap> For<TMap>(Expression<Func<TMap>> callback)
        {
            return CreateBinding<TMap>(callback);
        }

        public IBindingExpression<T> ToSelf()
        {
            return CreateBinding();
        }

        public IMappingExpression<T> OnResolved(Action<T> callback)
        {
            var component = Component.CreateComponent<T>();
            component.OnResolvedCallback = callback;

            return component.CreateMappingExpression<T>(Container);
        }

        /// <summary>
        /// Adds a substitute mapping for the mapping
        /// </summary>
        /// <typeparam name="TMap">The implementing type of the substitue</typeparam>
        /// <returns>New IBindingExpression with the substitute</returns>
        public IBindingExpression<TMap> Substitute<TMap>()
        {
            Ensure.MappingTypesMatch(Component.KeyType, typeof(TMap));

            var component = Component.CreateComponent<TMap>();
            component.IsSubstitute = true;

            return component.CreateBindingExpression<TMap>(Container);
        }

        /// <summary>
        /// Adds a substitute mapping for the mapping
        /// </summary>
        /// <typeparam name="T">Key type to create a substitute for</typeparam>
        /// <param name="callback">Callback expression to generate substitute</param>
        /// <returns>New IBindingExpression with the substitute</returns>
        public IBindingExpression<T> Substitute(Expression<Func<T>> callback)
        {
            Ensure.MappingTypesMatch(Component.KeyType, typeof(T));

            var component = Component.CreateComponent<T>();
            component.IsSubstitute = true;
            component.ValueCallback = callback;

            return component.CreateBindingExpression<T>(Container);
        }


        #endregion

        #region Implementation

        internal IBindingExpression<TMap> CreateBinding<TMap>(Expression<Func<TMap>> callback)
        {
            Ensure.MappingTypesMatch(Component.KeyType, typeof(T));

            var component = Component.CreateComponent<TMap>();
            component.ValueCallback = callback;
            return component.CreateBindingExpression<TMap>(Container);
        }

        internal IBindingExpression<T> CreateBinding()
        {
            Ensure.MappingTypesMatch(Component.KeyType, typeof(T));

            var component = Component.CreateComponent<T>();
            return component.CreateBindingExpression<T>(Container);
        }

        #endregion
    }
}
