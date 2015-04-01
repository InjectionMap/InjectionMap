using System;
using System.Collections.Generic;
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


        static Dictionary<string, ComponentContainer> _components;
        internal static Dictionary<string, ComponentContainer> Components
        {
            get
            {
                if (_components == null)
                    _components = new Dictionary<string, ComponentContainer>();
                return _components;
            }
        }

        public static ComponentContainer GetComponents(string context)
        {
            lock (Components)
            {
                if (!Components.Keys.Contains(context))
                {
                    Components.Add(context, new ComponentContainer
                    {
                        Components = new List<IMappingComponent>(),
                        Context = context
                    });
                }

                return Components[context];
            }
        }
    }
}
