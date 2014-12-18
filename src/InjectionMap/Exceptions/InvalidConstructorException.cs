using System;

namespace InjectionMap
{
    public class InvalidConstructorException : Exception
    {
        public InvalidConstructorException(string message)
            : base(message)
        {
        }
    }
}
