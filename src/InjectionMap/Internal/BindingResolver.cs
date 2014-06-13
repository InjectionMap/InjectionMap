using InjectionMap.Composition;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InjectionMap.Internal
{
    internal class BindingResolver : IResolver, IDisposable
    {
        #region Implementation

        /// <summary>
        /// Creates an instance of T
        /// </summary>
        /// <typeparam name="T">The type to create</typeparam>
        /// <returns>A new instance of T</returns>
        public T Get<T>()
        {
            return CompositionService.Compose<T>();
        }

        /// <summary>
        /// Creates an instance of T
        /// </summary>
        /// <param name="type">The type to create</param>
        /// <returns>A new instance of T</returns>
        public object Get(Type type)
        {
            return CompositionService.Compose(type);
        }

        /// <summary>
        /// Creates all instances of T
        /// </summary>
        /// <typeparam name="T">The type to create</typeparam>
        /// <returns>A new instance of T</returns>
        public IEnumerable<T> GetAll<T>()
        {
            yield return CompositionService.Compose<T>();
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
        ~BindingResolver()
        {
            Dispose(false);
        }

        #endregion
    }
}
