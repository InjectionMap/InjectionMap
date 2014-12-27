using System;
using System.Collections.Generic;

namespace InjectionMap
{
    public interface IComponentProvider
    {
        /// <summary>
        /// Gets all mappings of the type
        /// </summary>
        /// <typeparam name="T">Key type of the mapping</typeparam>
        /// <returns>A list of all maoppings to the given type</returns>
        IEnumerable<IMappingComponent> Get<T>();

        /// <summary>
        /// Gets all mappings of the type
        /// </summary>
        /// <param name="type">Key type of the mapping</param>
        /// <returns>A list of all mappings to the given type</returns>
        IEnumerable<IMappingComponent> Get(Type type);

        /// <summary>
        /// Gets all mappings
        /// </summary>
        /// <returns>A list of all mappings</returns>
        IEnumerable<IMappingComponent> GetAll();
    }
}
