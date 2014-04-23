
namespace WickedFlame.InjectionMap.Expressions
{
    public interface IComponentExpression
    {
        IComponentCollection Container { get; }

        IMappingComponent Component { get; }
    }
}
