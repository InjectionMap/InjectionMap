
namespace WickedFlame.InjectionMap
{
    public interface IOptionExpression
    {
        IMappingOption MappingOption { get; }

        IOptionExpression WithOptions(InjectionFlags option);
    }
}
