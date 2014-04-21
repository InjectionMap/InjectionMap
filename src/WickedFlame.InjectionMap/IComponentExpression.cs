
namespace WickedFlame.InjectionMap
{
    public interface IComponentExpression
    {
        IComponentContainer Container { get; }

        IMappingComponent Component { get; }
    }
}
