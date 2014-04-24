using System;
using System.Linq.Expressions;
using WickedFlame.InjectionMap.Exceptions;
using WickedFlame.InjectionMap.Expressions;
using WickedFlame.InjectionMap.Internals;

namespace WickedFlame.InjectionMap.Mapping
{
    internal class MappingExpression<T> : BindableComponent, IMappingExpression
    {
        public MappingExpression(IComponentCollection container, IMappingComponent component)
            : base(container, component)
        {
        }

        #region IMappingExpression Implementation

        public IBindingExpression<T> For<T>()
        {
            return CreateBinding<T>(null);
        }

        public IBindingExpression<T> For<T>(T value)
        {
            return CreateBinding<T>(() => value);
        }

        public IBindingExpression<T> For<T>(Expression<Func<T>> callback)
        {
            return CreateBinding<T>(callback);
        }

        public IBindingExpression ToSelf()
        {
            return CreateBinding(Component.KeyType);
        }

        #endregion

        #region Implementation

        internal IBindingExpression<T> CreateBinding<T>(Expression<Func<T>> callback)
        {
            if (!Component.KeyType.IsAssignableFrom(typeof(T)))
                throw new MappingMismatchException(typeof(T), Component.KeyType);

            var component = new MappingComponent<T>(Component.ID)
            {
                KeyType = Component.KeyType,
                ValueCallback = callback,
                MappingOption = Component.MappingOption,
                IsSubstitute = Component.IsSubstitute
            };

            Component.Arguments.ForEach(a => component.Arguments.Add(a));

            Container.AddOrReplace(component);
            if (component.MappingOption == null || component.MappingOption.AsSingleton)
            {
                Container.ReplaceAll(component);
            }

            return new BindingExpression<T>(Container, component);
        }

        internal IBindingExpression CreateBinding(Type type)
        {
            if (!Component.KeyType.IsAssignableFrom(type))
                throw new MappingMismatchException(type, Component.KeyType);

            var component = new MappingComponent<T>(Component.ID)
            {
                KeyType = Component.KeyType,
                MappingOption = Component.MappingOption,
                IsSubstitute = Component.IsSubstitute
            };

            Component.Arguments.ForEach(a => component.Arguments.Add(a));

            Container.AddOrReplace(component);
            if (component.MappingOption == null || component.MappingOption.AsSingleton)
            {
                Container.ReplaceAll(component);
            }

            return new BindingExpression(Container, component);
        }

        #endregion
    }
}
