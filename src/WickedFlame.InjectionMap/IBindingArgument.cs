using System;
using System.Linq.Expressions;

namespace WickedFlame.InjectionMap
{
    public interface IBindingArgument : IArgument
    {
        Expression<Func<object>> Callback { get; }
    }
}
