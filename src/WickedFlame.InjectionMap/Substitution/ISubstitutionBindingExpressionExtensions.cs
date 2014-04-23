using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WickedFlame.InjectionMap.Substitution
{
    public static class ISubstitutionBindingExpressionExtensions
    {
        public static IBindingExpression<T> With<T>(this ISubstitutionBindingExpression<T> expression)
        {
            throw new NotImplementedException();
        }

        public static IBindingExpression<T> With<T>(this ISubstitutionBindingExpression<T> expression, Expression<Func<T>> callback)
        {
            throw new NotImplementedException();
        }
    }
}
