using System;

namespace InjectionMap
{
    /// <summary>
    /// Represents a exception that gets thrown when a map that has to be used further can't be resolved
    /// </summary>
    public class ResolverException : Exception
    {
        public ResolverException(Type type)
            : base(string.Format("Type {0} can not be resolved", type))
        {
            KeyType = type;
        }

        public ResolverException(Type type, string message)
            : base(message)
        {
            KeyType = type;
        }

        public Type KeyType { get; private set; }
    }
}
