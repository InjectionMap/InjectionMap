using System;
using WickedFlame.InjectionMap.Expressions;

namespace WickedFlame.InjectionMap.Mapping
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
        /// Creates a Mapping to TSvc
        /// </summary>
        /// <typeparam name="TSvc">The type to map</typeparam>
        /// <returns>The expression for the mapping</returns>
        public IMappingExpression<TSvc> Map<TSvc>()
        {
            return _container.Map<TSvc>();
        }

        /// <summary>
        /// Creates a mapping to TSvc with TImpl
        /// </summary>
        /// <typeparam name="TSvc">The key type to map</typeparam>
        /// <typeparam name="TImpl">The instance type to map</typeparam>
        /// <returns>The expression for the mapping</returns>
        public IBindingExpression<TImpl> Map<TSvc, TImpl>() where TImpl : TSvc
        {
            return _container.Map<TSvc, TImpl>();
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
