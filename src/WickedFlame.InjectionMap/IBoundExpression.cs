
namespace WickedFlame.InjectionMap
{
    public interface IBoundExpression
    {
        IComponentContainer Container { get; }

        IMappingComponent Component { get; }

        IMappingOption MappingOption { get; }

        //IOptionExpression WithOptions(InjectionFlags option);
    }
}
