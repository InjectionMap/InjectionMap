﻿using System;
using System.Linq.Expressions;

namespace WickedFlame.InjectionMap
{
    public interface IInjectionExpression
    {
        IComponentContainer ComponentContainer { get; }

        IMappingComponent Component { get; }

        IBindingExpression<T> For<T>() where T : new();

        //IInjectionExpression For<T>(Expression<Func<IOptionExpression, IOptionExpression>> options) where T : new();

        IBindingExpression<T> For<T>(T value);

        IBindingExpression<T> For<T>(Expression<Func<T>> valueCallback);

        //IInjectionExpression For<T>(T value, Expression<Func<IOptionExpression, IOptionExpression>> options);

        //IInjectionExpression For<T>(Expression<Func<T>> valueCallback, Expression<Func<IOptionExpression, IOptionExpression>> options);

        //IInjectionExpression WithOptions(InjectionOption option);
    }
}
