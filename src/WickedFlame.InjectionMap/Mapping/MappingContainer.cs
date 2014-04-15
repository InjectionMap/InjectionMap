using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WickedFlame.InjectionMap.Composition;

namespace WickedFlame.InjectionMap.Mapping
{
    internal class MappingContainer : IMappingContainer, IComponentContainer
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
            var lst = Components.Where(c => c.Key == component.Key).ToList();
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

        #region IMappingContainer Implementation

        public IInjectionExpression Map<TSvc>()
        {
            if (!typeof(TSvc).IsInterface)
                throw new NotSupportedException("Key Type has to be an interface");

            var expression = MappingContainer.MapInternal<TSvc>(this);

            if (expression.Component.MappingOption != null && expression.Component.MappingOption.ResolveInstanceOnMapping)
                expression = expression.For(CompositionService.Compose(expression.Component));

            return expression;
        }

        //public IInjectionExpression Map<TSvc>(Expression<Func<IInjectionExpression, IInjectionExpression>> action)
        //{
        //    if (!typeof(TSvc).IsInterface)
        //        throw new NotSupportedException("Key Type has to be an interface");

        //    var expression = MappingContainer.MapInternal<TSvc>(this);

        //    if (action != null)
        //        expression = action.Compile().Invoke(expression);

        //    if (expression.Component.MappingOption != null && expression.Component.MappingOption.ResolveInstanceOnMapping)
        //        expression = expression.For(Resolver.Resolve(expression.Component));

        //    return expression;
        //}

        public IInjectionExpression Map<TSvc, TImpl>() where TImpl : TSvc, new()
        {
            if (!typeof(TSvc).IsInterface)
                throw new NotSupportedException("Key Type has to be an interface");

            var expression = MappingContainer.MapInternal<TSvc>(this).For<TImpl>();

            if (expression.Component.MappingOption != null && expression.Component.MappingOption.ResolveInstanceOnMapping)
                expression = expression.For(CompositionService.Compose(expression.Component));

            return expression;
        }

        //public IInjectionExpression Map<TSvc, TImpl>(Expression<Func<IInjectionExpression, IInjectionExpression>> action) where TImpl : TSvc, new()
        //{
        //    if (!typeof(TSvc).IsInterface)
        //        throw new NotSupportedException("Key Type has to be an interface");

        //    var expression = MappingContainer.MapInternal<TSvc>(this).For<TImpl>();

        //    if (action != null)
        //        expression = action.Compile().Invoke(expression);

        //    if (expression.Component.MappingOption != null && expression.Component.MappingOption.ResolveInstanceOnMapping)
        //        expression = expression.For(Resolver.Resolve(expression.Component));

        //    return expression;
        //}

        #endregion

        #region Internal Implementation

        internal static IInjectionExpression MapInternal<T>(IComponentContainer container)
        {
            var component = new MappingComponent<T>
            {
                Key = typeof(T)
            };

            container.AddOrReplace(component);

            return new InjectionExpression(container, component);
        }

        #endregion
    }
}
