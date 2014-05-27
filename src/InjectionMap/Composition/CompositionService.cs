using System;

namespace InjectionMap.Composition
{
    internal static class CompositionService
    {
        internal static T Compose<T>(IMappingComponent component)
        {
            if (component.ValueCallback != null)
            {
                // return callback if provided
                var value = (T)component.ValueCallback.Compile().Invoke();

                if (component.OnResolvedCallback != null)
                    component.OnResolvedCallback(value);

                return value;
            }

            using (var composition = new CompositionContainer())
            {
                // compose instance
                var value = composition.Compose<T>(component);

                if (component.MappingConfiguration.AsConstant)
                    component.ValueCallback = () => value;

                if (component.OnResolvedCallback != null)
                    component.OnResolvedCallback(value);

                return value;
            }
        }

        internal static object Compose(IMappingComponent component)
        {
            if (component.ValueCallback != null)
            {
                // return callback if provided
                var value = component.ValueCallback.Compile().Invoke();

                if (component.OnResolvedCallback != null)
                    component.OnResolvedCallback(value);

                return value;
            }

            using (var composition = new CompositionContainer())
            {
                // compose instance
                var value = composition.Compose(component);

                if (component.MappingConfiguration.AsConstant)
                    component.ValueCallback = () => value;

                if (component.OnResolvedCallback != null)
                    component.OnResolvedCallback(value);

                return value;
            }
        }

        internal static T Compose<T>()
        {
            using (var composition = new CompositionContainer())
            {
                // compose instance
                return composition.Compose<T>();
            }
        }

        internal static object Compose(Type type)
        {
            using (var composition = new CompositionContainer())
            {
                // compose instance
                return composition.Compose(type);
            }
        }
    }
}
