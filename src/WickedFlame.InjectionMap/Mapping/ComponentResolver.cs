using System;
using System.Collections.Generic;
using System.Linq;
using WickedFlame.InjectionMap.Composition;

namespace WickedFlame.InjectionMap.Mapping
{
    internal class ComponentResolver : IDisposable
    {
        IComponentProvider _container;

        /// <summary>
        /// Creates a componentresolver
        /// </summary>
        public ComponentResolver()
        {
            _container = MappingContainerManager.MappingContainer;
        }

        /// <summary>
        /// Creates a componentresolver with a custom container
        /// </summary>
        /// <param name="container">The <see cref="IMappingProvider"/> to resolve the mappings from</param>
        public ComponentResolver(IComponentProvider container)
        {
            _container = container;

            if (_container == null)
                _container = MappingContainerManager.MappingContainer;
        }

        #region Implementation

        /// <summary>
        /// Gets the first occurance of T
        /// </summary>
        /// <typeparam name="T">The mapped type</typeparam>
        /// <returns>The first occurance of T</returns>
        public T Get<T>()
        {
            return _container.Get<T>().Select(c => CompositionService.Compose<T>(c)).FirstOrDefault();
        }

        /// <summary>
        /// Gets the first occurance of the type
        /// </summary>
        /// <param name="type">The mapped type</param>
        /// <returns>The first occurance of the mapped type</returns>
        public object Get(Type type)
        {
            return _container.Get(type).Select(c => CompositionService.Compose(c)).FirstOrDefault();
        }

        /// <summary>
        /// Gets the IMappingComponent of the first occurance of T
        /// </summary>
        /// <typeparam name="T">The mapped type</typeparam>
        /// <returns>The first IMappingComponent of T</returns>
        public IMappingComponent GetComponent<T>()
        {
            return _container.Get<T>().FirstOrDefault();
        }

        /// <summary>
        /// Gets all IMappingComponents mapped to T
        /// </summary>
        /// <typeparam name="T">The mapped type</typeparam>
        /// <returns>All IMappingComonents of T</returns>
        public IEnumerable<IMappingComponent> GetAllComponents<T>()
        {
            return _container.Get<T>().Where(c => c.KeyType == typeof(T));
        }

        /// <summary>
        /// Gets all accurances of T
        /// </summary>
        /// <typeparam name="T">The mapped type</typeparam>
        /// <returns>All T</returns>
        public IEnumerable<T> GetAll<T>()
        {
            return _container.Get<T>().Select(c => CompositionService.Compose<T>(c));
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
        ~ComponentResolver()
        {
            Dispose(false);
        }

        #endregion
    }
}
