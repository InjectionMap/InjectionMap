
namespace WickedFlame.InjectionMap
{
    public interface IMappingOption
    {
        bool KeepInstance { get; }

        bool ResolveInstanceOnMapping { get; }

        bool WithoutOverwrite { get; }
    }
}
