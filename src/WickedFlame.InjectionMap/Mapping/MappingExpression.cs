using System;
using System.Linq.Expressions;
using WickedFlame.InjectionMap.Internals;

namespace WickedFlame.InjectionMap.Mapping
{
    internal class MappingExpression<T> : BindableComponent, IMappingExpression
    {
        public MappingExpression(IComponentContainer container, IMappingComponent component)
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
            //Ensure.ArgumentNotNull(Component.KeyType);

            return CreateBinding(Component.KeyType);
        }

        #endregion

        #region Implementation

        private IBindingExpression<T> CreateBinding<T>(Expression<Func<T>> callback)
        {
            if (!Component.KeyType.IsAssignableFrom(typeof(T)))
                throw new ArgumentException(string.Format("Type {0} has to implementen {1} to be assignable", typeof(T), Component.KeyType), "component");

            var component = new MappingComponent<T>(Component.ID)
            {
                KeyType = Component.KeyType,
                ValueCallback = callback,
                MappingOption = Component.MappingOption
            };

            Container.AddOrReplace(component);
            if (component.MappingOption == null || !component.MappingOption.WithoutOverwrite)
            {
                Container.ReplaceAll(component);
            }

            return new BindingExpression<T>(Container, component);
        }

        private IBindingExpression CreateBinding(Type type)
        {
            if (!Component.KeyType.IsAssignableFrom(type))
                throw new ArgumentException(string.Format("Type {0} has to implementen {1} to be assignable", typeof(T), Component.KeyType), "component");

            var component = new MappingComponent<T>(Component.ID)
            {
                KeyType = Component.KeyType,
                MappingOption = Component.MappingOption
            };

            Container.AddOrReplace(component);
            if (component.MappingOption == null || !component.MappingOption.WithoutOverwrite)
            {
                Container.ReplaceAll(component);
            }

            return new BindingExpression(Container, component);
        }

        #endregion
    }
}
