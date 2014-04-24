
namespace WickedFlame.InjectionMap
{
    public interface IMappingOption
    {
        /// <summary>
        /// Gets a value indicating if the instance should be cached
        /// </summary>
        bool CacheValue { get; }

        bool ResolveInstanceOnMapping { get; }

        /// <summary>
        /// Gets a value indicating if the Mapping replaces all other mappings with the same key type
        /// </summary>
        bool AsSingleton { get; }
    }
}
