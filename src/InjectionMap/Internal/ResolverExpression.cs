using System;
using System.Linq;
using System.Linq.Expressions;
using InjectionMap.Composition;
using InjectionMap.Expressions;
using InjectionMap.Extensions;
using System.Collections.Generic;

namespace InjectionMap.Internal
{
    /// <summary>
    /// The ResolverExpression gets called when resolving existing maps. With this Expression the map can be extended with further arguments
    /// </summary>
    /// <typeparam name="T">Key type</typeparam>
    internal class ResolverExpression<T> : ComponentExpression, IResolverExpression<T>, IComponentExpression
    {
        public ResolverExpression(IComponentCollection context, IMappingComponent component)
            : base(context, component)
        {
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
                AddArgument(item.Name, item.Value, null);
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

            var component = Component.CreateComponent();
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
            return (T)CompositionService.Compose(Component);
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
