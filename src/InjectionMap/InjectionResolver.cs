﻿using System;
using System.Collections.Generic;
using InjectionMap.Internals;
using InjectionMap.Mapping;

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
        /// Removes all mappings of type T
        /// </summary>
        /// <typeparam name="T">The type of mappings to remove</typeparam>
        public void Clean<T>()
        {
            using (var resolver = new ComponentResolver(_container))
            {
                resolver.Clean<T>();
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
        ~InjectionResolver()
        {
            Dispose(false);
        }

        #endregion
    }
}