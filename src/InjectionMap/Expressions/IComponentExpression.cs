
namespace InjectionMap
{
    public interface IComponentExpression
    {
        IComponentCollection Context { get; }

        IMappingComponent Component { get; }
    }
}
