using InjectionMap.Composition;
using System;
using System.Collections.Generic;

namespace InjectionMap.Internal
{
    /// <summary>
    /// Resolves the value from the passed type without using the maped components. This is used when resolving from a type that was not mapped previously.
    /// </summary>
    internal class TypeResolver : IResolver, IDisposable
    {
        public TypeResolver(IComponentProvider context)
        {
            _context = context;
        }

        #region Implementation

        readonly IComponentProvider _context;
        public IComponentProvider Context
        {
            get
            {
                return _context;
            }
        }

        /// <summary>
        /// Creates an instance of T
        /// </summary>
        /// <typeparam name="T">The type to create</typeparam>
        /// <returns>A new instance of T</returns>
        public T Get<T>()
        {
            return CompositionService.Compose<T>(_context);
        }

        /// <summary>
        /// Creates an instance of T
        /// </summary>
        /// <param name="type">The type to create</param>
        /// <returns>A new instance of T</returns>
        public object Get(Type type)
        {
            return CompositionService.Compose(type, _context);
        }

        /// <summary>
        /// Creates all instances of T
        /// </summary>
        /// <typeparam name="T">The type to create</typeparam>
        /// <returns>A new instance of T</returns>
        public IEnumerable<T> GetAll<T>()
        {
            yield return CompositionService.Compose<T>(_context);
        }

        /// <summary>
        /// Gets the IMappingComponent of the first occurance of T
        /// </summary>
        /// <typeparam name="T">The mapped type</typeparam>
        /// <returns>The first IMappingComponent of T</returns>
        public IMappingComponent GetComponent<T>()
        {
            return new MappingComponent<T>()
            {
                KeyType = typeof(T)
            };
        }

        /// <summary>
        /// Gets all IMappingComponents mapped to T
        /// </summary>
        /// <typeparam name="T">The mapped type</typeparam>
        /// <returns>All IMappingComonents of T</returns>
        public IEnumerable<IMappingComponent> GetAllComponents<T>()
        {
            return new List<IMappingComponent>
            {
                new MappingComponent<T>
                {
                    KeyType = typeof(T)
                }
            };
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
        ~TypeResolver()
        {
            Dispose(false);
        }

        #endregion
    }
}
