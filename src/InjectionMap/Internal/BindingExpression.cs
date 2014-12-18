using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using InjectionMap.Extensions;
using InjectionMap.Composition;
using InjectionMap.Tracing;

namespace InjectionMap.Internal
{
    internal class BindingExpression<T> : ComponentExpression, IBindingExpression<T>, IComponentExpression
    {
        internal Lazy<ILoggerFactory> LoggerFactory { get; private set; }

        internal ILogger Logger
        {
            get
            {
                return LoggerFactory.Value.CreateLogger();
            }
        }

        public BindingExpression(IComponentCollection container, IMappingComponent component)
            : base(container, component)
        {
            LoggerFactory = new Lazy<ILoggerFactory>(() => new LoggerFactory());
        }

        #region IBindingExpression<T> Implementation

        /// <summary>
        /// Adds an argument that will be passed as a parameter
        /// </summary>
        /// <typeparam name="TArg">Type of parameter</typeparam>
        /// <param name="value">Value that will be passed to the parameter</param>
        /// <returns><see cref="IBindingExpression{T}"/></returns>
        public IBindingExpression<T> WithArgument<TArg>(TArg value)
        {
            return AddArgument<TArg>(null, value, null);
        }

        /// <summary>
        /// Adds an argument that will be passed to the named parameter
        /// </summary>
        /// <typeparam name="TArg">Type of parameter</typeparam>
        /// <param name="name">Name of the parameter</param>
        /// <param name="value">Value that will be passed to the parameter</param>
        /// <returns><see cref="IBindingExpression{T}"/></returns>
        public IBindingExpression<T> WithArgument<TArg>(string name, TArg value)
        {
            return AddArgument<TArg>(name, value, null);
        }

        /// <summary>
        /// Adds an argument that will be passed as a parameter
        /// </summary>
        /// <typeparam name="TArg">Type of parameter</typeparam>
        /// <param name="predicate">Value that will be passed to the parameter</param>
        /// <returns><see cref="IBindingExpression{T}"/></returns>
        public IBindingExpression<T> WithArgument<TArg>(Expression<Func<TArg>> predicate)
        {
            return AddArgument<TArg>(null, default(TArg), predicate);
        }

        /// <summary>
        /// Adds an argument that will be passed to the named parameter
        /// </summary>
        /// <typeparam name="TArg">Type of parameter</typeparam>
        /// <param name="name">Name of the parameter</param>
        /// <param name="predicate">Value that will be passed to the parameter</param>
        /// <returns><see cref="IBindingExpression{T}"/></returns>
        public IBindingExpression<T> WithArgument<TArg>(string name, Expression<Func<TArg>> predicate)
        {
            return AddArgument<TArg>(name, default(TArg), predicate);
        }

        /// <summary>
        /// Appends a instance of the mapped object to the mapping of the same type as the mapped type. This can be used when the mapped type containes multiple constructors.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IBindingExpression<T> As(Expression<Func<T>> predicate)
        {
            return new MappingExpression<T>(Context, Component).For<T>(predicate);
        }

        /// <summary>
        /// Provides the ability to define configurations  
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public IBoundExpression<T> WithConfiguration(InjectionFlags option)
        {
            var resolveValueOnMapping = (option & InjectionFlags.ResolveValueOnMapping) == InjectionFlags.ResolveValueOnMapping;
            var asConstant = (option & InjectionFlags.AsConstant) == InjectionFlags.AsConstant;
            var asSingleton = (option & InjectionFlags.AsSingleton) == InjectionFlags.AsSingleton;

            if (resolveValueOnMapping && !asConstant)
                asConstant = true;

            var component = Component.CreateComponent<T>();
            component.MappingConfiguration = new MappingConfiguration
            {
                ResolveValueOnMapping = resolveValueOnMapping,
                AsConstant = asConstant,
                AsSingleton = asSingleton
            };

            // remove previous instances...
            Context.AddOrReplace(component);
            if (asSingleton)
            {
                Context.ReplaceAll(component);
            }

            if (resolveValueOnMapping)
            {
                var provider = Context as IComponentProvider;
                using (var resolver = new ComponentResolver(provider))
                {
                    var value = resolver.Get(component.KeyType);
                    component.ValueCallback = () => (T)value;
                }
            }

            return new BoundExpression<T>(Context, component)
            {
                MappingConfiguration = component.MappingConfiguration
            };
        }

        /// <summary>
        /// Maps a expression that gets executed when the component has been resolved
        /// </summary>
        /// <param name="callback">The implementing type of the substitue</param>
        /// <returns>New IBindingExpression with the substitute</returns>
        public IBindingExpression<T> OnResolved(Action<T> callback)
        {
            Component.KeyType.EnsureMappingTypeMatches(typeof(T));

            var component = Component.CreateComponent<T>();
            component.OnResolvedCallback = callback;

            return component.CreateBindingExpression<T>(Context);
        }

        /// <summary>
        /// Defines the constructor that has to be used when resolving.
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public IBindingExpression<T> WithConstructor(Func<ConstructorCollection, ConstructorDefinition> selector)
        {
            // get all constructors of the mapped type
            var constructors = Component.ValueType.GetConststructorCollection();

            // get the selected constructor from the map
            var definition = selector.Invoke(constructors);
            foreach (var item in definition.Where(c => c.Value != null))
            {
                // add all arguments that were added in the mapping
                AddArgument(item.Name, item.Value, null);
            }

            // mark the constructor to be selected
            var component = Component.CreateComponent<T>();
            component.ConstructorDefinition = definition;
            Context.AddOrReplace(component);

            return new BindingExpression<T>(Context, component);
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
            if (parameterTypes.Any())
            {
                var constructors = Component.ValueType.GetConststructorCollection();
                foreach (var ctor in constructors)
                {
                    var ctorParameters = ctor.ConstructorInfo.GetParameters();
                    if (ctorParameters.Count() != parameterTypes.Count())
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
                    parameterTypeCopy = parameterTypes/*.Select(p => p.Invoke())*/.ToList();
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
            }

            // create a copy of the component because the ConstructorDefinition has no setter in the interface
            var component = Component.CreateComponent();
            Context.AddOrReplace(component);

            // mark the constructor to be selected
            component.ConstructorDefinition = definition;

            return new ResolverExpression<T>(Context, component);
        }

        /// <summary>
        /// Instructs InjectionMap to inject a property when resolving
        /// </summary>
        /// <param name="property">The property that will be injected</param>
        /// <returns>A bindingexpression containing the mapping definition</returns>
        public IBindingExpression<T> InjectProperty(Expression<Func<T, object>> property)
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

            var component = Component.CreateComponent<T>();
            component.Properies.Add(definition);
            Context.AddOrReplace(component);

            return new BindingExpression<T>(Context, component);
        }


        #endregion

        #region Private Implementation

        private IBindingExpression<T> AddArgument<TArg>(string name, TArg value, Expression<Func<TArg>> predicate)
        {
            Component.Arguments.Add(new BindingArgument<TArg>
            {
                Name = name,
                Value = value,
                Callback = predicate
            });

            return new BindingExpression<T>(Context, Component);
        }

        #endregion
    }
}
