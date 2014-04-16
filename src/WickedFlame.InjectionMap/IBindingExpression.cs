using System;
using System.Linq.Expressions;

namespace WickedFlame.InjectionMap
{
    public interface IBindingExpression<T>
    {
        IComponentContainer ComponentContainer { get; }

        IMappingComponent Component { get; }

        IBindingExpression<T> WithArgument(object value);

        IBindingExpression<T> WithArgument(string name, object value);

        IBindingExpression<T> WithArgument(string name, Expression<Func<object>> argument);

        IOptionExpression WithOptions(InjectionFlags option);
    }
}
