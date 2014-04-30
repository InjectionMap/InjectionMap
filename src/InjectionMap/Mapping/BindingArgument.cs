using System;
using System.Linq.Expressions;

namespace InjectionMap.Mapping
{
    internal class BindingArgument<T> : IBindingArgument
    {
        public string Name { get; internal set; }

        public T Value { get; internal set; }

        object IArgument.Value
        {
            get
            {
                return Value;
            }
        }

        /// <summary>
        /// Predicate that gets executed when the value is passed to the parameter
        /// </summary>
        public Expression<Func<T>> Callback { get; internal set; }

        Expression<Func<object>> IBindingArgument.Callback
        {
            get
            {
                if (Callback == null)
                    return null;

                return () => Callback.Compile().Invoke();
            }
        }
    }
}
