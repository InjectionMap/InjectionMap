using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WickedFlame.InjectionMap.Composition
{
    internal class CompositionContainer : IDisposable
    {
        public T ComposePart<T>()
        {
            // 1. Search for InjectionConstructor
            // 2. Create Instances of all parameters in constructor
            // 3. pass objects to constructor
            // 4. if no InjectionConstructor use default constructor

            return (T)ComposePart(typeof(T));
        }

        public T ComposePart<T>(params object[] parameters)
        {
            // 1. Search for InjectionConstructor
            // 2. Create Instances of all parameters in constructor
            // 3. pass objects to constructor
            // 4. if no InjectionConstructor use default constructor

            return (T)ComposePart(typeof(T), parameters);
        }

        public object ComposePart(Type type)
        {
            var ctors = type.GetConstructors().Where(c => c.GetCustomAttributes(typeof(InjectionConstructorAttribute), false).Any());

            // check if there is a constructor marked as InjectionConstructor
            var ctor = GetComposeableConstructor(ctors);
            if (ctor != null)
            {
                return ctor.ConstructorInfo.Invoke(ctor.Parameters.ToArray());
            }

            // default constructor
            return Activator.CreateInstance(type);
        }

        public object ComposePart(Type type, params object[] parameters)
        {
            return Activator.CreateInstance(type, parameters);
        }

        private ConstructorInformation GetComposeableConstructor(IEnumerable<ConstructorInfo> ctors)
        {
            if (ctors == null || !ctors.Any())
                return null;

            var resolved = new List<ConstructorInformation>();
            foreach (var ctor in ctors)
            {
                bool ok = true;
                var info = new ConstructorInformation(ctor);

                foreach (var param in ctor.GetParameters())
                {
                    var composed = InjectionResolver.Resolve(param.ParameterType);
                    //var composed = Compose(param.ParameterType);
                    if (composed != null)
                    {
                        info.Parameters.Add(composed);
                    }
                    else
                    {
                        ok = false;
                        break;
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
