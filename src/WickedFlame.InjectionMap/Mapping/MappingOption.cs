
namespace WickedFlame.InjectionMap.Mapping
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
        /// Gets a value indicating if the Mapping replaces all other mappings with the same key type
        /// </summary>
        public bool AsSingleton { get; internal set; }
    }
}
