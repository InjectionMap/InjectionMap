using System;
using System.Collections.Generic;
using InjectionMap.Internal;
using InjectionMap.Extensions;
using System.Linq.Expressions;

namespace InjectionMap
{
    public class InjectionResolver : IDisposable
    {
        readonly IMappingContext _context;

        /// <summary>
        /// Creates a InjectionResolver to resolve from the common mappingcontext
        /// </summary>
        public InjectionResolver()
        {
        }

        /// <summary>
        /// Creates a InjectionResolver to resolve from the common mappingcontext
        /// </summary>
        /// <param name="context">Name of the context that containes the mappings</param>
        public InjectionResolver(string context)
        {
            context.EnsureArgumentNotNullOrEmpty("context");

            _context = new MappingContext(context);
        }

        /// <summary>
        /// Creates a InjectionResolver with a custom container to resolve from
        /// </summary>
        /// <param name="context">The <see cref="IMappingContext"/> to resolve the mappings from</param>
        public InjectionResolver(IMappingContext context)
        {
            context.EnsureArgumentNotNull("context");

            _context = context;
        }

        #region Implementation

        /// <summary>
        /// Resolves the first found occurance of T
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <returns>Instance of first found mapping of T</returns>
        public T Resolve<T>()
        {
            using (var resolver = ResolverFactory.GetResolver<T>(_context))
            {
                return resolver.Get<T>();
            }
        }

        /// <summary>
        /// Resolves the first found occurance of T and passes the arguments to the constructor
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <param name="arguments">The Arguments to be passed to the constructor</param>
        /// <returns>Instance of first found mapping of T</returns>
        public T Resolve<T>(params Expression<Func<object>>[] arguments)
        {
            // create a context to store the copy of the component
            var copyContext = new MappingContext();

            using (var resolver = ResolverFactory.GetResolver<T>(_context))
            {
                // create a copy of the component and add all arguments
                var component = resolver.GetComponent<T>().CopyComponent();
                var expression = component.CreateResolverExpression<T>(copyContext);
                foreach (var argument in arguments)
                {
                    expression.WithArgument(argument);
                }
            }

            // resolve from the new context
            using (var resolver = ResolverFactory.GetResolver<T>(copyContext))
            {
                return resolver.Get<T>();
            }
        }

        /// <summary>
        /// Resolves the first found occurance of T and passes the arguments to the constructor
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <param name="arguments">The Arguments to be passed to the constructor</param>
        /// <returns>Instance of first found mapping of T</returns>
        public T Resolve<T>(params Argument[] arguments)
        {
            // create a context to store the copy of the component
            var copyContext = new MappingContext();

            using (var resolver = ResolverFactory.GetResolver<T>(_context))
            {
                // create a copy of the component and add all arguments
                var component = resolver.GetComponent<T>().CopyComponent();
                var expression = component.CreateResolverExpression<T>(copyContext);
                foreach (var argument in arguments)
                {
                    expression.WithArgument(argument.Name, argument.Value);
                }
            }

            // resolve from the new context
            using (var resolver = ResolverFactory.GetResolver<T>(copyContext))
            {
                return resolver.Get<T>();
            }
        }

        /// <summary>
        /// Resolves the first found occurance of the type
        /// </summary>
        /// <param name="type">the type to resolve</param>
        /// <returns>Instance of first found mapping of type</returns>
        public object Resolve(Type type)
        {
            using (var resolver = ResolverFactory.GetResolver(type, _context))
            {
                return resolver.Get(type);
            }
        }

        /// <summary>
        /// Resolves T from the given context
        /// </summary>
        /// <typeparam name="T">The type to resolve</typeparam>
        /// <param name="context">The container to resolve from</param>
        /// <returns>A instance of T</returns>
        public T Resolve<T>(IMappingContext context)
        {
            using (var resolver = ResolverFactory.GetResolver<T>(context))
            {
                return resolver.Get<T>();
            }
        }

        /// <summary>
        /// Resolves T from the given context and passes the arguments to the constructor
        /// </summary>
        /// <typeparam name="T">The type to resolve</typeparam>
        /// <param name="context">The container to resolve from</param>
        /// <param name="arguments">The arguments for the constructor</param>
        /// <returns>A instance of T</returns>
        public T Resolve<T>(IMappingContext context, params Expression<Func<object>>[] arguments)
        {
            // create a context to store the copy of the component
            var copyContext = new MappingContext();

            using (var resolver = ResolverFactory.GetResolver<T>(context))
            {
                // create a copy of the component and add all arguments
                var component = resolver.GetComponent<T>().CopyComponent();
                var expression = component.CreateResolverExpression<T>(copyContext);
                foreach (var argument in arguments)
                {
                    expression.WithArgument(argument);
                }
            }

            // resolve from the new context
            using (var resolver = ResolverFactory.GetResolver<T>(copyContext))
            {
                return resolver.Get<T>();
            }
        }

