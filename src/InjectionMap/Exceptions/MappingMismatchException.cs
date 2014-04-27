using System;

namespace InjectionMap.Exceptions
{
    public class MappingMismatchException : Exception
    {
        public MappingMismatchException(Type keyType, Type mappableType) :
            this(keyType, mappableType, string.Format("Type {0} has to implementen or inherit from {1} to be assignable", mappableType.Name, keyType.Name))
        {
            KeyType = keyType;
            MappableType = mappableType;
        }

        public MappingMismatchException(Type keyType, Type mappableType, string message) :
            base(message)
        {
            KeyType = keyType;
            MappableType = mappableType;
        }


        public Type KeyType { get; private set; }

        public Type MappableType { get; private set; }
    }
}
