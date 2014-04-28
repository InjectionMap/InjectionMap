using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace InjectionMap.Composition
{
    internal class ArgumentContainer
    {
        public ArgumentContainer(ConstructorInfo ctor)
        {
            ConstructorInfo = ctor;
        }

        public ConstructorInfo ConstructorInfo { get; private set; }

        IList<IArgument> _parameters;
        public IList<IArgument> Parameters
        {
            get
            {
                if (_parameters == null)
                    _parameters = new List<IArgument>();
                return _parameters;
            }
        }

        public bool PushArgument(IArgument argument)
        {
            if (Parameters.Any(a => a.Name == argument.Name))
                return false;

            Parameters.Add(argument);

            return true;
        }
        
        public bool IsArgumentInUse(object value)
        {
            return Parameters.Any(a => a.Value == value);
        }
    }
}
