using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using WickedFlame.InjectionMap.Mapping;

namespace WickedFlame.InjectionMap
{
    public class InjectionMapper : IDisposable
    {
        public void InitializeMappings(Assembly assembly)
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


        public IMappingExpression Map<TSvc>()
        {
            return MappingManager.MappingContainer.Map<TSvc>();
        }

        public IBindingExpression<TImpl> Map<TSvc, TImpl>() where TImpl : TSvc
        {
            return MappingManager.MappingContainer.Map<TSvc, TImpl>();
        }

        /// <summary>
        /// Removes all mappings of type T
        /// </summary>
        /// <typeparam name="T">The type of mappings to remove</typeparam>
        public void Clean<T>()
        {
            MappingManager.Clean<T>();
        }

        public void Dispose()
        {
        }
    }
}
