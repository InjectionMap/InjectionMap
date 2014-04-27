using System;

namespace InjectionMap
{
    public class ArgumentNotDefinedException : Exception
    {
        public ArgumentNotDefinedException(Type argumentType, Type mappableType) :
            this(argumentType, mappableType, string.Format("A error occured while trying to resolve the mappedd type {0}. Expected Argument of type {1} could not be resolved or is not mapped for injection. Provide the Argument {1} as mapping or as Argument for constructing {0}", mappableType.Name, argumentType.Name))
        {
            ArgumentType = argumentType;
            MappableType = mappableType;
        }

        public ArgumentNotDefinedException(Type argumentType, Type mappableType, string message) :
            base(message)
        {
            ArgumentType = argumentType;
            MappableType = mappableType;
        }


        public Type ArgumentType { get; private set; }

        public Type MappableType { get; private set; }
    }
}
