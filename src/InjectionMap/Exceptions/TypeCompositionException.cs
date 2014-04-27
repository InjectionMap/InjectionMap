using System;

namespace InjectionMap.Exceptions
{
    public class TypeCompositionException : Exception
    {
        public TypeCompositionException(Type type)
            : base(string.Format("Instance can not be composed of Type {0}", type.Name))
        {
            CompositionType = type;
        }

        public Type CompositionType { get; private set; }
    }
}
