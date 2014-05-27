using System;
using System.Linq.Expressions;

namespace InjectionMap
{
    public interface IMappingComponent : IComponent
    {
        /// <summary>
        /// The predicate that gets executed to provide the value for the mapping
        /// </summary>
        Expression<Func<object>> ValueCallback { get; set; }

        Action<object> OnResolvedCallback { get; }
    }
}
