
namespace WickedFlame.InjectionMap
{
    public interface IComponentExpression
    {
        IComponentCollection Container { get; }

        IMappingComponent Component { get; }
    }
}
