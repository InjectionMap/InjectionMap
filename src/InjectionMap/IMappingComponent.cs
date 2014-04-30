using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace InjectionMap
{
    public interface IMappingComponent
    {
        Guid ID { get; }

        Type KeyType { get; }

        Type ValueType { get; }

        /// <summary>
        /// The predicate that gets executed to provide the value for the mapping
        /// </summary>
        Expression<Func<object>> ValueCallback { get; set; }

        Action<object> OnResolvedCallback { get; }

        IMappingConfiguration MappingConfiguration { get; }

        IList<IBindingArgument> Arguments { get; }

        bool IsSubstitute { get; }
    }
}
