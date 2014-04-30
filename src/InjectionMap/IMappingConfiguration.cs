
namespace InjectionMap
{
    public interface IMappingConfiguration
    {
        /// <summary>
        /// Indicates if the resolved value should be cached and reused for all further uses
        /// </summary>
        bool CacheValue { get; }

        /// <summary>
        /// Indicates if the value of the mapping will be resolved when mapping or when resolving
        /// </summary>
        bool ResolveInstanceOnMapping { get; }

        /// <summary>
        /// Indicates that only a single mapping with this key can exist and that this mapping cannot be overriden by other mappings
        /// </summary>
        bool AsSingleton { get; }
    }
}
