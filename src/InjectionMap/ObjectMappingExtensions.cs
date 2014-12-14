using InjectionMap.Extensions;
using InjectionMap.Internal;

namespace InjectionMap
{
    public static class ObjectMappingExtensions
    {
        /// <summary>
        /// Creates a mapping to the type TMap that gets composed when resolving
        /// </summary>
        /// <typeparam name="TKey">The mapped type</typeparam>
        /// <param name="obj">The object to extend</param>
        /// <returns>A IBindingExpression of TMap</returns>
        public static IBindingExpression<TKey> MapTo<TKey>(this object obj)
        {
            return MapTo<TKey>(obj, MappingContextManager.MappingContext);
        }

        /// <summary>
        /// Creates a mapping to the type TMap that gets composed when resolving
        /// </summary>
        /// <typeparam name="TKey">The mapped type</typeparam>
        /// <param name="obj">The object to extend</param>
        /// <param name="context">The IComponentCollection to map to</param>
        /// <returns>A IBindingExpression of TMap</returns>
        public static IBindingExpression<TKey> MapTo<TKey>(this object obj, IComponentCollection context)
        {
            typeof(TKey).EnsureMappingTypeMatches(obj.GetType());

            return MappingContext.MapInternal<TKey>(context).For(() => (TKey)obj);
        }
    }
}
