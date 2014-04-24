using System;
using System.Linq.Expressions;
using WickedFlame.InjectionMap.Exceptions;
using WickedFlame.InjectionMap.Expressions;
using WickedFlame.InjectionMap.Mapping;

namespace WickedFlame.InjectionMap.Substitution
{
    public static class IMappingExpressionExtension
    {
        //public static IBindingExpression<TImpl> Substitute<T, TImpl>(this IMappingExpression<T> expression)
        //{
        //    if (!expression.Component.KeyType.IsAssignableFrom(typeof(TImpl)))
        //        throw new MappingMismatchException(typeof(TImpl), expression.Component.KeyType);

        //    var component = new MappingComponent<TImpl>(expression.Component.ID)
        //    {
        //        KeyType = expression.Component.KeyType,
        //        MappingOption = expression.Component.MappingOption,
        //        IsSubstitute = true
        //    };

        //    expression.Container.AddOrReplace(component);
        //    if (component.MappingOption == null || component.MappingOption.AsSingleton)
        //    {
        //        expression.Container.ReplaceAll(component);
        //    }

        //    return new BindingExpression<TImpl>(expression.Container, component);
        //}

        //public static IBindingExpression<T> Substitute<T>(this IMappingExpression<T> expression, Expression<Func<T>> callback)
        //{
        //    var component = new MappingComponent<T>(expression.Component.ID)
        //    {
        //        KeyType = expression.Component.KeyType,
        //        ValueCallback = callback,
        //        MappingOption = expression.Component.MappingOption,
        //        IsSubstitute = true
        //    };

        //    expression.Container.AddOrReplace(component);
        //    if (component.MappingOption == null || component.MappingOption.AsSingleton)
        //    {
        //        expression.Container.ReplaceAll(component);
        //    }

        //    return new BindingExpression<T>(expression.Container, component);
        //}
    }
}
