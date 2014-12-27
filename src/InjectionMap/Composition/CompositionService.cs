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
        /// <param name="context"></param>
        /// <returns></returns>
        internal static T Compose<T>(IMappingComponent component, IComponentProvider context)
        {
            if (component.ValueCallback != null)
            {
                // return callback if provided
                var value = (T)component.ValueCallback.Compile().Invoke();

                // make sure the composed value is reused next time when mapped as constant
                if (component.MappingConfiguration.AsConstant)
                    component.ValueCallback = () => value;

                if (component.OnResolvedCallback != null)
                    component.OnResolvedCallback(value);

                return value;
            }

            using (var composition = new ObjectComposer(context))
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
        /// <param name="context"></param>
        /// <returns></returns>
        internal static object Compose(IMappingComponent component, IComponentProvider context)
        {
            if (component.ValueCallback != null)
            {
                // return callback if provided
                var value = component.ValueCallback.Compile().Invoke();

                // make sure the composed value is reused next time when mapped as constant
                if (component.MappingConfiguration.AsConstant)
                    component.ValueCallback = () => value;

                if (component.OnResolvedCallback != null)
                    component.OnResolvedCallback(value);

                return value;
            }

            using (var composition = new ObjectComposer(context))
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
        /// <param name="context"></param>
        /// <returns></returns>
        internal static T Compose<T>(IComponentProvider context)
        {
            using (var composition = new ObjectComposer(context))
            {
                // compose instance
                return composition.Compose<T>();
            }
        }

        /// <summary>
        /// Compose the value from a type
        /// </summary>
        /// <param name="type">The type to compose</param>
        /// <param name="context"></param>
        /// <returns></returns>
        internal static object Compose(Type type, IComponentProvider context)
        {
            using (var composition = new ObjectComposer(context))
            {
                // compose instance
                return composition.Compose(type);
            }
        }
    }
}
