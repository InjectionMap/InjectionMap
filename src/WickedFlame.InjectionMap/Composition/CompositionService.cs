using System;

namespace WickedFlame.InjectionMap.Composition
{
    static class CompositionService
    {
        internal static T Compose<T>()
        {
            using (var composition = new CompositionContainer())
            {
                return composition.ComposePart<T>();
            }
        }

        internal static T Compose<T>(IMappingComponent c)
        {
            if (c.Value != null)
                return (T)c.Value;

            var value = Compose<T>();
            if (c.MappingOption.KeepInstance)
                c.Value = value;

            return (T)value;
        }

        internal static object Compose(Type type)
        {
            using (var composition = new CompositionContainer())
            {
                return composition.ComposePart(type);
            }
        }

        internal static object Compose(IMappingComponent c)
        {
            if (c.Value != null)
                return c.Value;

            var value = Compose(c.ValueType);
            if (c.MappingOption.KeepInstance)
                c.Value = value;

            return value;
        }
    }
}
