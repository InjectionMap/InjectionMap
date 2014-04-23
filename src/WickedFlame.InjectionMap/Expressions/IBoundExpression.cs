
namespace WickedFlame.InjectionMap.Expressions
{
    public interface IBoundExpression : IComponentExpression
    {

        IMappingOption MappingOption { get; }

        //IOptionExpression WithOptions(InjectionFlags option);
    }
}
