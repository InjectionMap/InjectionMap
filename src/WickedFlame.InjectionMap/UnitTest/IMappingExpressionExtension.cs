using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WickedFlame.InjectionMap.UnitTest
{
    public static class IMappingExpressionExtension
    {
        public static IBindingExpression<T> Substitute<T, T2>(this IMappingExpression expression)
        {
            throw new NotImplementedException();
        }
    }
}
