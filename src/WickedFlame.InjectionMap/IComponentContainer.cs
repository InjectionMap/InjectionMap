using System.Collections.Generic;

namespace WickedFlame.InjectionMap
{
    public interface IComponentContainer
    {
        IList<IMappingComponent> Components { get; }

        void AddOrReplace(IMappingComponent component);

        void Add(IMappingComponent component);

        void ReplaceAll(IMappingComponent component);

        void Remove(IMappingComponent component);
    }
}
