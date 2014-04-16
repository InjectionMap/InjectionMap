using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            throw new NotImplementedException();
        }

        public IBindingExpression<T> WithArgument(string name, object value)
        {
            throw new NotImplementedException();
        }

        public IBindingExpression<T> WithArgument(string name, System.Linq.Expressions.Expression<Func<object>> argument)
        {
            throw new NotImplementedException();
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

        public IOptionExpression WithOptions(InjectionFlags option)
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

            return new OptionExpression(ComponentContainer, Component)
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
