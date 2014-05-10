
namespace InjectionMap.Mapping
{
    internal class MappingConfiguration : IMappingConfiguration
    {
        public MappingConfiguration()
        {
            AsConstant = false;
            ResolveValueOnMapping = false;
            AsSingleton = false;
        }

        /// <summary>
        /// Indicates if the resolved value should be cached and reused for all further uses
        /// </summary>
        public bool AsConstant { get; internal set; }

        /// <summary>
        /// Indicates if the value of the mapping will be resolved when mapping or when resolving
        /// </summary>
        public bool ResolveValueOnMapping { get; internal set; }

        /// <summary>
        /// Indicates that only a single mapping with this key can exist and that this mapping cannot be overriden by other mappings
        /// </summary>
        public bool AsSingleton { get; internal set; }
    }
}
