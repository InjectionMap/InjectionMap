using System;

namespace WickedFlame.InjectionMap.Composition
{
    static class CompositionService
    {
        internal static T Compose<T>(IMappingComponent component)
        {
            if (component.ValueCallback != null)
                return (T)component.ValueCallback.Compile().Invoke();

            using (var composition = new CompositionContainer())
            {
                var value = composition.ComposePart<T>(component);
                if (component.MappingOption.CacheValue)
                    component.ValueCallback = () => value;

                return value;
            }
        }

        internal static object Compose(IMappingComponent component)
        {
            if (component.ValueCallback != null)
                return component.ValueCallback;

            using (var composition = new CompositionContainer())
            {
                var value = composition.ComposePart(component);
                if (component.MappingOption.CacheValue)
                    component.ValueCallback = () => value;

                return value;
            }
        }
    }
}
