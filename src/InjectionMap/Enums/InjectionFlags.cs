using System;

namespace InjectionMap
{
    [Flags]
    public enum InjectionFlags
    {
        None = 0,

        /// <summary>
        /// Resolves the value when creating the mapping
        /// </summary>
        ResolveValueOnMapping = 1,

        /// <summary>
        /// Stores the value of the mapping and reuses the same value with every resolving
        /// </summary>
        AsConstant = 2,

        /// <summary>
        /// Overrides all existing mapings of the type
        /// </summary>
        AsSingleton = 4
    }
}
