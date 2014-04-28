using System;
using System.Linq.Expressions;

namespace InjectionMap.Expressions
{
    public interface IMappingExpression<T> : IComponentExpression
    {
        IBindingExpression<TMap> For<TMap>();

        IBindingExpression<TMap> For<TMap>(TMap value);

        IBindingExpression<TMap> For<TMap>(Expression<Func<TMap>> callback);

        IBindingExpression<T> ToSelf();

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
        /// <param name="callback">Callback expression to generate substitute</param>
        /// <returns>New IBindingExpression with the substitute</returns>
        IBindingExpression<T> Substitute(Expression<Func<T>> callback);
    }
}
