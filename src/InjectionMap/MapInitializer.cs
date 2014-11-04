using InjectionMap.Extensions;
using InjectionMap.Internal;
using System;
using System.Linq;
using System.Reflection;

namespace InjectionMap
{
    public class MapInitializer : IDisposable
    {
        readonly IMappingProvider _context;

        public MapInitializer()
        {
            _context = MappingContextManager.MappingContext;
        }

        public MapInitializer(IMappingProvider context)
        {
            _context = context;
        }

        /// <summary>
        /// Initializes all mappings in the <see cref="IMapInitializer"/>
        /// </summary>
        /// <param name="mapper"></param>
        public void Initialize(IMapInitializer mapper)
        {
            //Initialize(mapper/*, MappingContextManager.MappingContext*/);
            mapper.InitializeMap(_context);
        }

        ///// <summary>
        ///// Initializes all mappings in the <see cref="IMapInitializer"/>
        ///// </summary>
        ///// <param name="mapper"></param>
        ///// <param name="context">The IMappingProvider to store the mappings</param>
        //public void Initialize(IMapInitializer mapper, IMappingProvider context)
        //{
        //    mapper.EnsureArgumentNotNull("mapper");
        //    context.EnsureArgumentNotNull("context");

        //    mapper.InitializeMap(context);
        //}

        /// <summary>
        /// Initializes all implementations of <see cref="IMapInitializer"/> in the assembly
        /// </summary>
        /// <param name="assemblyFile"></param>
        public void Initialize(string assemblyFile)
        {
            Initialize(Assembly.Load(assemblyFile)/*, MappingContextManager.MappingContext*/);
        }

        ///// <summary>
        ///// Initializes all implementations of <see cref="IMapInitializer"/> in the assembly to the provided MappingProvider
        ///// </summary>
        ///// <param name="assemblyFile"></param>
        ///// <param name="context"></param>
        //public void Initialize(string assemblyFile, IMappingProvider context)
        //{
        //    assemblyFile.EnsureArgumentNotNullOrEmpty("assemblyFile");

        //    Initialize(Assembly.Load(assemblyFile), context);
        //}

        /// <summary>
        /// Initializes all implementations of <see cref="IMapInitializer"/> in the assembly
        /// </summary>
        /// <param name="assembly"></param>
        public void Initialize(Assembly assembly)
        {
            //Initialize(assembly, MappingContextManager.MappingContext);
            assembly.EnsureArgumentNotNull("assembly");

            //InitializeInternal(assembly, _context);

            var mappingtype = typeof(IMapInitializer);
            var types = assembly.GetTypes().Where(p => mappingtype.IsAssignableFrom(p) && !p.IsInterface);

            foreach (var type in types)
            {
                type.EnsureTypeCanBeDefaultInstantiated();

                var obj = Activator.CreateInstance(type) as IMapInitializer;
                if (obj == null)
                    continue;

                obj.InitializeMap(_context);
            }
        }

        ///// <summary>
        ///// Initializes all implementations of <see cref="IMapInitializer"/> in the assembly to the provided MappingProvider
        ///// </summary>
        ///// <param name="assembly"></param>
        ///// <param name="context"></param>
        //public void Initialize(Assembly assembly, IMappingProvider context)
        //{
        //    assembly.EnsureArgumentNotNull("assembly");
        //    context.EnsureArgumentNotNull("context");

        //    InitializeInternal(assembly, context);
        //}

        /// <summary>
        /// Initializes the <see cref="IMapInitializer"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Initialize<T>() where T : IMapInitializer
        {
            //Initialize<T>(MappingContextManager.MappingContext);

            var type = typeof(T);

            type.EnsureTypeCanBeDefaultInstantiated();
            type.EnsureTypeIsImplemented(typeof(IMapInitializer));

            var obj = Activator.CreateInstance(type) as IMapInitializer;
            if (obj != null)
                obj.InitializeMap(_context);
        }

        ///// <summary>
        ///// Initializes the <see cref="IMapInitializer"/> to the provided MappingProvider
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="context"></param>
        //public static void Initialize<T>(IMappingProvider context) where T : IMapInitializer
        //{
        //    var type = typeof(T);

        //    type.EnsureTypeCanBeDefaultInstantiated();
        //    type.EnsureTypeIsImplemented(typeof(IMapInitializer));

        //    var obj = Activator.CreateInstance(type) as IMapInitializer;
        //    if (obj != null)
        //        obj.InitializeMap(context);
        //}

        //#region Internal Implementation

        //internal static void InitializeInternal(Assembly assembly, IMappingProvider container)
        //{
        //    var mappingtype = typeof(IMapInitializer);
        //    var types = assembly.GetTypes().Where(p => mappingtype.IsAssignableFrom(p) && !p.IsInterface);

        //    foreach (var type in types)
        //    {
        //        type.EnsureTypeCanBeDefaultInstantiated();

        //        var obj = Activator.CreateInstance(type) as IMapInitializer;
        //        if (obj == null)
        //            continue;

        //        obj.InitializeMap(container);
        //    }
        //}

        //#endregion

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
        ~MapInitializer()
        {
            Dispose(false);
        }

        #endregion
    }
}
