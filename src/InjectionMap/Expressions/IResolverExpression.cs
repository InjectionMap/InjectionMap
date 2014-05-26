using System;
using System.Linq.Expressions;

namespace InjectionMap.Expressions
{
    /// <summary>
    /// The ResolverExpression gets called when resolving existing maps. With this Expression the map can be extended with further arguments
    /// </summary>
    /// <typeparam name="T">Key type</typeparam>
    public interface IResolverExpression<T> : IComponentExpression
    {
        /// <summary>
        /// Adds an argument that will be passed as a parameter
        /// </summary>
        /// <typeparam name="TArg">Type of parameter</typeparam>
        /// <param name="value">Value that will be passed to the parameter</param>
        /// <returns><see cref="IBindingExpression{T}"/></returns>
        IResolverExpression<T> WithArgument<TArg>(TArg value);

        /// <summary>
        /// Adds an argument that will be passed to the named parameter
        /// </summary>
        /// <typeparam name="TArg">Type of parameter</typeparam>
        /// <param name="name">Name of the parameter</param>
        /// <param name="value">Value that will be passed to the parameter</param>
        /// <returns><see cref="IBindingExpression{T}"/></returns>
        IResolverExpression<T> WithArgument<TArg>(string name, TArg value);

        /// <summary>
        /// Adds an argument that will be passed as a parameter
        /// </summary>
        /// <typeparam name="TArg">Type of parameter</typeparam>
        /// <param name="predicate">Value that will be passed to the parameter</param>
        /// <returns><see cref="IBindingExpression{T}"/></returns>
        IResolverExpression<T> WithArgument<TArg>(Expression<Func<TArg>> predicate);

        /// <summary>
        /// Adds an argument that will be passed to the named parameter
        /// </summary>
        /// <typeparam name="TArg">Type of parameter</typeparam>
        /// <param name="name">Name of the parameter</param>
        /// <param name="predicate">Value that will be passed to the parameter</param>
        /// <returns><see cref="IBindingExpression{T}"/></returns>
        IResolverExpression<T> WithArgument<TArg>(string name, Expression<Func<TArg>> predicate);

        /// <summary>
        /// Resolves the map
        /// </summary>
        /// <returns>The resolved value</returns>
        T Resolve();
    }
}
