using System;

namespace InjectionMap
{
    [Flags]
    public enum InjectionFlags
    {
        None = 0,
        ResolveInstanceOnMapping = 1,
        KeepInstanceAlive = 2,
        AsSingleton = 4
    }
}
