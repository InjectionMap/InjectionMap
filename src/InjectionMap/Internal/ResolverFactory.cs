using System;
using System.Linq;

namespace InjectionMap.Internal
{
    static class ResolverFactory
    {
        public static IResolver GetResolver<T>(IComponentProvider context)
        {
            if (context == null)
                context = MappingContextManager.MappingContext;

            var maps = context.Get<T>();
            if (maps.Any())
                return new ComponentResolver(context);

            if (!typeof(T).IsInterface)
                return new BindingResolver();

            return new ComponentResolver(context);
        }

        public static IResolver GetResolver(Type type, IComponentProvider context)
        {
            if (context == null)
                context = MappingContextManager.MappingContext;

            var maps = context.Get(type);
            if (maps.Any())
                return new ComponentResolver(context);

            if (!type.IsInterface)
                return new BindingResolver();

            return new ComponentResolver(context);
        }
    }
}
