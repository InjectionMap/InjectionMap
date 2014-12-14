﻿using System;

namespace InjectionMap
{
    public interface IBoundExpression<T>
    {
        /// <summary>
        /// Gets the configuration for the mapping
        /// </summary>
        IMappingConfiguration MappingConfiguration { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        IBoundExpression<T> OnResolved(Action<T> callback);
    }
}
