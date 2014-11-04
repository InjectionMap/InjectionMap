using System;
using System.Linq.Expressions;

namespace InjectionMap.Expressions
{
    /// <summary>
    /// Represents a expression for a mapping that containes a mapping key but does not containe a mapped type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMappingExpression<T>
    {
        /// <summary>
        /// Creates a mapping to the type TMap that gets composed when resolving
        /// </summary>
        /// <typeparam name="TMap">The mapped type</typeparam>
        /// <returns>A IBindingExpression of TMap</returns>
        IBindingExpression<TMap> For<TMap>();

        /// <summary>
        /// Creates a mapping to the value passed as parameter
        /// </summary>
        /// <typeparam name="TMap">The mapped type</typeparam>
        /// <param name="value">The value that gets mapped</param>
        /// <returns>A IBindingExpression of TMap</returns>
        IBindingExpression<TMap> For<TMap>(TMap value);

        /// <summary>
        /// Creates a mapping to the return value of the expression
        /// </summary>
        /// <typeparam name="TMap">The mapped type</typeparam>
        /// <param name="predicate">Expression that gets called to return the value for the mapping. The expression gets called everytime the mapping is resolved</param>
        /// <returns>A IBindingExpression of TMap</returns>
        IBindingExpression<TMap> For<TMap>(Expression<Func<TMap>> predicate);

        /// <summary>
        /// Creates a binding to the key type
        /// </summary>
        /// <returns>A IBindingExpression of T</returns>
        IBindingExpression<T> ToSelf();

        /// <summary>
        /// Maps a expression that gets executed when the component has been resolved
        /// </summary>
        /// <param name="callback">The Action to execute after resolving</param>
        /// <returns>A IMappingExpression of T</returns>
        IMappingExpression<T> OnResolved(Action<T> callback);


        /// <summary>
        /// Adds a substitute mapping for the mapping
        /// </summary>
        /// <typeparam name="TMap">The implementing type of the substitue</typeparam>
        /// <returns>New IBindingExpression with the substitute</returns>
        IBindingExpression<TMap> Substitute<TMap>();

        /// <summary>
        /// Adds a substitute mapping for the mapping
        /// </summary>
        /// <typeparam name="T">Key type to create a substitute for</typeparam>
        /// <param name="predicate">Callback expression to generate substitute</param>
        /// <returns>New IBindingExpression with the substitute</returns>
        IBindingExpression<T> Substitute(Expression<Func<T>> predicate);
    }
}
