
namespace InjectionMap.Mapping
{
    internal class MappingOption : IMappingOption
    {
        public MappingOption()
        {
            CacheValue = false;
            ResolveInstanceOnMapping = false;
            AsSingleton = false;
        }

        /// <summary>
        /// Gets a value indicating if the instance should be cached
        /// </summary>
        public bool CacheValue { get; internal set; }

        public bool ResolveInstanceOnMapping { get; internal set; }

        /// <summary>
        /// Indicates that only a single mapping with this key can exist and that this mapping cannot be overriden by other mappings
        /// </summary>
        public bool AsSingleton { get; internal set; }
    }
}
