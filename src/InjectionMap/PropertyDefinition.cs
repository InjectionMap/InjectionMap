using InjectionMap.Composition;
using System;
using System.Reflection;

namespace InjectionMap
{
    /// <summary>
    /// Defines a property that can be injected
    /// </summary>
    public class PropertyDefinition
    {
        /// <summary>
        /// The key that the mapping is registered to
        /// </summary>
        public Type KeyType { get; set; }

        /// <summary>
        /// The propertyinfo of the property that has to be injected
        /// </summary>
        public PropertyInfo Property { get; set; }

        /// <summary>
        /// The delegate of setter of the property
        /// </summary>
        public PropertySetterDelegate Setter { get; set; }
    }
}
