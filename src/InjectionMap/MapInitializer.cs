using InjectionMap.Extensions;
using InjectionMap.Internal;
using System;
using System.Linq;
using System.Reflection;

namespace InjectionMap
{
    public class MapInitializer : IDisposable
    {
        readonly IMappingContext _context;
        
        public MapInitializer()
        {
            _context = MappingContextManager.MappingContext;
        }

        public MapInitializer(string context)
        {
            context.EnsureArgumentNotNullOrEmpty("context");
            _context = new MappingContext(context);
        }

        public MapInitializer(IMappingContext context)
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


        /// <summary>
        /// Initializes all mappings in the <see cref="IMapInitializer"/>
        /// </summary>
        /// <param name="mapper"></param>
        public void Initialize(IMapInitializer mapper)
        {
            mapper.InitializeMap(_context);
        }

        /// <summary>
        /// Initializes all implementations of <see cref="IMapInitializer"/> in the assembly
        /// </summary>
        /// <param name="assemblyFile"></param>
        public void Initialize(string assemblyFile)
        {
            Initialize(Assembly.Load(assemblyFile));
        }

        /// <summary>
        /// Initializes all implementations of <see cref="IMapInitializer"/> in the assembly
        /// </summary>
        /// <param name="assembly"></param>
        public void Initialize(Assembly assembly)
        {
            assembly.EnsureArgumentNotNull("assembly");
            
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

        /// <summary>
        /// Initializes the <see cref="IMapInitializer"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Initialize<T>() where T : IMapInitializer
        {
            var type = typeof(T);

            type.EnsureTypeCanBeDefaultInstantiated();
            type.EnsureTypeIsImplemented(typeof(IMapInitializer));

            var obj = Activator.CreateInstance(type) as IMapInitializer;
            if (obj != null)
                obj.InitializeMap(_context);
        }
        
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
