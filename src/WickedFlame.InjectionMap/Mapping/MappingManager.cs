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

        #endregion
    }
}
