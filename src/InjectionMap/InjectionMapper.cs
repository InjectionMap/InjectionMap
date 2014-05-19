using System;
using System.Linq;
using System.Reflection;
using InjectionMap.Expressions;
using InjectionMap.Internals;
using InjectionMap.Mapping;

namespace InjectionMap
{
    public class InjectionMapper : IDisposable
    {
        IMappableContainer _container;

        /// <summary>
        /// Creates a InjectionMapper that mapps to the common mappingcontainer
        /// </summary>
        public InjectionMapper()
        {
        }

        /// <summary>
        /// Creates a InjectionMapper with a custom container to map to
        /// </summary>
        /// <param name="container">The <see cref="IMappableContainer"/> to add the mappings to</param>
        public InjectionMapper(IMappableContainer container)
        {
            Ensure.ArgumentNotNull(container, "container");

            _container = container;
        }

        #region Static Implementation

        /// <summary>
        /// Initializes all mappings in the <see cref="IInjectionMapping"/>
        /// </summary>
        /// <param name="mapper"></param>
        public static void Initialize(IInjectionMapping mapper)
        {
            Ensure.ArgumentNotNull(mapper, "mapper");
            
            mapper.InitializeMap(MappingContainerManager.MappingContainer);
        }

        /// <summary>
        /// Initializes all implementations of <see cref="IInjectionMapping"/> in the assembly
        /// </summary>
        /// <param name="assemblyFile"></param>
        public static void Initialize(string assemblyFile)
        {
            Ensure.ArgumentNotNullOrEmpty(assemblyFile, "assemblyFile");

            //Initialize(Assembly.LoadFrom(assemblyFile));
            Initialize(Assembly.Load(assemblyFile));
        }

        /// <summary>
        /// Initializes all implementations of <see cref="IInjectionMapping"/> in the assembly
        /// </summary>
        /// <param name="assembly"></param>
        public static void Initialize(Assembly assembly)
        {
            Ensure.ArgumentNotNull(assembly, "assembly");

            InitializeInternal(assembly);
        }

        /// <summary>
        /// Initializes the <see cref="IInjectionMapping"/>
        /// </summary>
        /// <param name="type"></param>
        public static void Initialize<T>() where T : IInjectionMapping
        {
            var type = typeof(T);

            Ensure.CanBeDefaultInstantiated(type);
            Ensure.TypeImplements(type, typeof(IInjectionMapping));

            var obj = Activator.CreateInstance(type) as IInjectionMapping;
            if (obj != null)
                obj.InitializeMap(MappingContainerManager.MappingContainer);
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Creates a Mapping to TKey
        /// </summary>
        /// <typeparam name="TKey">The type to map</typeparam>
        /// <returns>The expression for the mapping</returns>
        public IMappingExpression<TKey> Map<TKey>()
        {
            using (var provider = new ComponentMapper())
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
            using (var provider = new ComponentMapper(_container))
            {
                return provider.Map<TKey, TMap>();
            }
        }

        /// <summary>
        /// Removes all mappings of type T
        /// </summary>
        /// <typeparam name="T">The type of mappings to remove</typeparam>
        public void Clean<T>()
        {
            using (var provider = new ComponentMapper(_container))
            {
                provider.Clean<T>();
            }
        }

        #endregion

        #region Internal Implementation

        internal static void InitializeInternal(Assembly assembly)
        {
            var mappingtype = typeof(IInjectionMapping);
            var types = assembly.GetTypes().Where(p => mappingtype.IsAssignableFrom(p) && !p.IsInterface);

            foreach (var type in types)
            {
                Ensure.CanBeDefaultInstantiated(type);

                var obj = Activator.CreateInstance(type) as IInjectionMapping;
                if (obj == null)
                    continue;

                obj.InitializeMap(MappingContainerManager.MappingContainer);
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
                    _container = null;

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
