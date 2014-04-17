using System;
using System.Linq.Expressions;

namespace WickedFlame.InjectionMap
{
    public interface IArgument
    {
        string Name { get; }

        object Value { get; }

        Expression<Func<object>> Callback { get; }
    }
}
