using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InjectionMap.Extensions
{
    internal static class ReflectionExtensions
    {
        public static ConstructorCollection GetConststructorCollection(this Type type)
        {
            var collection = new ConstructorCollection();
            var constructors = type.GetConstructors();

            foreach (var constructor in constructors)
            {
                var arguments = constructor.GetParameters();

                var definition = new ConstructorDefinition
                {
                    ConstructorInfo = constructor
                };

                foreach (var argument in arguments)
                {
                    definition.Add(new Argument
                    {
                        Name = argument.Name,
                        Type = argument.ParameterType

                    });
                }

                collection.Add(definition);
            }

            return collection;
        }
    }
}
