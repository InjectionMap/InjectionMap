
namespace WickedFlame.InjectionMap
{
    public interface IOptionExpression
    {
        IComponentContainer ComponentContainer { get; }

        IMappingComponent Component { get; }

        IMappingOption MappingOption { get; }

        //IOptionExpression WithOptions(InjectionFlags option);
    }
}
