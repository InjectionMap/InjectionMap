using System;
using System.Linq.Expressions;

namespace InjectionMap.Expressions
{
    public interface IMappingExpression<T> : IComponentExpression
    {
        IBindingExpression<TImpl> For<TImpl>();

        IBindingExpression<TImpl> For<TImpl>(TImpl value);

        IBindingExpression<TImpl> For<TImpl>(Expression<Func<TImpl>> callback);

        IBindingExpression<T> ToSelf();

        IMappingExpression<T> OnResolved(Action<T> callback);


        /// <summary>
        /// Adds a substitute mapping for the mapping
        /// </summary>
        /// <typeparam name="TImpl">The implementing type of the substitue</typeparam>
        /// <returns>New IBindingExpression with the substitute</returns>
        IBindingExpression<TImpl> Substitute<TImpl>();

        /// <summary>
        /// Adds a substitute mapping for the mapping
        /// </summary>
        /// <typeparam name="T">Key type to create a substitute for</typeparam>
        /// <param name="callback">Callback expression to generate substitute</param>
        /// <returns>New IBindingExpression with the substitute</returns>
        IBindingExpression<T> Substitute(Expression<Func<T>> callback);
    }
}
