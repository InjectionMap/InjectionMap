using System;
using System.Linq.Expressions;

namespace WickedFlame.InjectionMap.Mapping
{
    internal class ConstructorArgument : IArgument
    {
        public string Name { get; internal set; }

        public object Value { get; internal set; }

        public Expression<Func<object>> Callback { get; internal set; }
    }
}
