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
        /// Keeps a reference to the value after the first resolver and reuses the same value with every further resolving
        /// </summary>
        Singleton = 2,

        /// <summary>
        /// Overrides all existing mapings of the type
        /// </summary>
        OverrideAllExisting = 4
    }
}
