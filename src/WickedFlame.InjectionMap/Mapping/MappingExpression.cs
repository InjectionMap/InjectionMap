using System;
using System.Linq.Expressions;
using WickedFlame.InjectionMap.Exceptions;
using WickedFlame.InjectionMap.Expressions;
using WickedFlame.InjectionMap.Internals;

namespace WickedFlame.InjectionMap.Mapping
{
    internal class MappingExpression<T> : BindableComponent, IMappingExpression<T>
    {
        public MappingExpression(IComponentCollection container, IMappingComponent component)
            : base(container, component)
        {
        }

        #region IMappingExpression Implementation

        public IBindingExpression<TImpl> For<TImpl>()
        {
            return CreateBinding<TImpl>(null);
        }

        public IBindingExpression<TImpl> For<TImpl>(TImpl value)
        {
            return CreateBinding<TImpl>(() => value);
        }

        public IBindingExpression<TImpl> For<TImpl>(Expression<Func<TImpl>> callback)
        {
            return CreateBinding<TImpl>(callback);
        }

        public IBindingExpression<T> ToSelf()
        {
            return CreateBinding(Component.KeyType);
        }

        public IMappingExpression<T> OnResolved(Action<T> callback)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a substitute mapping for the mapping
        /// </summary>
        /// <typeparam name="TImpl">The implementing type of the substitue</typeparam>
        /// <returns>New IBindingExpression with the substitute</returns>
        public IBindingExpression<TImpl> Substitute<TImpl>()
        {
            if (!Component.KeyType.IsAssignableFrom(typeof (TImpl)))
                throw new MappingMismatchException(typeof (TImpl), Component.KeyType);

            var component = new MappingComponent<TImpl>(Component.ID)
            {
                KeyType = Component.KeyType,
                MappingOption = Component.MappingOption,
                IsSubstitute = true
            };

            Container.AddOrReplace(component);
            if (component.MappingOption == null || component.MappingOption.AsSingleton)
            {
                Container.ReplaceAll(component);
            }

            return new BindingExpression<TImpl>(Container, component);
        }

        /// <summary>
        /// Adds a substitute mapping for the mapping
        /// </summary>
        /// <typeparam name="T">Key type to create a substitute for</typeparam>
        /// <param name="callback">Callback expression to generate substitute</param>
        /// <returns>New IBindingExpression with the substitute</returns>
        public IBindingExpression<T> Substitute(Expression<Func<T>> callback)
        {
            var component = new MappingComponent<T>(Component.ID)
            {
                KeyType = Component.KeyType,
                ValueCallback = callback,
                MappingOption = Component.MappingOption,
                IsSubstitute = true
            };

            Container.AddOrReplace(component);
            if (component.MappingOption == null || component.MappingOption.AsSingleton)
            {
                Container.ReplaceAll(component);
            }

            return new BindingExpression<T>(Container, component);
        }


        #endregion

        #region Implementation

        internal IBindingExpression<TImpl> CreateBinding<TImpl>(Expression<Func<TImpl>> callback)
        {
            if (!Component.KeyType.IsAssignableFrom(typeof(T)))
                throw new MappingMismatchException(typeof(T), Component.KeyType);

            var component = new MappingComponent<TImpl>(Component.ID)
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

            return new BindingExpression<TImpl>(Container, component);
        }

        internal IBindingExpression<T> CreateBinding(Type type)
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

            return new BindingExpression<T>(Container, component);
        }

        #endregion
    }
}
