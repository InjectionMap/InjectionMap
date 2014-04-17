using System;
using System.Collections.Generic;

namespace WickedFlame.InjectionMap
{
    public interface IMappingComponent
    {
        Guid ID { get; }

        Type Key { get; }

        Type ValueType { get; }

        object Value { get; set; }

        IMappingOption MappingOption { get; }

        IList<IArgument> Arguments { get; }

        //IMappingComponent For<T>() where T : new();

        //IMappingComponent For<T>(T value);

        //IMappingComponent Options(InjectionOption option);
    }

    //public interface IMappingComponent<T>
    //{
    //    Guid ID { get; }

    //    Type Key { get; }

    //    Type ValueType { get; }

    //    T Value { get; set; }

    //    IMappingOption MappingOption { get; }

    //    //IMappingComponent For<T>() where T : new();

    //    //IMappingComponent For<T>(T value);

    //    //IMappingComponent Options(InjectionOption option);
    //}
}
