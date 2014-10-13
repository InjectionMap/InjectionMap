using System;
using System.Linq;

namespace InjectionMap.Internal
{
    internal static class MappingContextManager
    {
        #region Collection and Helpers

        static MappingContext _context;
        internal static MappingContext MappingContext
        {
            get
            {
                if (_context == null)
                    _context = new MappingContext();

                return _context;
            }
        }
        
        #endregion
    }
}
