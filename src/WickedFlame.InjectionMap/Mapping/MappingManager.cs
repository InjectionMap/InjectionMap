using System;
using System.Collections.Generic;
using System.Linq;
using WickedFlame.InjectionMap.Composition;

namespace WickedFlame.InjectionMap.Mapping
{
    internal static class MappingManager
    {
        #region Collection and Helpers

        static MappingContainer _mappingContainer;
        internal static MappingContainer MappingContainer
        {
            get
            {
                if (_mappingContainer == null)
                    _mappingContainer = new MappingContainer();

                return _mappingContainer;
            }
        }

        public static void ReplaceMapping(IMappingComponent original, IMappingComponent substitute)
        {
            if (MappingContainer.Components.Contains(original))
                MappingContainer.Components.Remove(original);

            MappingContainer.Components.Add(substitute);
        }

        internal static void Clean<T>()
        {
            var items = MappingContainer.Components.Where(c => c.KeyType == typeof(T)).ToList();
            foreach (var item in items)
            {
                MappingContainer.Remove(item);
            }
        }

        #endregion

        #region Get Methods

        internal static T Get<T>()
        {
            return MappingContainer.Components.Where(c => c.KeyType == typeof(T)).Select(c => CompositionService.Compose<T>(c)).FirstOrDefault();
        }

        internal static object Get(Type type)
        {
            return MappingContainer.Components.Where(c => c.KeyType == type).Select(c => CompositionService.Compose(c)).FirstOrDefault();
        }

        internal static IMappingComponent GetComponent<T>()
        {
            return MappingContainer.Components.Where(c => c.KeyType == typeof(T)).FirstOrDefault();
        }

        internal static IEnumerable<IMappingComponent> GetAllComponents<T>()
        {
            return MappingContainer.Components.Where(c => c.KeyType == typeof(T));
        }

        internal static IEnumerable<T> GetAll<T>()
        {
            return MappingContainer.Components.Where(c => c.KeyType == typeof(T)).Select(c => CompositionService.Compose<T>(c));
        }

        #endregion
    }
}
