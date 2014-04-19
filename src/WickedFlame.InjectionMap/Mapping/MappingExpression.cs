using System;
using System.Linq.Expressions;
using WickedFlame.InjectionMap.Mapping;

namespace WickedFlame.InjectionMap.Mapping
{
    internal class MappingExpression<T> : BindableComponent, IMappingExpression
    {
        public MappingExpression(IComponentContainer container, IMappingComponent component)
            : base(container, component)
        {
        }

        public IBindingExpression<T> For<T>()
        {
            if (!Component.KeyType.IsAssignableFrom(typeof(T)))
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

            return new BindingExpression<T>(Container, component);
        }

        public IBindingExpression<T> For<T>(T value)
        {
            if (!Component.KeyType.IsAssignableFrom(typeof(T)))
                throw new ArgumentException(string.Format("Type {0} has to implementen {1} to be assignable", typeof(T), Component.KeyType), "component");

            var component = new MappingComponent<T>(Component.ID)
            {
                KeyType = Component.KeyType,
                Value = value,
                MappingOption = Component.MappingOption
            };
            
            Container.AddOrReplace(component);
            if (component.MappingOption == null || !component.MappingOption.WithoutOverwrite)
            {
                Container.ReplaceAll(component);
            }

            return new BindingExpression<T>(Container, component);
        }

        public IBindingExpression<T> For<T>(Expression<Func<T>> callback)
        {
            //TODO: Callback should only be called when resolving
            return For<T>(callback.Compile().Invoke());
        }
    }
}
