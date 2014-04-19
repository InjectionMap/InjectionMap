using System;
using System.Linq.Expressions;

namespace WickedFlame.InjectionMap
{
    public interface IMappingContainer
    {
        IMappingExpression Map<TSvc>();

        IBindingExpression<TImpl> Map<TSvc, TImpl>() where TImpl : TSvc;//, new();
    }
}
