using System;
using System.Linq;
using System.Linq.Expressions;
using InjectionMap.Composition;
using InjectionMap.Extensions;
using System.Collections.Generic;
using System.Text;
using InjectionMap.Tracing;

namespace InjectionMap.Internal
{
    /// <summary>
    /// The ResolverExpression gets called when resolving existing maps. With this Expression the map can be extended with further arguments
    /// </summary>
    /// <typeparam name="T">Key type</typeparam>
    internal class ResolverExpression<T> : ComponentExpression, IResolverExpression<T>, IComponentExpression
    {
        internal Lazy<ILoggerFactory> LoggerFactory { get; private set; }

        internal ILogger Logger
        {
            get
            {
                return LoggerFactory.Value.CreateLogger();
            }
        }

        //IComponentCollection IComponentExpression.Context
        //{
        //    get
        //    {
        //        return _context;
        //    }
        //}

        //readonly IResolverComponentProvider _context;
        //public IResolverComponentProvider Context
        //{
        //    get
        //    {
        //        return _context;
        //    }
        //}

        public ResolverExpression(IComponentCollection context, IMappingComponent component)
            : base(context, component)
        {
            LoggerFactory = new Lazy<ILoggerFactory>(() => new LoggerFactory());
            //_context = context;
        }

        /// <summary>
        /// Adds an argument that will be passed as a parameter
        /// </summary>
        /// <typeparam name="TArg">Type of parameter</typeparam>
        /// <param name="value">Value that will be passed to the parameter</param>
        /// <returns><see cref="IResolverExpression{T}"/></returns>
        public IResolverExpression<T> WithArgument<TArg>(TArg value)
        {
            return AddArgument<TArg>(null, value, null);
        }

        /// <summary>
        /// Adds an argument that will be passed to the named parameter
        /// </summary>
        /// <typeparam name="TArg">Type of parameter</typeparam>
        /// <param name="name">Name of the parameter</param>
        /// <param name="value">Value that will be passed to the parameter</param>
        /// <returns><see cref="IResolverExpression{T}"/></returns>
        public IResolverExpression<T> WithArgument<TArg>(string name, TArg value)
        {
            return AddArgument<TArg>(name, value, null);
        }

        /// <summary>
        /// Adds an argument that will be passed as a parameter
        /// </summary>
        /// <typeparam name="TArg">Type of parameter</typeparam>
        /// <param name="predicate">Value that will be passed to the parameter</param>
        /// <returns><see cref="IResolverExpression{T}"/></returns>
        public IResolverExpression<T> WithArgument<TArg>(Expression<Func<TArg>> predicate)
        {
            return AddArgument<TArg>(null, default(TArg), predicate);
        }

        /// <summary>
        /// Adds an argument that will be passed to the named parameter
        /// </summary>
        /// <typeparam name="TArg">Type of parameter</typeparam>
        /// <param name="name">Name of the parameter</param>
        /// <param name="predicate">Value that will be passed to the parameter</param>
        /// <returns><see cref="IResolverExpression{T}"/></returns>
        public IResolverExpression<T> WithArgument<TArg>(string name, Expression<Func<TArg>> predicate)
        {
            return AddArgument<TArg>(name, default(TArg), predicate);
        }

        /// <summary>
        /// Defines the constructor that has to be used when resolving.
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public IResolverExpression<T> WithConstructor(Func<ConstructorCollection, ConstructorDefinition> selector)
        {
            // get all constructors of the mapped type
            var constructors = Component.ValueType.GetConststructorCollection();

            // get the selected constructor from the map
            var definition = selector.Invoke(constructors);
            foreach (var item in definition.Where(c => c.Value != null))
            {
                // add all arguments that were added in the mapping
                // this means that the value was set in the selector
                AddArgument(item.Name, item.Value, null);
            }
            
            // create a copy of the component because the ConstructorDefinition has no setter in the interface
            var component = Component.CopyComponent();
            Context.AddOrReplace(component);

            // mark the constructor to be selected
            component.ConstructorDefinition = definition;

            return new ResolverExpression<T>(Context, component);
        }

