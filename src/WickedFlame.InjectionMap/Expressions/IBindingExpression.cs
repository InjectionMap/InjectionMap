using System;
using System.Linq.Expressions;

namespace WickedFlame.InjectionMap.Expressions
{
    public interface IBindingExpression<T> : IComponentExpression
    {
        IBindingExpression<T> WithArgument<TArg>(TArg value);

        IBindingExpression<T> WithArgument<TArg>(string name, TArg value);

        IBindingExpression<T> WithArgument<TArg>(Expression<Func<TArg>> callback);

        IBindingExpression<T> WithArgument<TArg>(string name, Expression<Func<TArg>> callback);

        IBindingExpression<T> As(Expression<Func<T>> callback);

        IBoundExpression<T> WithOptions(InjectionFlags option);

        IBindingExpression<T> OnResolved(Action<T> callback);
    }
}
