using System;
using System.Collections.Generic;
using System.Linq;
using InjectionMap.Composition;

namespace InjectionMap.Internal
{
    internal class ComponentResolver : IResolver, IDisposable
    {
        IComponentProvider _context;

        /// <summary>
        /// Creates a componentresolver
        /// </summary>
        public ComponentResolver()
        {
            _context = MappingContextManager.MappingContext;
        }

        /// <summary>
        /// Creates a componentresolver with a custom container
        /// </summary>
        /// <param name="context">The <see cref="IMappingProvider"/> to resolve the mappings from</param>
        public ComponentResolver(IComponentProvider context)
        {
            _context = context;

            if (_context == null)
                _context = MappingContextManager.MappingContext;
        }

        #region Implementation

        /// <summary>
        /// Gets the first occurance of T
        /// </summary>
        /// <typeparam name="T">The mapped type</typeparam>
        /// <returns>The first occurance of T</returns>
        public T Get<T>()
        {
            return _context.Get<T>().Select(c => CompositionService.Compose<T>(c)).FirstOrDefault();
        }

        /// <summary>
        /// Gets the first occurance of the type
        /// </summary>
        /// <param name="type">The mapped type</param>
        /// <returns>The first occurance of the mapped type</returns>
        public object Get(Type type)
        {
            return _context.Get(type).Select(c => CompositionService.Compose(c)).FirstOrDefault();
        }

        /// <summary>
        /// Gets all accurances of T
        /// </summary>
        /// <typeparam name="T">The mapped type</typeparam>
        /// <returns>All T</returns>
        public IEnumerable<T> GetAll<T>()
        {
            return _context.Get<T>().Select(c => CompositionService.Compose<T>(c));
        }

        /// <summary>
        /// Gets the IMappingComponent of the first occurance of T
        /// </summary>
        /// <typeparam name="T">The mapped type</typeparam>
        /// <returns>The first IMappingComponent of T</returns>
        public IMappingComponent GetComponent<T>()
        {
            return _context.Get<T>().FirstOrDefault();
        }

        /// <summary>
        /// Gets all IMappingComponents mapped to T
        /// </summary>
        /// <typeparam name="T">The mapped type</typeparam>
        /// <returns>All IMappingComonents of T</returns>
        public IEnumerable<IMappingComponent> GetAllComponents<T>()
        {
            return _context.Get<T>().Where(c => c.KeyType == typeof(T));
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
                    _context = null;

                    IsDisposed = true;
                    GC.SuppressFinalize(this);
                }
            }
        }

        /// <summary>
        /// Releases resources before the object is reclaimed by garbage collection.
        /// </summary>
        ~ComponentResolver()
        {
            Dispose(false);
        }

        #endregion
    }
}
