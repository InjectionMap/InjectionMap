using System;
using System.Collections.Generic;
using InjectionMap.Internal;
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
            container.EnsureArgumentNotNull("container");

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
            using (var resolver = ResolverFactory.GetResolver<T>(_container))
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
            using (var resolver = ResolverFactory.GetResolver(type, _container))
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
            using (var resolver = ResolverFactory.GetResolver<T>(_container))
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
            using (var resolver = ResolverFactory.GetResolver<T>(container))
            {
                return resolver.Get<T>();
            }
        }

        /// <summary>
        /// Extends the first occurance of a existing map of the given type. This affects the stored mapping and all future resolvings. 
        /// </summary>
        /// <typeparam name="T">The key type of the registered map to</typeparam>
        /// <returns>IResolverExpression{T}</returns>
        public IResolverExpression<T> ExtendMap<T>()
        {
            // create a IResolverExpression with the values
            return ExtendMap<T>(_container);
        }

        /// <summary>
        /// Extends the first occurance of a existing map of the given type. This affects the stored mapping and all future resolvings. 
        /// </summary>
        /// <typeparam name="T">The key type of the registered map to</typeparam>
        /// <param name="container">The container containing the map</param>
        /// <returns>IResolverExpression{T}</returns>
        public IResolverExpression<T> ExtendMap<T>(IMappableContainer container)
        {
            // create a IResolverExpression with the values
            using (var resolver = ResolverFactory.GetResolver<T>(container))
            {
                var map = resolver.GetComponent<T>();
                if (map == null)
                    throw new ResolverException(typeof(T));
                
                var cont = container as IComponentCollection;
                if (cont == null)
                    cont = MappingContainerManager.MappingContainer;

                return map.CreateExtendedResolverExpression<T>(cont);
            }
        }

        /// <summary>
        /// Extends all occurances of existing mappings of the given type. This affects the stored mapping and all future resolvings. 
        /// </summary>
        /// <typeparam name="T">The key type of the registered map to</typeparam>
        /// <returns>IMultiResolverExpression{T}</returns>
        public IMultiResolverExpression<T> ExtendAll<T>()
        {
            return ExtendAll<T>(_container);
        }

        /// <summary>
        /// Extends all occurances of existing mappings of the given type. This affects the stored mapping and all future resolvings. 
        /// </summary>
        /// <typeparam name="T">The key type of the registered map to</typeparam>
        /// <param name="container">The container containing the maps</param>
        /// <returns>IMultiResolverExpression{T}</returns>
        public IMultiResolverExpression<T> ExtendAll<T>(IMappableContainer container)
        {
            // create a IMultiResolverExpression with the values
            using (var resolver = ResolverFactory.GetResolver<T>(container))
            {
                var map = resolver.GetComponent<T>();

                if (map == null)
                    throw new ResolverException(typeof(T));

                var cont = container as IComponentCollection;
                if (cont == null)
                    cont = MappingContainerManager.MappingContainer;

                return map.CreateExtendedMultiResolverExpression<T>(cont);
            }
        }

        /// <summary>
        /// Creates a new IResolverExpression for the given type by copying the original map and allows extendig the mapping. This will not affect the stored mapping.
        /// </summary>
        /// <typeparam name="T">The key type of the registered map to</typeparam>
        /// <returns>IResolverExpression{T}</returns>
        public IResolverExpression<T> For<T>()
        {
            // create a IResolverExpression with the values
            return For<T>(_container);
        }

        /// <summary>
        /// Creates a new IResolverExpression for the given type by copying the original map and allows extendig the mapping. This will not affect the stored mapping.
        /// </summary>
        /// <typeparam name="T">The key type of the registered map to</typeparam>
        /// <param name="container">The container containing the map</param>
        /// <returns>IResolverExpression{T}</returns>
        public IResolverExpression<T> For<T>(IMappableContainer container)
        {
            // create a IResolverExpression with the values
            using (var resolver = ResolverFactory.GetResolver<T>(container))
            {
                var map = resolver.GetComponent<T>();
                if (map == null)
                    throw new ResolverException(typeof(T));

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
