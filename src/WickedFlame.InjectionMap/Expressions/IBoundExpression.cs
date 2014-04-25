using System;
using System.Linq.Expressions;

namespace WickedFlame.InjectionMap.Expressions
{
    public interface IBoundExpression : IComponentExpression
    {

        IMappingOption MappingOption { get; }

        //IOptionExpression WithOptions(InjectionFlags option);

        IBoundExpression OnResolved<T>(Action<T> callback);
    }
}
