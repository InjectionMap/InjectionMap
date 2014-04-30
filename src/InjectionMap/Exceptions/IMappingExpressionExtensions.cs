using InjectionMap.Expressions;
using InjectionMap.Internals;
using InjectionMap.Mapping;

namespace InjectionMap.Exceptions
{
    public static class IMappingExpressionExtensions
    {
        /// <summary>
        /// Creates a mapping to the type TMap that gets composed when resolving
        /// </summary>
        /// <typeparam name="TKey">The mapped type</typeparam>
        /// <returns>A IBindingExpression of TMap</returns>
        public static IBindingExpression<TKey> MapTo<TKey>(this object obj)
        {
            return MapTo<TKey>(obj, MappingContainerManager.MappingContainer);
        }

        /// <summary>
        /// Creates a mapping to the type TMap that gets composed when resolving
        /// </summary>
        /// <typeparam name="TKey">The mapped type</typeparam>
        /// <param name="container">The IComponentCollection to map to</param>
        /// <returns>A IBindingExpression of TMap</returns>
        public static IBindingExpression<TKey> MapTo<TKey>(this object obj, IComponentCollection container)
        {
            Ensure.MappingTypesMatch(typeof(TKey), obj.GetType());

            return MappingContainer.MapInternal<TKey>(container).For(() => (TKey)obj);
        }
    }
}
