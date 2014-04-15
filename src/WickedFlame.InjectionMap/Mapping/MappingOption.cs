
namespace WickedFlame.InjectionMap.Mapping
{
    internal class MappingOption : IMappingOption
    {
        public MappingOption()
        {
            KeepInstance = false;
            ResolveInstanceOnMapping = false;
            WithoutOverwrite = true;
        }

        public bool KeepInstance { get; internal set; }

        public bool ResolveInstanceOnMapping { get; internal set; }

        public bool WithoutOverwrite { get; internal set; }
    }
}
