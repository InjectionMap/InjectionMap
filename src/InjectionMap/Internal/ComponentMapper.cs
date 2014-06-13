using System;
using InjectionMap.Expressions;

namespace InjectionMap.Internal
{
    internal class ComponentMapper : IDisposable
    {
        IMappingProvider _container;

        /// <summary>
        /// Creates a componentmapper to add mappings
        /// </summary>
        public ComponentMapper()
        {
            _container = MappingContainerManager.MappingContainer;
        }

        /// <summary>
        /// Creates a componentmapper with a custom container
        /// </summary>
        /// <param name="container">The <see cref="IMappingProvider"/> to resolve the mappings from</param>
        public ComponentMapper(IMappingProvider container)
        {
            _container = container;

            if (_container == null)
                _container = MappingContainerManager.MappingContainer;
        }

        #region Implementation

        /// <summary>
        /// Creates a Mapping to TKey
        /// </summary>
        /// <typeparam name="TKey">The type to map</typeparam>
        /// <returns>The expression for the mapping</returns>
        public IMappingExpression<TKey> Map<TKey>()
        {
            return _container.Map<TKey>();
        }

        /// <summary>
        /// Creates a mapping to TKey with TMap
        /// </summary>
        /// <typeparam name="TKey">The key type to map</typeparam>
        /// <typeparam name="TMap">The instance type to map</typeparam>
        /// <returns>The expression for the mapping</returns>
        public IBindingExpression<TMap> Map<TKey, TMap>() where TMap : TKey
        {
            return _container.Map<TKey, TMap>();
        }

        /// <summary>
        /// Removes all mappings of type T
        /// </summary>
        /// <typeparam name="T">The type of mappings to remove</typeparam>
        public void Clean<T>()
        {
            _container.Clean<T>();
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
                    _container = null;

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
