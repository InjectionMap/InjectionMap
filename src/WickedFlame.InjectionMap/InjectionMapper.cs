using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using WickedFlame.InjectionMap.Mapping;

namespace WickedFlame.InjectionMap
{
    public static class InjectionMapper
    {
        public static void InitializeMappings(Assembly assembly)
        {
            var type = typeof(IInjectionMapping);
            var types = assembly.GetTypes().Where(p => type.IsAssignableFrom(p) && !p.IsInterface);

            foreach (var t in types)
            {
                var obj = Activator.CreateInstance(t) as IInjectionMapping;
                if (obj == null)
                    continue;

                obj.Register(MappingManager.MappingContainer);
            }
        }


        public static IMappingExpression Map<TSvc>()
        {
            return MappingManager.MappingContainer.Map<TSvc>();
        }

        //public static IInjectionExpression Map<TSvc>(Expression<Func<IInjectionExpression, IInjectionExpression>> action)
        //{
        //    return MappingManager.MappingContainer.Map<TSvc>(action);
        //}

        public static IMappingExpression Map<TSvc, TImpl>() where TImpl : TSvc, new()
        {
            return MappingManager.MappingContainer.Map<TSvc, TImpl>();
        }

        //public static IInjectionExpression Map<TSvc, TImpl>(Expression<Func<IInjectionExpression, IInjectionExpression>> action) where TImpl : TSvc, new()
        //{
        //    return MappingManager.MappingContainer.Map<TSvc, TImpl>(action);
        //}
    }
}
