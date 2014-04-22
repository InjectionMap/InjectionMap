using System;
using System.Linq.Expressions;

namespace WickedFlame.InjectionMap
{
    public interface IBindingExpression : IComponentExpression
    {
        IBindingExpression WithArgument<TArg>(TArg value);

        IBindingExpression WithArgument<TArg>(string name, TArg value);

        IBindingExpression WithArgument<TArg>(Expression<Func<TArg>> callback);

        IBindingExpression WithArgument<TArg>(string name, Expression<Func<TArg>> callback);

        IBindingExpression<T> As<T>(Expression<Func<T>> callback);

        IBoundExpression WithOptions(InjectionFlags option);
    }

    public interface IBindingExpression<T> : IComponentExpression
    {
        IBindingExpression<T> WithArgument<TArg>(TArg value);

        IBindingExpression<T> WithArgument<TArg>(string name, TArg value);

        IBindingExpression<T> WithArgument<TArg>(Expression<Func<TArg>> callback);

        IBindingExpression<T> WithArgument<TArg>(string name, Expression<Func<TArg>> callback);

        IBindingExpression<T> As<T>(Expression<Func<T>> callback);

        IBoundExpression WithOptions(InjectionFlags option);
    }
}
