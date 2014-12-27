using System;

namespace InjectionMap.Extensions
{
    internal static class EnsureExtensions
    {
        public static void EnsureArgumentNotNull(this object argument, string name)
        {
            if (argument == null) 
                throw new ArgumentNullException(name, "Cannot be null");
        }

        public static void EnsureArgumentNotNullOrEmpty(this string argument, string name)
        {
            if (String.IsNullOrEmpty(argument)) 
                throw new ArgumentException("Cannot be null or empty", name);
        }

        public static void EnsureMappingTypeMatches(this Type keyType, Type mappedType)
        {
            if (!keyType.IsAssignableFrom(mappedType))
                throw new MappingMismatchException(mappedType, keyType);
        }

        public static void EnsureTypeIsImplemented(this Type type, Type basetype)
        {
            EnsureMappingTypeMatches(basetype, type);
        }

        public static void EnsureTypeCanBeInstantiated(this Type type)
        {
            if (type.IsInterface)
                throw new TypeCompositionException(type, string.Format("The Type {0} cannot be instantiated because {0} is a interface", type.Name));

            if (type.IsAbstract)
                throw new TypeCompositionException(type, string.Format("The Type {0} cannot be instantiated because {0} is a abstract class", type.Name));
        }

        public static void EnsureTypeCanBeDefaultInstantiated(this Type type)
        {
            if (type.IsInterface)
                throw new TypeCompositionException(type, string.Format("The Type {0} cannot be instantiated because {0} is a interface", type.Name));

            if (type.IsAbstract)
                throw new TypeCompositionException(type, string.Format("The Type {0} cannot be instantiated because {0} is a abstract class", type.Name));

            if (type.GetConstructor(new Type[0]) == null)
                throw new TypeCompositionException(type, string.Format("Could not find a default constructor for the Type {0}", type.Name));
        }
    }
}
