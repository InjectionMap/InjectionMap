
namespace WickedFlame.InjectionMap
{
    public interface IComponentCollection
    {
        void AddOrReplace(IMappingComponent component);

        void Add(IMappingComponent component);

        void ReplaceAll(IMappingComponent component);

        void Remove(IMappingComponent component);
    }
}
