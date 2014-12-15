using System;

namespace InjectionMap
{
    /// <summary>
    /// Represents an expression that containes the map after it was bound
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBoundExpression<T>
    {
        /// <summary>
        /// Gets the configuration for the mapping
        /// </summary>
        IMappingConfiguration MappingConfiguration { get; }

        /// <summary>
        /// Adds a delegate that gets executed when the map is resoved
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        IBoundExpression<T> OnResolved(Action<T> callback);
    }
}
