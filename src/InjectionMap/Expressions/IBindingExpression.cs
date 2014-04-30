using System;
using System.Linq.Expressions;

namespace InjectionMap.Expressions
{
    public interface IBindingExpression<T> : IComponentExpression
    {
        IBindingExpression<T> WithArgument<TArg>(TArg value);

        IBindingExpression<T> WithArgument<TArg>(string name, TArg value);

        IBindingExpression<T> WithArgument<TArg>(Expression<Func<TArg>> predicate);

        IBindingExpression<T> WithArgument<TArg>(string name, Expression<Func<TArg>> predicate);

        IBindingExpression<T> As(Expression<Func<T>> predicate);

        IBoundExpression<T> WithConfiguration(InjectionFlags option);

        IBindingExpression<T> OnResolved(Action<T> callback);
    }
}
