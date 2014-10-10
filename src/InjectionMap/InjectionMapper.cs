using System;
using System.Linq;
using System.Reflection;
using InjectionMap.Expressions;
using InjectionMap.Internal;
using InjectionMap.Extensions;

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
            container.EnsureArgumentNotNull("container");

            _container = container;
        }

        #region Static Implementation

        /// <summary>
        /// Initializes all mappings in the <see cref="IMapInitializer"/>
        /// </summary>
        /// <param name="mapper"></param>
        public static void Initialize(IMapInitializer mapper)
        {
            Initialize(mapper, MappingContainerManager.MappingContainer);
        }

        /// <summary>
        /// Initializes all mappings in the <see cref="IMapInitializer"/>
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="container">The IMappingProvider to store the mappings</param>
        public static void Initialize(IMapInitializer mapper, IMappingProvider container)
        {
            mapper.EnsureArgumentNotNull("mapper");
            container.EnsureArgumentNotNull("container");

            mapper.InitializeMap(container);
        }

        /// <summary>
        /// Initializes all implementations of <see cref="IMapInitializer"/> in the assembly
        /// </summary>
        /// <param name="assemblyFile"></param>
        public static void Initialize(string assemblyFile)
        {
            Initialize(Assembly.Load(assemblyFile), MappingContainerManager.MappingContainer);
        }

        /// <summary>
        /// Initializes all implementations of <see cref="IMapInitializer"/> in the assembly to the provided MappingProvider
        /// </summary>
        /// <param name="assemblyFile"></param>
        /// <param name="container"></param>
        public static void Initialize(string assemblyFile, IMappingProvider container)
        {
            assemblyFile.EnsureArgumentNotNullOrEmpty("assemblyFile");

            Initialize(Assembly.Load(assemblyFile), container);
        }

        /// <summary>
        /// Initializes all implementations of <see cref="IMapInitializer"/> in the assembly
        /// </summary>
        /// <param name="assembly"></param>
        public static void Initialize(Assembly assembly)
        {
            Initialize(assembly, MappingContainerManager.MappingContainer);
        }

        /// <summary>
        /// Initializes all implementations of <see cref="IMapInitializer"/> in the assembly to the provided MappingProvider
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="container"></param>
        public static void Initialize(Assembly assembly, IMappingProvider container)
        {
            assembly.EnsureArgumentNotNull("assembly");
            container.EnsureArgumentNotNull("container");

            InitializeInternal(assembly, container);
        }

        /// <summary>
        /// Initializes the <see cref="IMapInitializer"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void Initialize<T>() where T : IMapInitializer
        {
            //var type = typeof(T);

            //type.EnsureTypeCanBeDefaultInstantiated();
            //type.EnsureTypeIsImplemented(typeof(IMapInitializer));

            //var obj = Activator.CreateInstance(type) as IMapInitializer;
            //if (obj != null)
            //    obj.InitializeMap(MappingContainerManager.MappingContainer);

            Initialize<T>(MappingContainerManager.MappingContainer);
        }

        /// <summary>
        /// Initializes the <see cref="IMapInitializer"/> to the provided MappingProvider
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container"></param>
        public static void Initialize<T>(IMappingProvider container) where T : IMapInitializer
        {
            var type = typeof(T);

            type.EnsureTypeCanBeDefaultInstantiated();
            type.EnsureTypeIsImplemented(typeof(IMapInitializer));

            var obj = Activator.CreateInstance(type) as IMapInitializer;
            if (obj != null)
                obj.InitializeMap(container);
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
            using (var provider = new ComponentMapper(_container))
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

        internal static void InitializeInternal(Assembly assembly, IMappingProvider container)
        {
            var mappingtype = typeof(IMapInitializer);
            var types = assembly.GetTypes().Where(p => mappingtype.IsAssignableFrom(p) && !p.IsInterface);

            foreach (var type in types)
            {
                type.EnsureTypeCanBeDefaultInstantiated();

                var obj = Activator.CreateInstance(type) as IMapInitializer;
                if (obj == null)
                    continue;

                obj.InitializeMap(container);
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
