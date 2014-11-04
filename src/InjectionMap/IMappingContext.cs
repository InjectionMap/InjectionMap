
namespace InjectionMap
{
    /// <summary>
    /// Represents the context that is mapped to. A mapping only exists in a distinct context.
    /// </summary>
    public interface IMappingContext : IMappingProvider, IComponentProvider
    {
    }
}