        /// <summary>
        /// Resolves T from the given context and passes the arguments to the constructor
        /// </summary>
        /// <typeparam name="T">The type to resolve</typeparam>
        /// <param name="context">The container to resolve from</param>
        /// <param name="arguments">The arguments for the constructor</param>
        /// <returns>A instance of T</returns>
        public T Resolve<T>(IMappingContext context, params Argument[] arguments)
        {
            // create a context to store the copy of the component
            var copyContext = new MappingContext();

            using (var resolver = ResolverFactory.GetResolver<T>(context))
            {
                // create a copy of the component and add all arguments
                var component = resolver.GetComponent<T>().CopyComponent();
                var expression = component.CreateResolverExpression<T>(copyContext);
                foreach (var argument in arguments)
                {
                    expression.WithArgument(argument.Name, argument.Value);
                }
            }

            // resolve from the new context
            using (var resolver = ResolverFactory.GetResolver<T>(copyContext))
            {
                return resolver.Get<T>();
            }
        }

        /// <summary>
        /// Resolves all mappings of type T
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <returns>All mappings of T</returns>
        public IEnumerable<T> ResolveMultiple<T>()
        {
            using (var resolver = ResolverFactory.GetResolver<T>(_context))
            {
                return resolver.GetAll<T>();
            }
        }

        /// <summary>
        /// Extends the first occurance of a existing map of the given type. This affects the stored mapping and all future resolvings. 
        /// </summary>
        /// <typeparam name="T">The key type of the registered map to</typeparam>
        /// <returns>IResolverExpression{T}</returns>
        public IResolverExpression<T> ExtendMap<T>()
        {
            // create a IResolverExpression with the values
            return ExtendMap<T>(_context);
        }

        /// <summary>
        /// Extends the first occurance of a existing map of the given type. This affects the stored mapping and all future resolvings. 
        /// </summary>
        /// <typeparam name="T">The key type of the registered map to</typeparam>
        /// <param name="context">The container containing the map</param>
        /// <returns>IResolverExpression{T}</returns>
        public IResolverExpression<T> ExtendMap<T>(IMappingContext context)
        {
            // create a IResolverExpression with the values
            using (var resolver = ResolverFactory.GetResolver<T>(context))
            {
                var map = resolver.GetComponent<T>();
                if (map == null)
                    throw new ResolverException(typeof(T));

                var cont = context as IComponentCollection;
                if (cont == null)
                    cont = MappingContextManager.MappingContext;

                return map.CreateExtendedResolverExpression<T>(cont);
            }
        }

        /// <summary>
        /// Extends all occurances of existing mappings of the given type. This affects the stored mapping and all future resolvings. 
        /// </summary>
        /// <typeparam name="T">The key type of the registered map to</typeparam>
        /// <returns>IMultiResolverExpression{T}</returns>
        public IMultiResolverExpression<T> ExtendAll<T>()
        {
            return ExtendAll<T>(_context);
        }

        /// <summary>
        /// Extends all occurances of existing mappings of the given type. This affects the stored mapping and all future resolvings. 
        /// </summary>
        /// <typeparam name="T">The key type of the registered map to</typeparam>
        /// <param name="context">The container containing the maps</param>
        /// <returns>IMultiResolverExpression{T}</returns>
        public IMultiResolverExpression<T> ExtendAll<T>(IMappingContext context)
        {
            // create a IMultiResolverExpression with the values
            using (var resolver = ResolverFactory.GetResolver<T>(context))
            {
                var map = resolver.GetComponent<T>();

                if (map == null)
                    throw new ResolverException(typeof(T));

                var cont = context as IComponentCollection;
                if (cont == null)
                    cont = MappingContextManager.MappingContext;

                return map.CreateExtendedMultiResolverExpression<T>(cont);
            }
        }

        /// <summary>
        /// Creates a new IResolverExpression for the given type by copying the original map and allows extendig the mapping. This will not affect the stored mapping.
        /// </summary>
        /// <typeparam name="T">The key type of the registered map to</typeparam>
        /// <returns>IResolverExpression{T}</returns>
        public IResolverExpression<T> For<T>()
        {
            // create a IResolverExpression with the values
            return For<T>(_context);
        }

        /// <summary>
        /// Creates a new IResolverExpression for the given type by copying the original map and allows extendig the mapping. This will not affect the stored mapping.
        /// </summary>
        /// <typeparam name="T">The key type of the registered map to</typeparam>
        /// <param name="context">The container containing the map</param>
        /// <returns>IResolverExpression{T}</returns>
        public IResolverExpression<T> For<T>(IMappingContext context)
        {
            // create a IResolverExpression with the values
            using (var resolver = ResolverFactory.GetResolver<T>(context))
            {
                var map = resolver.GetComponent<T>();
                if (map == null)
                    throw new ResolverException(typeof(T));

                // map to new context so that the original mapping will not be affected
                var copyContext = new MappingContext(resolver.Context);
                return map.CreateResolverExpression<T>(copyContext);
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
                    IsDisposed = true;
                    GC.SuppressFinalize(this);
                }
            }
        }

        /// <summary>
        /// Releases resources before the object is reclaimed by garbage collection.
        /// </summary>
        ~InjectionResolver()
        {
            Dispose(false);
        }

        #endregion
    }
}
