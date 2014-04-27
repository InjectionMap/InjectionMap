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

        Expression<Func<object>> ValueCallback { get; set; }

        Action<object> OnResolvedCallback { get; }

        IMappingOption MappingOption { get; }

        IList<IBindingArgument> Arguments { get; }

        bool IsSubstitute { get; }
    }
}
