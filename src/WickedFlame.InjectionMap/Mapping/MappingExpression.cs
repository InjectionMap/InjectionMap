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

        public IBindingExpression<T> For<T>() where T : new()
        {
            if (!Component.Key.IsAssignableFrom(typeof(T)))
                throw new ArgumentException(string.Format("Type {0} has to implementen {1} to be assignable", typeof(T), Component.Key), "component");

            var component = new MappingComponent<T>(Component.ID)
            {
                Key = Component.Key,
                //ValueType = typeof(T),
                MappingOption = Component.MappingOption
            };

            //if (options != null)
            //{
            //    var expr = options.Compile().Invoke(CompileOptionExpression(component));
            //    component.MappingOption = expr != null && expr.MappingOption != null ? expr.MappingOption : new MappingOption();
            //}

            ComponentContainer.AddOrReplace(component);
            if (component.MappingOption == null || !component.MappingOption.WithoutOverwrite)
            {
                ComponentContainer.ReplaceAll(component);
            }

            //return new InjectionExpression(ComponentContainer, component);
            return new BindingExpression<T>(ComponentContainer, component);

            //return For<T>((Expression<Func<IOptionExpression, IOptionExpression>>)null);
        }

        //public IBindingExpression For<T>(Expression<Func<IOptionExpression, IOptionExpression>> options) where T : new()
        //{
        //    if (!Component.Key.IsAssignableFrom(typeof(T)))
        //        throw new ArgumentException(string.Format("Type {0} has to implementen {1} to be assignable", typeof(T), Component.Key), "component");

        //    var component = new MappingComponent<T>(Component.ID)
        //    {
        //        Key = Component.Key,
        //        ValueType = typeof(T),
        //        MappingOption = Component.MappingOption
        //    };

        //    if (options != null)
        //    {
        //        var expr = options.Compile().Invoke(CompileOptionExpression(component));
        //        component.MappingOption = expr != null && expr.MappingOption != null ? expr.MappingOption : new MappingOption();
        //    }

        //    ComponentContainer.AddOrReplace(component);
        //    if (component.MappingOption == null || !component.MappingOption.WithoutOverwrite)
        //    {
        //        ComponentContainer.ReplaceAll(component);
        //    }

        //    return new InjectionExpression(ComponentContainer, component);
        //}

        public IBindingExpression<T> For<T>(T value)
        {
            if (!Component.Key.IsAssignableFrom(typeof(T)))
                throw new ArgumentException(string.Format("Type {0} has to implementen {1} to be assignable", typeof(T), Component.Key), "component");

            var component = new MappingComponent<T>(Component.ID)
            {
                Key = Component.Key,
                //ValueType = value.GetType(),
                Value = value,
                MappingOption = Component.MappingOption
            };

            //if (options != null)
            //{
            //    var expr = options.Compile().Invoke(CompileOptionExpression(component));
            //    component.MappingOption = expr != null && expr.MappingOption != null ? expr.MappingOption : new MappingOption();
            //}

            ComponentContainer.AddOrReplace(component);
            if (component.MappingOption == null || !component.MappingOption.WithoutOverwrite)
            {
                ComponentContainer.ReplaceAll(component);
            }

            return new BindingExpression<T>(ComponentContainer, component);

            //return For<T>(value, null);
        }

        public IBindingExpression<T> For<T>(Expression<Func<T>> valueCallback)
        {
            return For<T>(valueCallback.Compile().Invoke());

            //return For<T>(valueCallback.Compile().Invoke(), null);
        }

        //public IInjectionExpression For<T>(T value, Expression<Func<IOptionExpression, IOptionExpression>> options)
        //{
        //    if (!Component.Key.IsAssignableFrom(typeof(T)))
        //        throw new ArgumentException(string.Format("Type {0} has to implementen {1} to be assignable", typeof(T), Component.Key), "component");

        //    var component = new MappingComponent<T>(Component.ID)
        //    {
        //        Key = Component.Key,
        //        ValueType = value.GetType(),
        //        Value = value,
        //        MappingOption = Component.MappingOption
        //    };

        //    if (options != null)
        //    {
        //        var expr = options.Compile().Invoke(CompileOptionExpression(component));
        //        component.MappingOption = expr != null && expr.MappingOption != null ? expr.MappingOption : new MappingOption();
        //    }

        //    ComponentContainer.AddOrReplace(component);
        //    if (component.MappingOption == null || !component.MappingOption.WithoutOverwrite)
        //    {
        //        ComponentContainer.ReplaceAll(component);
        //    }
            
        //    return new InjectionExpression(ComponentContainer, component);
        //}

        //public IInjectionExpression For<T>(Expression<Func<T>> valueCallback, Expression<Func<IOptionExpression, IOptionExpression>> options)
        //{
        //    return For<T>(valueCallback.Compile().Invoke(), options);
        //}

        //private IOptionExpression CompileOptionExpression(IMappingComponent component)
        //{
        //    return new OptionExpression(
        //    {
        //        MappingOption = component.MappingOption ?? new MappingOption()
        //    };
        //}
    }
}
