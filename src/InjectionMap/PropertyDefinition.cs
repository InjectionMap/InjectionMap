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
        public Type KeyType { get; internal set; }

        /// <summary>
        /// The propertyinfo of the property that has to be injected
        /// </summary>
        internal PropertyInfo Property { get; set; }

        /// <summary>
        /// The delegate of setter of the property
        /// </summary>
        internal PropertySetterDelegate Setter { get; set; }
    }
}
