
namespace WickedFlame.InjectionMap
{
    public interface IBoundExpression
    {
        IComponentContainer ComponentContainer { get; }

        IMappingComponent Component { get; }

        IMappingOption MappingOption { get; }

        //IOptionExpression WithOptions(InjectionFlags option);
    }
}