        /// <summary>
        /// Defines the constructor based on the type of arguments that are contained as parameters
        /// </summary>
        /// <param name="parameterTypes">The types of the parameters contained in the desired constructor</param>
        /// <returns></returns>
        public IResolverExpression<T> WithConstructor(params Type[] parameterTypes)
        {
            IConstructorDefinition definition = null;

            // try to find the constructor that matches the parameter types
            var constructors = Component.ValueType.GetConststructorCollection();
            foreach (var ctor in constructors)
            {
                var ctorParameters = ctor.ConstructorInfo.GetParameters();
                if (ctorParameters.Count() != parameterTypes.Where(prm => prm != typeof(void)).Count())
                    continue;

                definition = ctor;

                // check if parameters in corect order
                // create a copy
                var parameterTypeCopy = parameterTypes.ToList();
                foreach (var ctorParam in ctorParameters)
                {
                    var tmpParam = parameterTypeCopy.FirstOrDefault();
                    if (tmpParam == null || tmpParam != ctorParam.ParameterType)
                    {
                        definition = null;
                        break;
                    }

                    parameterTypeCopy.RemoveAt(0);
                }

                if (!parameterTypeCopy.Any())
                {
                    // all parameters were matched in the correct order
                    break;
                }

                definition = ctor;

                // check if parameters are in incorect order
                // create a copy
                parameterTypeCopy = parameterTypes.ToList();
                foreach (var ctorParam in ctorParameters)
                {
                    var tmpParam = parameterTypeCopy.FirstOrDefault(p => p == ctorParam.ParameterType);
                    if (tmpParam == null)
                    {
                        definition = null;
                        break;
                    }

                    parameterTypeCopy.Remove(tmpParam);
                }

                if (!parameterTypeCopy.Any())
                {
                    // all parameters were matched in the correct order
                    break;
                }
            }

            if (definition == null)
            {
                var sb = new StringBuilder();
                sb.AppendLine("Could not find a constructor that can be accessed based on the arguments passed");
                foreach (var param in parameterTypes)
                {
                    sb.AppendLine(string.Format("Parameter type: {0}", param.Name));
                }

                Logger.Write(string.Format("InjectionMap - {0}", sb.ToString()), LogLevel.Error, "ResolverExpression", "Resolver", DateTime.Now);
                throw new InvalidConstructorException(sb.ToString());
            }

            // create a copy of the component because the ConstructorDefinition has no setter in the interface
            var component = Component.CopyComponent();
            Context.AddOrReplace(component);

            // mark the constructor to be selected
            component.ConstructorDefinition = definition;

            return new ResolverExpression<T>(Context, component);
        }

        /// <summary>
        /// Instructs InjectionMap to inject a property when resolving
        /// </summary>
        /// <param name="property">The property that will be injected</param>
        /// <returns>A resolverexpression containing the mapping definition</returns>
        public IResolverExpression<T> InjectProperty(Expression<Func<T, object>> property)
        {
            var factory = new TypeDefinitionFactory();

            // extract propertyinfo
            var info = factory.ExtractProperty(property);
            var setter = factory.GetPropertySetter(info);

            var definition = new PropertyDefinition
            {
                KeyType = info.PropertyType,
                Property = info,
                Setter = setter
            };

            var component = Component.CopyComponent();
            component.Properies.Add(definition);
            Context.AddOrReplace(component);

            return new ResolverExpression<T>(Context, component);
        }

        /// <summary>
        /// Resolves the map
        /// </summary>
        /// <returns>The resolved value</returns>
        public T Resolve()
        {
            return (T)CompositionService.Compose(Component, Context as IComponentProvider);
        }

        #region Private Implementation

        private IResolverExpression<T> AddArgument<TArg>(string name, TArg value, Expression<Func<TArg>> predicate)
        {
            if (!string.IsNullOrEmpty(name) && Component.Arguments.Any(a => a.Name == name))
                Component.Arguments.Remove(Component.Arguments.First(a => a.Name == name));

            Component.Arguments.Add(new BindingArgument<TArg>
            {
                Name = name,
                Value = value,
                Callback = predicate
            });

            return new ResolverExpression<T>(Context, Component);
        }

