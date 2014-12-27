using System;
using System.Linq;

namespace InjectionMap.Internal
{
    /// <summary>
    /// A factory that tries to find the correct resolver for the component/type
    /// If a key is mapped as component a componentresolver is returned. If the key is not mapped a typeresolver is returned
    /// </summary>
    internal static class ResolverFactory
    {
        /// <summary>
        /// Gets the Resolver needed to resolve an instance of the key
        /// </summary>
        /// <typeparam name="T">The key to resolve</typeparam>
        /// <param name="context">The context that the map is stored in</param>
        /// <returns>A resolver that is needed to resolve the value from</returns>
        public static IResolver GetResolver<T>(IComponentProvider context)
        {
            if (context == null)
                context = MappingContextManager.MappingContext;

            // check if there is a map for the key
            var maps = context.Get<T>();
            if (maps.Any())
                return new ComponentResolver(context);

            // if the key is not mapped and is a class/struct resolve by creating an instance of the key
            if (!typeof(T).IsInterface)
                return new TypeResolver(context);

            return new ComponentResolver(context);
        }

        /// <summary>
        /// Gets the Resolver needed to resolve an instance of the key
        /// </summary>
        /// <param name="type">The key to resolve</param>
        /// <param name="context">The context that the map is stored in</param>
        /// <returns>A resolver that is needed to resolve the value from</returns>
        public static IResolver GetResolver(Type type, IComponentProvider context)
        {
            if (context == null)
                context = MappingContextManager.MappingContext;

            // check if there is a map for the key
            var maps = context.Get(type);
            if (maps.Any())
                return new ComponentResolver(context);

            // if the key is not mapped and is a class/struct resolve by creating an instance of the key
            if (!type.IsInterface)
                return new TypeResolver(context);

            return new ComponentResolver(context);
        }
    }
}
