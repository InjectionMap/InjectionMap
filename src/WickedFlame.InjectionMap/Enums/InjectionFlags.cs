using System;

namespace WickedFlame.InjectionMap
{
    [Flags]
    public enum InjectionFlags
    {
        None = 0,
        ResolveInstanceOnMapping = 1,
        KeepInstance = 2,
        WithOverwrite = 4
    }
}
