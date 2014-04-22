using System;
using System.Collections.Generic;

namespace WickedFlame.InjectionMap
{
    public interface IComponentProvider
    {
        IEnumerable<IMappingComponent> Get<T>();

        IEnumerable<IMappingComponent> Get(Type type);

        //IEnumerable<T> Get<T>(Func<bool, T> condition);

        /// <summary>
        /// Removes all mappings of type T
        /// </summary>
        /// <typeparam name="T">The type of mappings to remove</typeparam>
        void Clean<T>();
    }
}
