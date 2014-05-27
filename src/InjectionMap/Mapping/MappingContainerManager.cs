using System;
using System.Linq;

namespace InjectionMap.Mapping
{
    internal static class MappingContainerManager
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
        
        #endregion
    }
}
