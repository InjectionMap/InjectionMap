using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WickedFlame.InjectionMap.Substitution
{
    public static class IMappingExpressionExtension
    {
        public static IBindingExpression<T> Substitute<T>(this IMappingExpression expression)
        {
            throw new NotImplementedException();
        }

        public static IBindingExpression<T> Substitute<T>(this IMappingExpression expression, Expression<Func<T>> callback)
        {
            throw new NotImplementedException();
        }
    }
}
