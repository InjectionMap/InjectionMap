using System;

namespace InjectionMap
{
    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = true)]
    public class InjectionConstructorAttribute : Attribute
    {
    }
}
