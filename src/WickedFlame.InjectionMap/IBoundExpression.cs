
namespace WickedFlame.InjectionMap
{
    public interface IBoundExpression : IComponentExpression
    {

        IMappingOption MappingOption { get; }

        //IOptionExpression WithOptions(InjectionFlags option);
    }
}
