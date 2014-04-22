using System;
using System.Linq.Expressions;

namespace WickedFlame.InjectionMap
{
    public interface IMappingExpression : IComponentExpression
    {
        IBindingExpression<T> For<T>();
        
        IBindingExpression<T> For<T>(T value);

        IBindingExpression<T> For<T>(Expression<Func<T>> callback);

        IBindingExpression ToSelf();
    }
}
