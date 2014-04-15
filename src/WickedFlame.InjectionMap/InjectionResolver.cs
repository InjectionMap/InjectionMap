using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WickedFlame.InjectionMap.Composition;
using WickedFlame.InjectionMap.Mapping;

namespace WickedFlame.InjectionMap
{
    public static class InjectionResolver
    {
        /// <summary>
        /// Resolves the first found occurance of T
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <returns>First found mapping of T</returns>
        public static T Resolve<T>()
        {
            return MappingManager.Get<T>();
        }

        /// <summary>
        /// Resolves the first found occurance of the type
        /// </summary>
        /// <param name="type">the type to resolve</param>
        /// <returns>First found mappinf of type</returns>
        public static object Resolve(Type type)
        {
            return MappingManager.Get(type);
        }

        /// <summary>
        /// Resolves all mappings of type T
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <returns>All mappings of T</returns>
        public static IEnumerable<T> ResolveMultiple<T>()
        {
            return MappingManager.GetAll<T>();
        }

        /// <summary>
        /// Removes all mappings of type T
        /// </summary>
        /// <typeparam name="T">The type of mappings to remove</typeparam>
        public static void Clean<T>()
        {
            MappingManager.Clean<T>();
        }
    }
}
