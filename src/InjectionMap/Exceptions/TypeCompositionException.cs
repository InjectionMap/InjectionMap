using System;

namespace InjectionMap
{
    public class TypeCompositionException : Exception
    {
        public TypeCompositionException(Type type)
            : this(type, null)
        {
            CompositionType = type;
        }

        public TypeCompositionException(Type type, string message)
            : base(string.Format("Instance can not be composed of Type {0}\n{1}", type.Name, message))
        {
            CompositionType = type;
        }

        public Type CompositionType { get; private set; }
    }
}
