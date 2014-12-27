﻿using System;
using System.Collections.Generic;

namespace InjectionMap.Internal
{
    interface IResolver : IDisposable
    {
        IComponentProvider Context { get; }

        /// <summary>
        /// Gets the first occurance of T
        /// </summary>
        /// <typeparam name="T">The mapped type</typeparam>
        /// <returns>The first occurance of T</returns>
        T Get<T>();

        /// <summary>
        /// Gets the first occurance of the type
        /// </summary>
        /// <param name="type">The mapped type</param>
        /// <returns>The first occurance of the mapped type</returns>
        object Get(Type type);

        /// <summary>
        /// Gets all accurances of T
        /// </summary>
        /// <typeparam name="T">The mapped type</typeparam>
        /// <returns>All T</returns>
        IEnumerable<T> GetAll<T>();

        /// <summary>
        /// Gets the IMappingComponent of the first occurance of T
        /// </summary>
        /// <typeparam name="T">The mapped type</typeparam>
        /// <returns>The first IMappingComponent of T</returns>
        IMappingComponent GetComponent<T>();

        /// <summary>
        /// Gets all IMappingComponents mapped to T
        /// </summary>
        /// <typeparam name="T">The mapped type</typeparam>
        /// <returns>All IMappingComonents of T</returns>
        IEnumerable<IMappingComponent> GetAllComponents<T>();
    }
}
