using System;
using System.Linq;
using System.Reflection;
using InjectionMap.Internal;
using InjectionMap.Extensions;
using System.Linq.Expressions;

namespace InjectionMap
{
    public class InjectionMapper : IDisposable
    {
        IMappingContext _context;

        /// <summary>
        /// Creates a InjectionMapper that mapps to the common mappingcontext
        /// </summary>
        public InjectionMapper()
        {
        }

        /// <summary>
        /// Creates a InjectionMapper that mapps to the common mappingcontext
        /// </summary>
        /// <param name="context">the name of the stored context</param>
        public InjectionMapper(string context)
        {
            context.EnsureArgumentNotNullOrEmpty("context");

            _context = new MappingContext(context);
        }

        /// <summary>
        /// Creates a InjectionMapper with a custom context to map to
        /// </summary>
        /// <param name="context">The <see cref="IMappingContext"/> to add the mappings to</param>
        public InjectionMapper(IMappingContext context)
        {
            context.EnsureArgumentNotNull("context");

            _context = context;
        }

        public IMappingContext Context
        {
            get
            {
                return _context;
            }
        }
        
        #region Implementation

        /// <summary>
        /// Creates a Mapping to TKey without defining the mapped type or object
        /// </summary>
        /// <typeparam name="TKey">The type to map</typeparam>
        /// <returns>The expression for the mapping</returns>
        public IMappingExpression<TKey> Map<TKey>()
        {
            using (var provider = new ComponentMapper(_context))
            {
                return provider.Map<TKey>();
            }
        }

        /// <summary>
        /// Creates a mapping to TKey with TMap
        /// </summary>
        /// <typeparam name="TKey">The key type to map</typeparam>
        /// <typeparam name="TMap">The instance type to map</typeparam>
        /// <returns>The expression for the mapping</returns>
        public IBindingExpression<TMap> Map<TKey, TMap>() where TMap : TKey
        {
            using (var provider = new ComponentMapper(_context))
            {
                return provider.Map<TKey, TMap>();
            }
        }

        public IBindingExpression<TKey> Map<TKey>(Expression<Func<TKey>> predicate)
        {
            using (var provider = new ComponentMapper(_context))
            {
                return provider.Map<TKey>().For(predicate);
            }
        }

        public IBindingExpression<TKey> Map<TKey>(TKey value)
        {
            using (var provider = new ComponentMapper(_context))
            {
                return provider.Map<TKey>().For(value);
            }
        }

        /// <summary>
        /// Removes all mappings of type T
        /// </summary>
        /// <typeparam name="T">The type of mappings to remove</typeparam>
        public void Clean<T>()
        {
            using (var provider = new ComponentMapper(_context))
            {
                provider.Clean<T>();
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
                    _context = null;

                    IsDisposed = true;
                    GC.SuppressFinalize(this);
                }
            }
        }

        /// <summary>
        /// Releases resources before the object is reclaimed by garbage collection.
        /// </summary>
        ~InjectionMapper()
        {
            Dispose(false);
        }

        #endregion
    }
}
