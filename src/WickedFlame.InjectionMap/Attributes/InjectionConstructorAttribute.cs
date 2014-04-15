using System;

namespace WickedFlame.InjectionMap
{
    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = true)]
    public class InjectionConstructorAttribute : Attribute
    {
    }
}