        #endregion
    }

    /// <summary>
    /// The MultiResolverExpression gets called when resolving multiple existing maps. With this Expression all mappings can be extended with further arguments
    /// </summary>
    /// <typeparam name="T">Key type</typeparam>
    internal class MultiResolverExpression<T> : IMultiResolverExpression<T>
    {
        public MultiResolverExpression(IComponentCollection context)
        {
            context.EnsureArgumentNotNull("context");

            _context = context;
        }

        readonly IComponentCollection _context;
        public IComponentCollection Context
        {
            get
            {
                return _context;
            }
        }
        
        /// <summary>
        /// Adds an argument that will be passed as a parameter
        /// </summary>
        /// <typeparam name="TArg">Type of parameter</typeparam>
        /// <param name="value">Value that will be passed to the parameter</param>
        /// <returns><see cref="IResolverExpression{T}"/></returns>
        public IMultiResolverExpression<T> WithArgument<TArg>(TArg value)
        {
            return AddArgument<TArg>(null, value, null);
        }

        /// <summary>
        /// Adds an argument that will be passed to the named parameter
        /// </summary>
        /// <typeparam name="TArg">Type of parameter</typeparam>
        /// <param name="name">Name of the parameter</param>
        /// <param name="value">Value that will be passed to the parameter</param>
        /// <returns><see cref="IResolverExpression{T}"/></returns>
        public IMultiResolverExpression<T> WithArgument<TArg>(string name, TArg value)
        {
            return AddArgument<TArg>(name, value, null);
        }

        /// <summary>
        /// Adds an argument that will be passed as a parameter
        /// </summary>
        /// <typeparam name="TArg">Type of parameter</typeparam>
        /// <param name="predicate">Value that will be passed to the parameter</param>
        /// <returns><see cref="IResolverExpression{T}"/></returns>
        public IMultiResolverExpression<T> WithArgument<TArg>(Expression<Func<TArg>> predicate)
        {
            return AddArgument<TArg>(null, default(TArg), predicate);
        }

        /// <summary>
        /// Adds an argument that will be passed to the named parameter
        /// </summary>
        /// <typeparam name="TArg">Type of parameter</typeparam>
        /// <param name="name">Name of the parameter</param>
        /// <param name="predicate">Value that will be passed to the parameter</param>
        /// <returns><see cref="IResolverExpression{T}"/></returns>
        public IMultiResolverExpression<T> WithArgument<TArg>(string name, Expression<Func<TArg>> predicate)
        {
            return AddArgument<TArg>(name, default(TArg), predicate);
        }

        /// <summary>
        /// Resolves the map
        /// </summary>
        /// <returns>The resolved value</returns>
        public T Resolve()
        {
            //return CompositionService.Compose<T>();
            using (var resolver = ResolverFactory.GetResolver<T>(Context as IComponentProvider))
            {
                return resolver.Get<T>();
            }
        }

        /// <summary>
        /// Resolves all mappings of type T
        /// </summary>
        /// <returns>All mappings of T</returns>
        public IEnumerable<T> ResolveMultiple()
        {
            //return CompositionService.Compose<T>();
            using (var resolver = ResolverFactory.GetResolver<T>(Context as IComponentProvider))
            {
                return resolver.GetAll<T>();
            }
        }

        #region Private Implementation

        private IMultiResolverExpression<T> AddArgument<TArg>(string name, TArg value, Expression<Func<TArg>> predicate)
        {
            foreach (var component in Context.Get<T>())
            {
                if (!string.IsNullOrEmpty(name) && component.Arguments.Any(a => a.Name == name))
                    component.Arguments.Remove(component.Arguments.First(a => a.Name == name));

                component.Arguments.Add(new BindingArgument<TArg>
                {
                    Name = name,
                    Value = value,
                    Callback = predicate
                });
            }

            return new MultiResolverExpression<T>(Context);
        }

        #endregion
    }
}
