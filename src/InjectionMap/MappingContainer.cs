﻿using System;
using System.Collections.Generic;
using System.Linq;
using InjectionMap.Expressions;
using InjectionMap.Mapping;

namespace InjectionMap
{
    public class MappingContainer : IMappableContainer, IComponentCollection, IComponentProvider, IMappingProvider
    {
        #region IComponentContainer Implementation

        IList<IMappingComponent> _components;
        internal IList<IMappingComponent> Components
        {
            get
            {
                if (_components == null)
                    _components = new List<IMappingComponent>();
                return _components;
            }
        }

        public void AddOrReplace(IMappingComponent component)
        {
            var remove = Components.FirstOrDefault(c => c.ID == component.ID);
            if (Components.Contains(remove))
            {
                Components.Remove(remove);
            }

            Components.Add(component);
        }

        public void Add(IMappingComponent component)
        {
            Components.Add(component);
        }

        public void ReplaceAll(IMappingComponent component)
        {
            var lst = Components.Where(c => c.KeyType == component.KeyType).ToList();
            foreach (var comp in lst)
                Components.Remove(comp);

            Components.Add(component);
        }

        public void Remove(IMappingComponent component)
        {
            if (Components.Contains(component))
                Components.Remove(component);
        }

        #endregion

        #region IComponentProvider Implementation

        public IEnumerable<IMappingComponent> Get<T>()
        {
            var lst = Components.Where(c => c.KeyType == typeof(T));
            
            // check if a component was added as substitute
            if (lst.Any(c => c.IsSubstitute))
                return lst.Where(c => c.IsSubstitute);

            // return all components
            return lst;
        }

        public IEnumerable<IMappingComponent> Get(Type type)
        {
            var components = Components.Where(c => c.KeyType == type);

            // check if a component was added as substitute
            if (components.Any(c => c.IsSubstitute))
                return components.Where(c => c.IsSubstitute);

            // return all components
            return components;
        }
        
        #endregion

        #region IMappingContainer Implementation

        public IMappingExpression<TKey> Map<TKey>()
        {
            var expression = MappingContainer.MapInternal<TKey>(this);

            return expression;
        }

        public IBindingExpression<TMap> Map<TKey, TMap>() where TMap : TKey
        {
            var expression = MappingContainer.MapInternal<TKey>(this);

            return expression.For<TMap>();
        }

        /// <summary>
        /// Removes all mappings of type T
        /// </summary>
        /// <typeparam name="T">The type of mappings to remove</typeparam>
        public void Clean<T>()
        {
            var items = Components.Where(c => c.KeyType == typeof(T)).ToList();
            foreach (var item in items)
            {
                Remove(item);
            }
        }

        #endregion

        #region Internal Implementation

        private static IMappingExpression<T> MapInternal<T>(IComponentCollection container)
        {
            var component = new MappingComponent<T>
            {
                KeyType = typeof(T)
            };

            container.AddOrReplace(component);

            return new MappingExpression<T>(container, component);
        }

        #endregion
    }
}