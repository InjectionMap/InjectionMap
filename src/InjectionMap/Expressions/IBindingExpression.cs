using System;
using System.Linq.Expressions;

namespace InjectionMap.Expressions
{
    /// <summary>
    /// Represents a expression that containes a key and a mapped type or object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBindingExpression<T>
    {
        /// <summary>
        /// Adds an argument that will be passed as a parameter
        /// </summary>
        /// <typeparam name="TArg">Type of parameter</typeparam>
        /// <param name="value">Value that will be passed to the parameter</param>
        /// <returns><see cref="IBindingExpression{T}"/></returns>
        IBindingExpression<T> WithArgument<TArg>(TArg value);

        /// <summary>
        /// Adds an argument that will be passed to the named parameter
        /// </summary>
        /// <typeparam name="TArg">Type of parameter</typeparam>
        /// <param name="name">Name of the parameter</param>
        /// <param name="value">Value that will be passed to the parameter</param>
        /// <returns><see cref="IBindingExpression{T}"/></returns>
        IBindingExpression<T> WithArgument<TArg>(string name, TArg value);

        /// <summary>
        /// Adds an argument that will be passed as a parameter
        /// </summary>
        /// <typeparam name="TArg">Type of parameter</typeparam>
        /// <param name="predicate">Value that will be passed to the parameter</param>
        /// <returns><see cref="IBindingExpression{T}"/></returns>
        IBindingExpression<T> WithArgument<TArg>(Expression<Func<TArg>> predicate);

        /// <summary>
        /// Adds an argument that will be passed to the named parameter
        /// </summary>
        /// <typeparam name="TArg">Type of parameter</typeparam>
        /// <param name="name">Name of the parameter</param>
        /// <param name="predicate">Value that will be passed to the parameter</param>
        /// <returns><see cref="IBindingExpression{T}"/></returns>
        IBindingExpression<T> WithArgument<TArg>(string name, Expression<Func<TArg>> predicate);

        /// <summary>
        /// Appends a instanitated object to the mapping of the same type as the mapped type
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IBindingExpression<T> As(Expression<Func<T>> predicate);

        /// <summary>
        /// Provides the ability to define configurations  
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        IBoundExpression<T> WithConfiguration(InjectionFlags option);

        /// <summary>
        /// Maps a expression that gets executed when the component has been resolved
        /// </summary>
        /// <param name="callback">The implementing type of the substitue</param>
        /// <returns>New IBindingExpression with the substitute</returns>
        IBindingExpression<T> OnResolved(Action<T> callback);

        /// <summary>
        /// Defines the constructor that has to be used when resolving.
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        IBindingExpression<T> WithConstructor(Func<ConstructorCollection, ConstructorDefinition> selector);

        /// <summary>
        /// Instructs InjectionMap to inject a property when resolving
        /// </summary>
        /// <param name="property">The property that will be injected</param>
        /// <returns>A bindingexpression containing the mapping definition</returns>
        IBindingExpression<T> InjectProperty(Expression<Func<T, object>> property);
    }
}
