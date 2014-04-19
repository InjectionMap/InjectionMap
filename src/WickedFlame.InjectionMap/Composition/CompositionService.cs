using System;

namespace WickedFlame.InjectionMap.Composition
{
    static class CompositionService
    {
        //internal static T Compose<T>()
        //{
        //    using (var composition = new CompositionContainer())
        //    {
        //        return composition.ComposePart<T>();
        //    }
        //}

        internal static T Compose<T>(IMappingComponent component)
        {
            if (component.Value != null)
                return (T)component.Value;

            using (var composition = new CompositionContainer())
            {
                var value = composition.ComposePart<T>(component);
                if (component.MappingOption.KeepInstance)
                    component.Value = value;

                return value;
            }
        }

        //internal static object Compose(Type type)
        //{
        //    using (var composition = new CompositionContainer())
        //    {
        //        return composition.ComposePart(type);
        //    }
        //}

        internal static object Compose(IMappingComponent component)
        {
            if (component.Value != null)
                return component.Value;

            using (var composition = new CompositionContainer())
            {
                var value = composition.ComposePart(component);
                if (component.MappingOption.KeepInstance)
                    component.Value = value;

                return value;
            }
        }
    }
}
