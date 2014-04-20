using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WickedFlame.InjectionMap
{
    public interface IMappingComponent
    {
        Guid ID { get; }

        Type KeyType { get; }

        Type ValueType { get; }

        //object Value { get; set; }
        Expression<Func<object>> ValueCallback { get; set; }

        IMappingOption MappingOption { get; }

        IList<IBindingArgument> Arguments { get; }
    }

    //public interface IMappingComponent<T>
    //{
    //    Guid ID { get; }

    //    Type KeyType { get; }

    //    Type ValueType { get; }

    //    T Value { get; set; }

    //    IMappingOption MappingOption { get; }
    //}
}
