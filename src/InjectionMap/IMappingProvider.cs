using System;

namespace InjectionMap
{
    public interface IMappingProvider
    {
        /// <summary>
        /// Creates a Mapping to TKey
        /// </summary>
        /// <typeparam name="TKey">The type to map</typeparam>
        /// <returns>The expression for the mapping</returns>
        IMappingExpression<TKey> Map<TKey>();

        /// <summary>
        /// Creates a mapping to TKey with TMap
        /// </summary>
        /// <typeparam name="TKey">The key type to map</typeparam>
        /// <typeparam name="TMap">The instance type to map</typeparam>
        /// <returns>The expression for the mapping</returns>
        IBindingExpression<TMap> Map<TKey, TMap>() where TMap : TKey;

        /// <summary>
        /// Removes all mappings of type T
        /// </summary>
        /// <typeparam name="T">The type of mappings to remove</typeparam>
        void Clean<T>();
    }
}
