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
        /// <param name="component"></param>
        /// <returns></returns>
        public T ComposePart<T>(IMappingComponent component)
        {
            return (T)ComposePart(component);
        }

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
            var ctors = component.ValueType.GetConstructors().Where(c => c.GetCustomAttributes(typeof(InjectionConstructorAttribute), false).Any());

            if (ctors == null || !ctors.Any())
            {
                // if no InjectionConstructor, test if any arguments
                ctors = component.ValueType.GetConstructors();//.Where(c => c.GetParameters().Count() == component.Arguments.Count);
            }

            if (ctors == null || !ctors.Any())
                return null;

            //var resolved = new List<ArgumentContainer>();
            foreach (var ctor in ctors)
            {
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
                
                // add the info anyway to try to resolve
                //resolved.Add(info);
            }

            return null;// resolved.FirstOrDefault();
        }

        public void Dispose()
        {
        }
    }
}
