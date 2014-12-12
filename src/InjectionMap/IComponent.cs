using System;
using System.Collections.Generic;

namespace InjectionMap
{
    /// <summary>
    /// Defines a component that containes the definitions of the mapping
    /// </summary>
    public interface IComponent : IDisposable
    {
        /// <summary>
        /// The identification of the mapping
        /// </summary>
        Guid ID { get; }

        /// <summary>
        /// The key that the mapping is registered to
        /// </summary>
        Type KeyType { get; }

        /// <summary>
        /// The type that will be resilved from the mapping
        /// </summary>
        Type ValueType { get; }
        
        /// <summary>
        /// The configuration
        /// </summary>
        IMappingConfiguration MappingConfiguration { get; }

        /// <summary>
        /// A list of arguments that will be passed to the constructor when resolving
        /// </summary>
        IList<IBindingArgument> Arguments { get; }

        /// <summary>
        /// Gets a list of properties that will be injected when resolving the type
        /// </summary>
        IList<PropertyDefinition> Properies { get; }

        /// <summary>
        /// Defines if the component is a substitue for all other mappings of the same key
        /// </summary>
        bool IsSubstitute { get; }

        /// <summary>
        /// Gets the consturctor that will be injected into
        /// </summary>
        IConstructorDefinition ConstructorDefinition { get; }
    }
}
