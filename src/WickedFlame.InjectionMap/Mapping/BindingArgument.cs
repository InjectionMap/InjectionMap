using System;
using System.Linq.Expressions;

namespace WickedFlame.InjectionMap.Mapping
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

        //public Expression<Func<object>> Callback { get; internal set; }
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
