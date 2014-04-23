using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WickedFlame.InjectionMap.Exceptions;
using WickedFlame.InjectionMap.Expressions;
using WickedFlame.InjectionMap.Mapping;

namespace WickedFlame.InjectionMap.Substitution
{
    public static class IMappingExpressionExtension
    {
        public static IBindingExpression<T> Substitute<T>(this IMappingExpression expression)
        {
            return expression.CreateSubstituteBinding<T>(null);
        }

        public static IBindingExpression<T> Substitute<T>(this IMappingExpression expression, Expression<Func<T>> callback)
        {
            return expression.CreateSubstituteBinding<T>(callback);
        }


        internal static IBindingExpression<T> CreateSubstituteBinding<T>(this IMappingExpression expression, Expression<Func<T>> callback)
        {
            if (!expression.Component.KeyType.IsAssignableFrom(typeof(T)))
                throw new MappingMismatchException(typeof(T), expression.Component.KeyType);

            var component = new MappingComponent<T>(expression.Component.ID)
            {
                KeyType = expression.Component.KeyType,
                ValueCallback = callback,
                MappingOption = expression.Component.MappingOption,
                IsSubstitute = true
            };

            expression.Container.AddOrReplace(component);
            if (component.MappingOption == null || component.MappingOption.AsSingleton)
            {
                expression.Container.ReplaceAll(component);
            }

            return new BindingExpression<T>(expression.Container, component);
        }
    }
}
