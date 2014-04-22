using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WickedFlame.InjectionMap.Composition;

namespace WickedFlame.InjectionMap.Mapping
{
    internal class MappingContainer : IMappingContainer, IComponentContainer, IComponentProvider
    {
        #region IComponentContainer Implementation

        IList<IMappingComponent> _components;
        public IList<IMappingComponent> Components
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
            return Components.Where(c => c.KeyType == typeof(T));
        }

        public IEnumerable<IMappingComponent> Get(Type type)
        {
            return Components.Where(c => c.KeyType == type);
        }
        
        #endregion

        #region IMappingContainer Implementation

        public IMappingExpression Map<TSvc>()
        {
            if (!typeof(TSvc).IsInterface)
                throw new NotSupportedException("Key Type has to be an interface");

            var expression = MappingContainer.MapInternal<TSvc>(this);

            return expression;
        }

        public IBindingExpression<TImpl> Map<TSvc, TImpl>() where TImpl : TSvc//, new()
        {
            if (!typeof(TSvc).IsInterface)
                throw new NotSupportedException("Key Type has to be an interface");

            var expression = MappingContainer.MapInternal<TSvc>(this);

            return expression.For<TImpl>();
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

        internal static IMappingExpression MapInternal<T>(IComponentContainer container)
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
