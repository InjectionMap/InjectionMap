
using System;
using System.Collections.Generic;

namespace InjectionMap
{
    /// <summary>
    /// Represents a collection of IMappingComponent
    /// </summary>
    public interface IComponentCollection
    {
        /// <summary>
        /// Adds a component or replace all existing mappings of components with the same id
        /// </summary>
        /// <param name="component"></param>
        void AddOrReplace(IMappingComponent component);

        /// <summary>
        /// Adds a new component
        /// </summary>
        /// <param name="component"></param>
        void Add(IMappingComponent component);

        /// <summary>
        /// Replaces all existing components with this version
        /// </summary>
        /// <param name="component"></param>
        void ReplaceAll(IMappingComponent component);

        /// <summary>
        /// remove the given component
        /// </summary>
        /// <param name="component"></param>
        void Remove(IMappingComponent component);

        /// <summary>
        /// Gets all mappings of the type
        /// </summary>
        /// <typeparam name="T">Key type of the mapping</typeparam>
        /// <returns>A list of all maoppings to the given type</returns>
        IEnumerable<IMappingComponent> Get<T>();

        /// <summary>
        /// Gets all mappings of the type
        /// </summary>
        /// <typeparam name="T">Key type of the mapping</typeparam>
        /// <param name="predicate">The predicate that checks the expression</param>
        /// <returns>A list of all maoppings to the given type</returns>
        IEnumerable<IMappingComponent> Get<T>(Func<IMappingComponent, bool> predicate);
    }
}
