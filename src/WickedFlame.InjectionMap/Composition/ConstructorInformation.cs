using System.Collections.Generic;
using System.Reflection;

namespace WickedFlame.InjectionMap.Composition
{
    class ConstructorInformation
    {
        public ConstructorInformation(ConstructorInfo ctor)
        {
            ConstructorInfo = ctor;
        }

        public ConstructorInfo ConstructorInfo { get; private set; }

        IList<object> _parameters;
        public IList<object> Parameters
        {
            get
            {
                if (_parameters == null)
                    _parameters = new List<object>();
                return _parameters;
            }
        }
    }
}
