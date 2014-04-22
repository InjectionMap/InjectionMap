using System;
using System.Linq;
using System.Reflection;
using WickedFlame.InjectionMap.Mapping;

namespace WickedFlame.InjectionMap
{
    public class InjectionMapper : IDisposable
    {
        #region Static Implementation

        public static void Initialize(IInjectionMapping mapper)
        {
            mapper.Register(MappingManager.MappingContainer);
        }

        public static void Initialize(Assembly assembly)
        {
            var type = typeof(IInjectionMapping);
            var types = assembly.GetTypes().Where(p => type.IsAssignableFrom(p) && !p.IsInterface);

            foreach (var t in types)
            {
                var obj = Activator.CreateInstance(t) as IInjectionMapping;
                if (obj == null)
                    continue;

                obj.Register(MappingManager.MappingContainer);
            }
        }

        #endregion

        #region Implementation

        public IMappingExpression Map<TSvc>()
        {
            using (var provider = new MappingProvider())
            {
                return provider.Map<TSvc>();
            }
        }

        public IBindingExpression<TImpl> Map<TSvc, TImpl>() where TImpl : TSvc
        {
            using (var provider = new MappingProvider())
            {
                return provider.Map<TSvc, TImpl>();
            }
        }

        /// <summary>
        /// Removes all mappings of type T
        /// </summary>
        /// <typeparam name="T">The type of mappings to remove</typeparam>
        public void Clean<T>()
        {
            using (var provider = new MappingProvider())
            {
                provider.Clean<T>();
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
        ~InjectionMapper()
        {
            Dispose(false);
        }

        #endregion
    }
}
