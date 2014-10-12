using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using InjectionMap.Extensions;

namespace InjectionMap.Composition
{
    internal class CompositionContainer : IDisposable
    {
        #region Compose Implementation

        public T Compose<T>()
        {
            return (T)Compose(typeof(T));
        }

        public object Compose(Type type)
        {
            // check if there is a constructor marked as InjectionConstructor
            var ctor = GetComposeableConstructor(type);
            if (ctor != null)
            {
                return ctor.ConstructorInfo.Invoke(ctor.Parameters.Select(p => p.Value).ToArray());
            }

            type.EnsureTypeCanBeDefaultInstantiated();

            // default constructor
            return Activator.CreateInstance(type);
        }

        /// <summary>
        /// Composes an instance of T from the component
        /// </summary>
        /// <typeparam name="T">The type to create from the component</typeparam>
        /// <param name="component">The mapping</param>
        /// <returns>The composed object</returns>
        public T Compose<T>(IMappingComponent component)
        {
            return (T)Compose(component);
        }

        /// <summary>
        /// Composes an instance from the component
        /// </summary>
        /// <param name="component">The mapping</param>
        /// <returns>The composed object</returns>
        public object Compose(IMappingComponent component)
        {
            // check if there is a constructor marked as InjectionConstructor
            var ctor = GetComposeableConstructor(component);
            if (ctor != null)
            {
                return ctor.ConstructorInfo.Invoke(ctor.Parameters.Select(p => p.Value).ToArray());
            }

            component.ValueType.EnsureTypeCanBeDefaultInstantiated();

            // default constructor
            return Activator.CreateInstance(component.ValueType);
        }

        /// <summary>
        /// Composes an instance from the component
        /// </summary>
        /// <param name="component">The mapping</param>
        /// <param name="parameters">Parameters to resolve</param>
        /// <returns>The composed object</returns>
        public object Compose(IMappingComponent component, params object[] parameters)
        {
            return Activator.CreateInstance(component.ValueType, parameters);
        }

        #endregion

        #region Composeable Constructors

        /// <summary>
        /// Gets a constructor where all parameters can be composed 
        /// </summary>
        /// <param name="component">The component to compose</param>
        /// <returns>An ArgumentContainer containing all arguments</returns>
        private ArgumentContainer GetComposeableConstructor(IMappingComponent component)
        {
            if (component.ConstructorDefinition != null)
            {
                // TODO: remove cast! Try to define the selected constructor without using ConstructorInfo!
                var definition = component.ConstructorDefinition as ConstructorDefinition;
                if (definition != null)
                {
                    var container = CreateArgumentContainer(component, definition.ConstructorInfo);
                    if (container != null)
                        return container;
                }
            }

            var ctors = component.ValueType.GetConstructors();
            
            // try get InjectionConstructors
            var tmpctors = ctors.Where(c => c.GetCustomAttributes(typeof(InjectionConstructorAttribute), false).Any());
            if (tmpctors.Any())
            {
                var container = GetArgumentContainer(component, tmpctors);
                if (container != null)
                    return container;
            }

            // try resolve by amount of arguments
            tmpctors = ctors.Where(c => c.GetParameters().Count() == component.Arguments.Count);
            if (tmpctors.Any())
            {
                var container = GetArgumentContainer(component, tmpctors);
                if (container != null)
                    return container;
            }

            // try resolve any constructor
            if (ctors.OrderByDescending(c => c.GetParameters().Count()).Any())
            {
                var container = GetArgumentContainer(component, ctors);
                if (container != null)
                    return container;
            }

            // try to resolve default constructor
            tmpctors = ctors.Where(c => !c.GetParameters().Any());
            if (tmpctors.Any())
            {
                // no arguments so use the default constructor
                return null;
            }
            
            return null;
        }

        /// <summary>
        /// Gets a constructor where all parameters can be composed 
        /// </summary>
        /// <param name="component">The component to compose</param>
        /// <returns>An ArgumentContainer containing all arguments</returns>
        private ArgumentContainer GetComposeableConstructor(Type type)
        {
            var ctors = type.GetConstructors();

            // try get InjectionConstructors
            var tmpctors = ctors.Where(c => c.GetCustomAttributes(typeof(InjectionConstructorAttribute), false).Any());
            if (tmpctors.Any())
            {
                var container = GetArgumentContainer(tmpctors);
                if (container != null)
                    return container;
            }

            //// try resolve by amount of arguments
            //tmpctors = ctors.Where(c => c.GetParameters().Count() == component.Arguments.Count);
            //if (tmpctors.Any())
            //{
            //    var container = GetArgumentContainer(component, tmpctors);
            //    if (container != null)
            //        return container;
            //}

            // try resolve any constructor
            if (ctors.OrderByDescending(c => c.GetParameters().Count()).Any())
            {
                var container = GetArgumentContainer(ctors);
                if (container != null)
                    return container;
            }

            // try to resolve default constructor
            tmpctors = ctors.Where(c => !c.GetParameters().Any());
            if (tmpctors.Any())
            {
                // no arguments so use the default constructor
                return null;
            }

            return null;
        }

        #endregion

        #region Arguments

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

        private ArgumentContainer GetArgumentContainer(IEnumerable<ConstructorInfo> ctors)
        {
            // loops trough all constructors and tries to resolve
            foreach (var ctor in ctors)
            {
                var container = CreateArgumentContainer(ctor);
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

        private ArgumentContainer CreateArgumentContainer(ConstructorInfo ctor)
        {
            // tries to resolve the constructor
            bool ok = true;
            var info = new ArgumentContainer(ctor);

            using (var factory = new ArgumentFactory(info))
            {
                foreach (var param in ctor.GetParameters())
                {
                    var composed = factory.CreateArgument(param);

                    if (composed != null)
                    {
                        if (!info.PushArgument(composed))
                            throw new ArgumentNotDefinedException(param.ParameterType, ctor.DeclaringType);
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

        #endregion

        #region IDisposeable Implementation

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases resources held by the object.
        /// </summary>
        public virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                if (disposing && !IsDisposed)
                {
                    IsDisposed = true;
                    GC.SuppressFinalize(this);
                }
            }
        }

        /// <summary>
        /// Releases resources before the object is reclaimed by garbage collection.
        /// </summary>
        ~CompositionContainer()
        {
            Dispose(false);
        }

        #endregion
    }
}
