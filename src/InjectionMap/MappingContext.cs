using System;
using System.Collections.Generic;
using System.Linq;
using InjectionMap.Expressions;
using InjectionMap.Internal;
using InjectionMap.Extensions;

namespace InjectionMap
{
    /// <summary>
    /// Represents a distinct context that containes the map definitions. 
    /// A map definition only exists in one context. The context that is mapped to can be provided by the developer/user or by the system. If no context is provided, InjectionMap creates a default context.
    /// There can be multipel contexts in a application.
    /// </summary>
    public class MappingContext : IMappingContext, IComponentCollection, IComponentProvider, IMappingProvider
    {
        public MappingContext()
        {
        }

        public MappingContext(string context)
        {
            _components = MappingContextManager.GetComponents(context).Components;
        }

        #region IComponentContainer Implementation

        IList<IMappingComponent> _components;
        /// <summary>
        /// Gets a collection of components that contain the mapping definitions
        /// </summary>
        internal IList<IMappingComponent> Components
        {
            get
            {
                if (_components == null)
                    _components = new List<IMappingComponent>();
                return _components;
            }
        }

        /// <summary>
        /// Adds a component or replace all existing mappings of components with the same id
        /// </summary>
        /// <param name="component"></param>
        public void AddOrReplace(IMappingComponent component)
        {
            component.EnsureArgumentNotNull("component");

            var remove = Components.FirstOrDefault(c => c.ID == component.ID);
            if (Components.Contains(remove))
                Components.Remove(remove);

            Components.Add(component);
        }

        /// <summary>
        /// Adds a new component
        /// </summary>
        /// <param name="component"></param>
        public void Add(IMappingComponent component)
        {
            component.EnsureArgumentNotNull("component");

            Components.Add(component);
        }

        /// <summary>
        /// Replaces all existing components with this version
        /// </summary>
        /// <param name="component"></param>
        public void ReplaceAll(IMappingComponent component)
        {
            component.EnsureArgumentNotNull("component");

            var lst = Components.Where(c => c.KeyType == component.KeyType).ToList();
            foreach (var comp in lst)
                Components.Remove(comp);

            Components.Add(component);
        }

        /// <summary>
        /// remove the given component
        /// </summary>
        /// <param name="component"></param>
        public void Remove(IMappingComponent component)
        {
            component.EnsureArgumentNotNull("component");

            if (Components.Contains(component))
                Components.Remove(component);
        }

        /// <summary>
        /// Gets all mappings of the type
        /// </summary>
        /// <typeparam name="T">Key type of the mapping</typeparam>
        /// <param name="predicate">The predicate that checks the expression</param>
        /// <returns>A list of all maoppings to the given type</returns>
        public IEnumerable<IMappingComponent> Get<T>(Func<IMappingComponent, bool> predicate)
        {
            var lst = Components.Where(c => c.KeyType == typeof(T) && predicate(c));

            // check if a component was added as substitute
            if (lst.Any(c => c.IsSubstitute))
                return lst.Where(c => c.IsSubstitute);

            // return all components
            return lst;
        }

        #endregion

        #region IComponentProvider Implementation

        /// <summary>
        /// Gets all mappings of the type
        /// </summary>
        /// <typeparam name="T">Key type of the mapping</typeparam>
        /// <returns>A list of all maoppings to the given type</returns>
        public IEnumerable<IMappingComponent> Get<T>()
        {
            var lst = Components.Where(c => c.KeyType == typeof(T));
            
            // check if a component was added as substitute
            if (lst.Any(c => c.IsSubstitute))
                return lst.Where(c => c.IsSubstitute);

            // return all components
            return lst;
        }

        /// <summary>
        /// Gets all mappings of the type
        /// </summary>
        /// <param name="type">Key type of the mapping</param>
        /// <returns>A list of all maoppings to the given type</returns>
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

        /// <summary>
        /// Creates a Mapping to TKey
        /// </summary>
        /// <typeparam name="TKey">The type to map</typeparam>
        /// <returns>The expression for the mapping</returns>
        public IMappingExpression<TKey> Map<TKey>()
        {
            return MappingContext.MapInternal<TKey>(this);
        }

        /// <summary>
        /// Creates a mapping to TKey with TMap
        /// </summary>
        /// <typeparam name="TKey">The key type to map</typeparam>
        /// <typeparam name="TMap">The instance type to map</typeparam>
        /// <returns>The expression for the mapping</returns>
        public IBindingExpression<TMap> Map<TKey, TMap>() where TMap : TKey
        {
            return MappingContext.MapInternal<TKey>(this).For<TMap>();
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
                item.Dispose();
            }
        }

        #endregion

        #region Internal Implementation

        internal static IMappingExpression<T> MapInternal<T>(IComponentCollection context)
        {
            context.EnsureArgumentNotNull("context");

            if (context.Get<T>(m => m.MappingConfiguration.AsSingleton).Any())
            {
                //TODO: It is not jet decided if the Mapping should cast an error when there is existing mapping in singletonscope. It may not be a good idea to just silently throw the new mapping away without notifying the user!

                // create a new mappingcontainer
                // this preserves the original mapping but prevents a null reference when processing further with fluent syntax
                // this mapping will get lost because the container is not stored anywhere!
                context = new MappingContext();
            }

            var component = new MappingComponent<T>
            {
                KeyType = typeof(T)
            };

            context.AddOrReplace(component);

            return new MappingExpression<T>(context, component);
        }

        #endregion
    }
}
