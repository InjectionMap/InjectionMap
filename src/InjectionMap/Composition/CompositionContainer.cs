using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using InjectionMap.Internals;
using InjectionMap.Mapping;

namespace InjectionMap.Composition
{
    internal class CompositionContainer : IDisposable
    {
        /// <summary>
        /// Composes an instance of T from the component
        /// </summary>
        /// <typeparam name="T">The type to create from the component</typeparam>
        /// <param name="component">The mapping</param>
        /// <returns>The composed object</returns>
        public T ComposePart<T>(IMappingComponent component)
        {
            return (T)ComposePart(component);
        }

        /// <summary>
        /// Composes an instance from the component
        /// </summary>
        /// <param name="component">The mapping</param>
        /// <returns>The composed object</returns>
        public object ComposePart(IMappingComponent component)
        {
            // check if there is a constructor marked as InjectionConstructor
            var ctor = GetComposeableConstructor(component);
            if (ctor != null)
            {
                return ctor.ConstructorInfo.Invoke(ctor.Parameters.Select(p => p.Value).ToArray());
            }

            Ensure.CanBeDefaultInstantiated(component.ValueType);

            // default constructor
            return Activator.CreateInstance(component.ValueType);
        }

        /// <summary>
        /// Composes an instance from the component
        /// </summary>
        /// <param name="component">The mapping</param>
        /// <param name="parameters">Parameters to resolve</param>
        /// <returns>The composed object</returns>
        public object ComposePart(IMappingComponent component, params object[] parameters)
        {
            return Activator.CreateInstance(component.ValueType, parameters);
        }

        /// <summary>
        /// Gets a constructor where all parameters can be composed 
        /// </summary>
        /// <param name="component">The component to compose</param>
        /// <returns>An ArgumentContainer containing all arguments</returns>
        private ArgumentContainer GetComposeableConstructor(IMappingComponent component)
        {
            var ctors = component.ValueType.GetConstructors();
            
            // try get InjectionConstructors
            var tmpctors = ctors.Where(c => c.GetCustomAttributes(typeof(InjectionConstructorAttribute), false).Any());
            if (tmpctors != null && tmpctors.Any())
            {
                var container = GetArgumentContainer(component, tmpctors);
                if (container != null)
                    return container;
            }

            // try resolve by argument amount
            tmpctors = ctors.Where(c => c.GetParameters().Count() == component.Arguments.Count);
            if (tmpctors != null && tmpctors.Any())
            {
                var container = GetArgumentContainer(component, tmpctors);
                if (container != null)
                    return container;
            }

            // try to resolve default constructor
            tmpctors = ctors.Where(c => !c.GetParameters().Any());
            if (tmpctors != null && tmpctors.Any())
            {
                // no arguments so use the default constructor
                return null;
            }

            // try resolve any constructor
            if (ctors != null && ctors.OrderByDescending(c => c.GetParameters().Count()).Any())
            {
                var container = GetArgumentContainer(component, ctors);
                if (container != null)
                    return container;
            }
            
            return null;
        }

        private ArgumentContainer GetArgumentContainer(IMappingComponent component, IEnumerable<ConstructorInfo> ctors)
        {
            // loops trough all constructors and tries to resolve
            foreach (var ctor in ctors)
            {
                var container = CreateArgumentContainer(component, ctor);
                if (container != null)
                    return container;
            }

            return null;
        }

        private ArgumentContainer CreateArgumentContainer(IMappingComponent component, ConstructorInfo ctor)
        {
            // tries to resolve the constructor
            bool ok = true;
            var info = new ArgumentContainer(ctor);

            using (var factory = new ArgumentFactory(component, info))
            {
                foreach (var param in ctor.GetParameters())
                {
                    var composed = factory.CreateArgument(param);

                    if (composed != null)
                    {
                        if (!info.PushArgument(composed))
                            throw new ArgumentNotDefinedException(param.ParameterType, component.KeyType);
                    }
                    else
                    {
                        ok = false;
                        break;
                    }
                }
            }

            // get first constructor that is composeable
            if (ok)
                return info;


            return null;
        }

        public void Dispose()
        {
        }
    }
}
