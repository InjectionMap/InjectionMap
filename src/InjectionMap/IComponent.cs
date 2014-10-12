using System;
using System.Collections.Generic;

namespace InjectionMap
{
    public interface IComponent : IDisposable
    {
        Guid ID { get; }

        Type KeyType { get; }

        Type ValueType { get; }
        
        IMappingConfiguration MappingConfiguration { get; }

        IList<IBindingArgument> Arguments { get; }

        bool IsSubstitute { get; }

        IConstructorDefinition ConstructorDefinition { get; }
    }
}
