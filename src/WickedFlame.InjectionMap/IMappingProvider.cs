using System;
using WickedFlame.InjectionMap.Expressions;

namespace WickedFlame.InjectionMap
{
    public interface IMappingProvider
    {
        /// <summary>
        /// Creates a Mapping to TSvc
        /// </summary>
        /// <typeparam name="TSvc">The type to map</typeparam>
        /// <returns>The expression for the mapping</returns>
        IMappingExpression<TSvc> Map<TSvc>();

        /// <summary>
        /// Creates a mapping to TSvc with TImpl
        /// </summary>
        /// <typeparam name="TSvc">The key type to map</typeparam>
        /// <typeparam name="TImpl">The instance type to map</typeparam>
        /// <returns>The expression for the mapping</returns>
        IBindingExpression<TImpl> Map<TSvc, TImpl>() where TImpl : TSvc;

        /// <summary>
        /// Removes all mappings of type T
        /// </summary>
        /// <typeparam name="T">The type of mappings to remove</typeparam>
        void Clean<T>();
    }
}
