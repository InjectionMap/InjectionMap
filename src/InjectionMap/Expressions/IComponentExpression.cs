
namespace InjectionMap.Expressions
{
    public interface IComponentExpression
    {
        IComponentCollection Context { get; }

        IMappingComponent Component { get; }
    }
}
