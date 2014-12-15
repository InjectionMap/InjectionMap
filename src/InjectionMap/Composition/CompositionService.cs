using System;

namespace InjectionMap.Composition
{
    /// <summary>
    /// A service class that provides funcuality to compose instances from a component or a type
    /// </summary>
    internal static class CompositionService
    {
        /// <summary>
        /// Compose the value from a component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component"></param>
        /// <returns></returns>
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

            using (var composition = new ObjectComposer())
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

        /// <summary>
        /// Compose the value from a component
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
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

            using (var composition = new ObjectComposer())
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

        /// <summary>
        /// Compose the value from a type
        /// </summary>
        /// <typeparam name="T">The type to compose</typeparam>
        /// <returns></returns>
        internal static T Compose<T>()
        {
            using (var composition = new ObjectComposer())
            {
                // compose instance
                return composition.Compose<T>();
            }
        }

        /// <summary>
        /// Compose the value from a type
        /// </summary>
        /// <param name="type">The type to compose</param>
        /// <returns></returns>
        internal static object Compose(Type type)
        {
            using (var composition = new ObjectComposer())
            {
                // compose instance
                return composition.Compose(type);
            }
        }
    }
}
