﻿using System;
using System.Linq.Expressions;

namespace WickedFlame.InjectionMap.Mapping
{
    internal class BindingExpression<T> : BindableComponent, IBindingExpression<T>
    {
        public BindingExpression(IComponentContainer container, IMappingComponent component)
            : base(container, component)
        {
        }

        public IBindingExpression<T> WithArgument<TArg>(TArg value)
        {
            return AddArgument<TArg>(null, value, null);
        }

        public IBindingExpression<T> WithArgument<TArg>(string name, TArg value)
        {
            return AddArgument<TArg>(name, value, null);
        }

        public IBindingExpression<T> WithArgument<TArg>(Expression<Func<TArg>> argument)
        {
            return AddArgument<TArg>(null, default(TArg), argument);
        }

        public IBindingExpression<T> WithArgument<TArg>(string name, Expression<Func<TArg>> argument)
        {
            return AddArgument<TArg>(name, default(TArg), argument);
        }

        public IBindingExpression<T> As<T>(Expression<Func<T>> callback)
        {
            return new MappingExpression<T>(Container, Component).For<T>(callback);
        }

        public IBoundExpression WithOptions(InjectionFlags option)
        {
            var resolveInstanceOnMapping = (option & InjectionFlags.ResolveInstanceOnMapping) == InjectionFlags.ResolveInstanceOnMapping;
            var keepInstance = (option & InjectionFlags.KeepInstance) == InjectionFlags.KeepInstance;
            var withOverwrite = (option & InjectionFlags.WithOverwrite) == InjectionFlags.WithOverwrite;

            if (resolveInstanceOnMapping && !keepInstance)
                keepInstance = true;

            // remove previous instances...
            Container.AddOrReplace(Component);
            if (withOverwrite)
            {
                Container.ReplaceAll(Component);
            }

            return new BoundExpression(Container, Component)
            {
                MappingOption = new MappingOption
                {
                    ResolveInstanceOnMapping = resolveInstanceOnMapping,
                    KeepInstance = keepInstance,
                    WithoutOverwrite = !withOverwrite
                }
            };
        }

        #region Private Implementation

        private IBindingExpression<T> AddArgument<TArg>(string name, TArg value, Expression<Func<TArg>> callback)
        {
            Component.Arguments.Add(new BindingArgument<TArg>
            {
                Name = name,
                Value = value,
                Callback = callback
            });

            return new BindingExpression<T>(Container, Component);
        }

        #endregion
    }
}