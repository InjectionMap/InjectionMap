using System;

namespace InjectionMap
{
    [Flags]
    public enum InjectionFlags
    {
        None = 0,
        ResolveInstanceOnMapping = 1,

        /// <summary>
        /// Caches the instance of the value
        /// </summary>
        CacheValue = 2,

        /// <summary>
        /// Overrides all existing mapings of the type
        /// </summary>
        AsSingleton = 4
    }
}
