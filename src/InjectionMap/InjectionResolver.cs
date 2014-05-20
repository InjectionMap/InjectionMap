using System;
using System.Collections.Generic;
using InjectionMap.Internals;
using InjectionMap.Mapping;
using InjectionMap.Expressions;
using InjectionMap.Extensions;

namespace InjectionMap
{
    public class InjectionResolver : IDisposable
    {
        IMappableContainer _container;

        /// <summary>
        /// Creates a InjectionResolver to resolve from the common mappingcontainer
        /// </summary>
        public InjectionResolver()
        {
        }

        /// <summary>
        /// Creates a InjectionResolver with a custom container to resolve from
        /// </summary>
        /// <param name="container">The <see cref="IMappableContainer"/> to resolve the mappings from</param>
        public InjectionResolver(IMappableContainer container)
        {
            Ensure.ArgumentNotNull(container, "container");

            _container = container;
        }

        #region Implementation

        /// <summary>
        /// Resolves the first found occurance of T
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <returns>First found mapping of T</returns>
        public T Resolve<T>()
        {
            using (var resolver = new ComponentResolver(_container))
            {
                return resolver.Get<T>();
            }
        }

        /// <summary>
        /// Resolves the first found occurance of the type
        /// </summary>
        /// <param name="type">the type to resolve</param>
        /// <returns>First found mappinf of type</returns>
        public object Resolve(Type type)
        {
            using (var resolver = new ComponentResolver(_container))
            {
                return resolver.Get(type);
            }
        }

        /// <summary>
        /// Resolves all mappings of type T
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <returns>All mappings of T</returns>
        public IEnumerable<T> ResolveMultiple<T>()
        {
            using (var resolver = new ComponentResolver(_container))
            {
                return resolver.GetAll<T>();
            }
        }

        /// <summary>
        /// Resolves T from the given container
        /// </summary>
        /// <typeparam name="T">The type to resolve</typeparam>
        /// <param name="container">The container to resolve from</param>
        /// <returns>A instance of T</returns>
        public T Resolve<T>(IMappableContainer container)
        {
            using (var resolver = new ComponentResolver(container))
            {
                return resolver.Get<T>();
            }
        }

        public IResolverExpression<T> ExtendMap<T>()
        {
            // create a IResolverExpression with the values
            return ExtendMap<T>(_container);
        }

        public IResolverExpression<T> ExtendMap<T>(IMappableContainer container)
        {
            // create a IResolverExpression with the values
            using (var resolver = new ComponentResolver(container))
            {
                var map = resolver.GetComponent<T>();

                //return map.CreateResolverExpression<T>(container as IComponentCollection ?? MappingContainerManager.MappingContainer);

                // map to new container
                return map.CreateResolverExpression<T>(new MappingContainer());
            }
        }

        #endregion

        #region IDisposeable Implementation

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        internal bool IsDisposed { get; private set; }

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
        ~InjectionResolver()
        {
            Dispose(false);
        }

        #endregion
    }
}
