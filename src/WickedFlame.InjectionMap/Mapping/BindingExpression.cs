using System;

namespace WickedFlame.InjectionMap.Mapping
{
    internal class BindingExpression<T> : BindableComponent, IBindingExpression<T>
    {
        public BindingExpression(IComponentContainer container, IMappingComponent component)
            : base(container, component)
        {
        }

        public IBindingExpression<T> WithArgument(object value)
        {
            return AddArgument(null, value, null);
        }

        public IBindingExpression<T> WithArgument(string name, object value)
        {
            return AddArgument(name, value, null);
        }

        public IBindingExpression<T> WithArgument(string name, System.Linq.Expressions.Expression<Func<object>> argument)
        {
            return AddArgument(name, null, argument);
        }

        private IBindingExpression<T> AddArgument(string name, object value, System.Linq.Expressions.Expression<Func<object>> callback)
        {
            Component.Arguments.Add(new ConstructorArgument
            {
                Name = name,
                Value = value,
                Callback = callback
            });

            return new BindingExpression<T>(ComponentContainer, Component);
        }

        //public IOptionExpression WithOption(InjectionFlags options)
        //{
        //    if (options != null)
        //    {
        //        var expr = options.Compile().Invoke(CompileOptionExpression(component));
        //        component.MappingOption = expr != null && expr.MappingOption != null ? expr.MappingOption : new MappingOption();
        //    }

        //    if (expression.Component.MappingOption != null && expression.Component.MappingOption.ResolveInstanceOnMapping)
        //        expression = expression.For(CompositionService.Compose(expression.Component));
        //}

        public IBoundExpression WithOptions(InjectionFlags option)
        {
            var resolveInstanceOnMapping = (option & InjectionFlags.ResolveInstanceOnMapping) == InjectionFlags.ResolveInstanceOnMapping;
            var keepInstance = (option & InjectionFlags.KeepInstance) == InjectionFlags.KeepInstance;
            var withOverwrite = (option & InjectionFlags.WithOverwrite) == InjectionFlags.WithOverwrite;

            if (resolveInstanceOnMapping && !keepInstance)
                keepInstance = true;

            // remove previous instances...
            ComponentContainer.AddOrReplace(Component);
            if (withOverwrite)
            {
                ComponentContainer.ReplaceAll(Component);
            }

            return new BoundExpression(ComponentContainer, Component)
            {
                MappingOption = new MappingOption
                {
                    ResolveInstanceOnMapping = resolveInstanceOnMapping,
                    KeepInstance = keepInstance,
                    WithoutOverwrite = !withOverwrite
                }
            };
        }
    }
}
