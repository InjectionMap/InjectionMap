using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WickedFlame.InjectionMap.Internals;
using WickedFlame.InjectionMap.Mapping;

namespace WickedFlame.InjectionMap.Composition
{
    internal class CompositionContainer : IDisposable
    {
        public T ComposePart<T>(IMappingComponent component)
        {
            // 1. Search for InjectionConstructor
            // 2. Create Instances of all parameters in constructor
            // 3. pass objects to constructor
            // 4. if no InjectionConstructor use default constructor

            return (T)ComposePart(component);
        }

        //public T ComposePart<T>(params object[] parameters)
        //{
        //    // 1. Search for InjectionConstructor
        //    // 2. Create Instances of all parameters in constructor
        //    // 3. pass objects to constructor
        //    // 4. if no InjectionConstructor use default constructor

        //    return (T)ComposePart(typeof(T), parameters);
        //}

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

        private ArgumentContainer GetComposeableConstructor(IMappingComponent component)
        {
            var ctors = component.ValueType.GetConstructors().Where(c => c.GetCustomAttributes(typeof(InjectionConstructorAttribute), false).Any());

            if (ctors == null || !ctors.Any())
            {
                // if no InjectionConstructor, test if any arguments
                ctors = component.ValueType.GetConstructors().Where(c => c.GetParameters().Count() == component.Arguments.Count);
            }

            if (ctors == null || !ctors.Any())
                return null;

            var resolved = new List<ArgumentContainer>();
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

                if (ok)
                    resolved.Add(info);
            }

            return resolved.FirstOrDefault();
        }

        public void Dispose()
        {
        }
    }
}
