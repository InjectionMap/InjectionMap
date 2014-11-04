using System;
using System.Linq;
using System.Reflection;
using InjectionMap.Expressions;
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

        #region Static Implementation

        /// <summary>
        /// Initializes all mappings in the <see cref="IMapInitializer"/>
        /// </summary>
        /// <param name="mapper"></param>
        public static void Initialize(IMapInitializer mapper)
        {
            Initialize(mapper, MappingContextManager.MappingContext);
        }

        /// <summary>
        /// Initializes all mappings in the <see cref="IMapInitializer"/>
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="context">The IMappingProvider to store the mappings</param>
        public static void Initialize(IMapInitializer mapper, IMappingProvider context)
        {
            mapper.EnsureArgumentNotNull("mapper");
            context.EnsureArgumentNotNull("context");

            mapper.InitializeMap(context);
        }

        /// <summary>
        /// Initializes all implementations of <see cref="IMapInitializer"/> in the assembly
        /// </summary>
        /// <param name="assemblyFile"></param>
        public static void Initialize(string assemblyFile)
        {
            Initialize(Assembly.Load(assemblyFile), MappingContextManager.MappingContext);
        }

        /// <summary>
        /// Initializes all implementations of <see cref="IMapInitializer"/> in the assembly to the provided MappingProvider
        /// </summary>
        /// <param name="assemblyFile"></param>
        /// <param name="context"></param>
        public static void Initialize(string assemblyFile, IMappingProvider context)
        {
            assemblyFile.EnsureArgumentNotNullOrEmpty("assemblyFile");

            Initialize(Assembly.Load(assemblyFile), context);
        }

        /// <summary>
        /// Initializes all implementations of <see cref="IMapInitializer"/> in the assembly
        /// </summary>
        /// <param name="assembly"></param>
        public static void Initialize(Assembly assembly)
        {
            Initialize(assembly, MappingContextManager.MappingContext);
        }

        /// <summary>
        /// Initializes all implementations of <see cref="IMapInitializer"/> in the assembly to the provided MappingProvider
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="context"></param>
        public static void Initialize(Assembly assembly, IMappingProvider context)
        {
            assembly.EnsureArgumentNotNull("assembly");
            context.EnsureArgumentNotNull("context");

            InitializeInternal(assembly, context);
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

            Initialize<T>(MappingContextManager.MappingContext);
        }

        /// <summary>
        /// Initializes the <see cref="IMapInitializer"/> to the provided MappingProvider
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        public static void Initialize<T>(IMappingProvider context) where T : IMapInitializer
        {
            var type = typeof(T);

            type.EnsureTypeCanBeDefaultInstantiated();
            type.EnsureTypeIsImplemented(typeof(IMapInitializer));

            var obj = Activator.CreateInstance(type) as IMapInitializer;
            if (obj != null)
                obj.InitializeMap(context);
        }
        
        #endregion

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
