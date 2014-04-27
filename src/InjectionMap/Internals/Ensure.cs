using System;
using InjectionMap.Exceptions;

namespace InjectionMap.Internals
{
    internal static class Ensure
    {
        public static void ArgumentNotNull(object argument, string name)
        {
            if (argument == null) 
                throw new ArgumentNullException(name, "Cannot be null");
        }

        public static void ArgumentNotNullOrEmpty(string argument, string name)
        {
            if (String.IsNullOrEmpty(argument)) 
                throw new ArgumentException("Cannot be null or empty", name);
        }

        public static void MappingTypesMatch(Type keyType, Type mappedType)
        {
            if (!keyType.IsAssignableFrom(mappedType))
                throw new MappingMismatchException(mappedType, keyType);
        }

        public static void CanBeInstantiated(Type type)
        {
            if (type.IsAbstract || type.IsInterface)
                throw new TypeCompositionException(type);
        }

        public static void CanBeDefaultInstantiated(Type type)
        {
            if (type.IsAbstract || type.IsInterface)
                throw new TypeCompositionException(type);

            if (type.GetConstructor(Type.EmptyTypes) == null)
                throw new TypeCompositionException(type);
        }
    }
}
