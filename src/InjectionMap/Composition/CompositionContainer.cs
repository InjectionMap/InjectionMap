using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using InjectionMap.Extensions;
using InjectionMap.Tracing;

namespace InjectionMap.Composition
{
    internal class CompositionContainer : IDisposable
    {
        internal Lazy<ILoggerFactory> LoggerFactory { get; private set; }

        internal ILogger Logger
        {
            get
            {
                return LoggerFactory.Value.CreateLogger();
            }
        }

        public CompositionContainer()
        {
            LoggerFactory = new Lazy<ILoggerFactory>(() => new LoggerFactory());
        }

        #region Compose Implementation

        public T Compose<T>()
        {
            return (T)Compose(typeof(T));
        }

        public object Compose(Type type)
        {
            Logger.Write(string.Format("InjectionMap - Compose Type {0}", type), "CompositionContainer", "Resolver");

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
            object instance = null;

            // check if there is a constructor marked as InjectionConstructor
            var ctor = GetComposeableConstructor(component);
            if (ctor != null)
            {
                instance = ctor.ConstructorInfo.Invoke(ctor.Parameters.Select(p => p.Value).ToArray());

                Logger.Write(string.Format("InjectionMap - Create Instance of Type {0} through composed constructor", component.KeyType), "CompositionContainer", "Resolver");
            }

            if (instance == null)
            {
                component.ValueType.EnsureTypeCanBeDefaultInstantiated();

                // default constructor
                instance = Activator.CreateInstance(component.ValueType);

                Logger.Write(string.Format("InjectionMap - Create Instance of Type {0} through default constructor", component.KeyType), "CompositionContainer", "Resolver");
            }

            InjectProperties(component, instance);

            return instance;
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
                // Check if a constructor was defined in the mapping
                if (component.ConstructorDefinition != null)
                {
                    //TODO: is it wise to present the constructorinfo to public in the constructordefinition?
                    //TODO: try hide this from public
                    var container = CreateArgumentContainer(component, component.ConstructorDefinition.ConstructorInfo);
                    if (container != null)
                        return container;
                }
            }

            // get all constructors from the value type
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
        /// <param name="type">The component to compose</param>
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

        #region Properties

        private void InjectProperties(IMappingComponent component, object instance)
        {
            if (!component.Properies.Any())
                return;

            using (var resolver = new InjectionMap.Internal.ComponentResolver())
            {
                foreach (var property in component.Properies)
                {
                    var value = resolver.Get(property.KeyType);
                    property.Setter(instance, value);

                    if (value == null)
                        Logger.Write(string.Format("InjectionMap - Could not resolve value for Property {0}", property.Property.Name), "CompositionContainer", "Resolver");
                }
            }
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
