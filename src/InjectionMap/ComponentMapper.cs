using System;
using InjectionMap.Expressions;
using InjectionMap.Internal;

namespace InjectionMap
{
    public class ComponentMapper : IDisposable
    {
        IMappingProvider _context;

        /// <summary>
        /// Creates a componentmapper to add mappings
        /// </summary>
        public ComponentMapper()
        {
            _context = MappingContextManager.MappingContext;
        }

        /// <summary>
        /// Creates a componentmapper with a custom context
        /// </summary>
        /// <param name="context">The <see cref="IMappingProvider"/> to resolve the mappings from</param>
        public ComponentMapper(IMappingProvider context)
        {
            _context = context;

            if (_context == null)
                _context = MappingContextManager.MappingContext;
        }

        #region Implementation

        /// <summary>
        /// Creates a Mapping to TKey
        /// </summary>
        /// <typeparam name="TKey">The type to map</typeparam>
        /// <returns>The expression for the mapping</returns>
        public IMappingExpression<TKey> Map<TKey>()
        {
            return _context.Map<TKey>();
        }

        /// <summary>
        /// Creates a mapping to TKey with TMap
        /// </summary>
        /// <typeparam name="TKey">The key type to map</typeparam>
        /// <typeparam name="TMap">The instance type to map</typeparam>
        /// <returns>The expression for the mapping</returns>
        public IBindingExpression<TMap> Map<TKey, TMap>() where TMap : TKey
        {
            return _context.Map<TKey, TMap>();
        }

        /// <summary>
        /// Removes all mappings of type T
        /// </summary>
        /// <typeparam name="T">The type of mappings to remove</typeparam>
        public void Clean<T>()
        {
            _context.Clean<T>();
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
                    _context = null;

                    IsDisposed = true;
                    GC.SuppressFinalize(this);
                }
            }
        }

        /// <summary>
        /// Releases resources before the object is reclaimed by garbage collection.
        /// </summary>
        ~ComponentMapper()
        {
            Dispose(false);
        }

        #endregion
    }
}
