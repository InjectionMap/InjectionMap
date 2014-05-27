using InjectionMap.Mapping;
using System;
using System.Linq;

namespace InjectionMap.Resolving
{
    static class ResolverFactory
    {
        public static IResolver GetResolver<T>(IComponentProvider container)
        {
            if (container == null)
                container = MappingContainerManager.MappingContainer;

            var maps = container.Get<T>();
            if (maps.Any()) 
                return new ComponentResolver(container);

            if (!typeof(T).IsInterface)
                return new BindingResolver();

            return new ComponentResolver(container);
        }

        public static IResolver GetResolver(Type type, IComponentProvider container)
        {
            if (container == null)
                container = MappingContainerManager.MappingContainer;

            var maps = container.Get(type);
            if (maps.Any())
                return new ComponentResolver(container);

            if (!type.IsInterface)
                return new BindingResolver();

            return new ComponentResolver(container);
        }
    }
}
