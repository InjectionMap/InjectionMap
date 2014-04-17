using System;
using System.Linq.Expressions;

namespace WickedFlame.InjectionMap
{
    public interface IMappingContainer
    {
        IMappingExpression Map<TSvc>();

        //IInjectionExpression Map<TSvc>(Expression<Func<IInjectionExpression, IInjectionExpression>> action);

        IMappingExpression Map<TSvc, TImpl>() where TImpl : TSvc, new();

        //IInjectionExpression Map<TSvc, TImpl>(Expression<Func<IInjectionExpression, IInjectionExpression>> action) where TImpl : TSvc, new();
    }
}
