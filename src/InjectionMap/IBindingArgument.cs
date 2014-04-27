using System;
using System.Linq.Expressions;

namespace InjectionMap
{
    /// <summary>
    /// Represents an Argument that can be passed to a parameter optionaly with a callback expression
    /// </summary>
    public interface IBindingArgument : IArgument
    {
        /// <summary>
        /// Expression that gets executed when the value is passed to the parameter
        /// </summary>
        Expression<Func<object>> Callback { get; }
    }
}
