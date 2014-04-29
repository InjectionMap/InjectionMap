using System;
using System.Linq.Expressions;
using InjectionMap.Expressions;
using InjectionMap.Internals;
using InjectionMap.Extensions;

namespace InjectionMap.Mapping
{
    internal class BindingExpression<T> : BindableComponent, IBindingExpression<T>
    {
        public BindingExpression(IComponentCollection container, IMappingComponent component)
            : base(container, component)
        {
        }

        #region IBindingExpression<T> Implementation

        public IBindingExpression<T> WithArgument<TArg>(TArg value)
        {
            return AddArgument<TArg>(null, value, null);
        }

        public IBindingExpression<T> WithArgument<TArg>(string name, TArg value)
        {
            return AddArgument<TArg>(name, value, null);
        }

        public IBindingExpression<T> WithArgument<TArg>(Expression<Func<TArg>> argument)
        {
            return AddArgument<TArg>(null, default(TArg), argument);
        }

        public IBindingExpression<T> WithArgument<TArg>(string name, Expression<Func<TArg>> argument)
        {
            return AddArgument<TArg>(name, default(TArg), argument);
        }

        public IBindingExpression<T> As(Expression<Func<T>> callback)
        {
            return new MappingExpression<T>(Container, Component).For<T>(callback);
        }

        public IBoundExpression<T> WithOptions(InjectionFlags option)
        {
            var resolveInstanceOnMapping = (option & InjectionFlags.ResolveInstanceOnMapping) == InjectionFlags.ResolveInstanceOnMapping;
            var cacheValue = (option & InjectionFlags.CacheValue) == InjectionFlags.CacheValue;
            var asSingleton = (option & InjectionFlags.AsSingleton) == InjectionFlags.AsSingleton;

            if (resolveInstanceOnMapping && !cacheValue)
                cacheValue = true;

            // remove previous instances...
            Container.AddOrReplace(Component);
            if (asSingleton)
            {
                Container.ReplaceAll(Component);
            }

            return new BoundExpression<T>(Container, Component)
            {
                MappingOption = new MappingOption
                {
                    ResolveInstanceOnMapping = resolveInstanceOnMapping,
                    CacheValue = cacheValue,
                    AsSingleton = asSingleton
                }
            };
        }

        public IBindingExpression<T> OnResolved(Action<T> callback)
        {
            Ensure.MappingTypesMatch(Component.KeyType, typeof(T));

            var component = Component.CreateComponent<T>();
            component.OnResolvedCallback = callback;

            return component.CreateBindingExpression<T>(Container);
        }

        #endregion

        #region Private Implementation

        private IBindingExpression<T> AddArgument<TArg>(string name, TArg value, Expression<Func<TArg>> callback)
        {
            Component.Arguments.Add(new BindingArgument<TArg>
            {
                Name = name,
                Value = value,
                Callback = callback
            });

            return new BindingExpression<T>(Container, Component);
        }

        #endregion
    }
}
