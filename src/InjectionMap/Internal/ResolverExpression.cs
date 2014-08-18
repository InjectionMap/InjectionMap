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
        public ResolverExpression(IComponentCollection container, IMappingComponent component)
            : base(container, component)
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

            return new ResolverExpression<T>(Container, Component);
        }

        #endregion
    }

    /// <summary>
    /// The MultiResolverExpression gets called when resolving multiple existing maps. With this Expression all mappings can be extended with further arguments
    /// </summary>
    /// <typeparam name="T">Key type</typeparam>
    internal class MultiResolverExpression<T> : IMultiResolverExpression<T>
    {
        public MultiResolverExpression(IComponentCollection container)
        {
            container.EnsureArgumentNotNull("container");

            _container = container;
        }

        readonly IComponentCollection _container;
        public IComponentCollection Container
        {
            get
            {
                return _container;
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
            using (var resolver = ResolverFactory.GetResolver<T>(Container as IComponentProvider))
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
            using (var resolver = ResolverFactory.GetResolver<T>(Container as IComponentProvider))
            {
                return resolver.GetAll<T>();
            }
        }

        #region Private Implementation

        private IMultiResolverExpression<T> AddArgument<TArg>(string name, TArg value, Expression<Func<TArg>> predicate)
        {
            foreach (var component in Container.Get<T>())
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

            return new MultiResolverExpression<T>(Container);
        }

        #endregion
    }
}
