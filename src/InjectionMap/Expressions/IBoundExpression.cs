using System;
using System.Linq.Expressions;

namespace InjectionMap.Expressions
{
    public interface IBoundExpression<T> : IComponentExpression
    {

        IMappingOption MappingOption { get; }

        //IOptionExpression WithOptions(InjectionFlags option);

        IBoundExpression<T> OnResolved(Action<T> callback);
    }
}
