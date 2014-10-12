using System;
using System.Linq;
using System.Linq.Expressions;
using InjectionMap.Expressions;
using InjectionMap.Extensions;

namespace InjectionMap.Internal
{
    internal class BindingExpression<T> : ComponentExpression, IBindingExpression<T>, IComponentExpression
    {
        public BindingExpression(IComponentCollection container, IMappingComponent component)
            : base(container, component)
        {
        }

        #region IBindingExpression<T> Implementation

        /// <summary>
        /// Adds an argument that will be passed as a parameter
        /// </summary>
        /// <typeparam name="TArg">Type of parameter</typeparam>
        /// <param name="value">Value that will be passed to the parameter</param>
        /// <returns><see cref="IBindingExpression{T}"/></returns>
        public IBindingExpression<T> WithArgument<TArg>(TArg value)
        {
            return AddArgument<TArg>(null, value, null);
        }

        /// <summary>
        /// Adds an argument that will be passed to the named parameter
        /// </summary>
        /// <typeparam name="TArg">Type of parameter</typeparam>
        /// <param name="name">Name of the parameter</param>
        /// <param name="value">Value that will be passed to the parameter</param>
        /// <returns><see cref="IBindingExpression{T}"/></returns>
        public IBindingExpression<T> WithArgument<TArg>(string name, TArg value)
        {
            return AddArgument<TArg>(name, value, null);
        }

        /// <summary>
        /// Adds an argument that will be passed as a parameter
        /// </summary>
        /// <typeparam name="TArg">Type of parameter</typeparam>
        /// <param name="predicate">Value that will be passed to the parameter</param>
        /// <returns><see cref="IBindingExpression{T}"/></returns>
        public IBindingExpression<T> WithArgument<TArg>(Expression<Func<TArg>> predicate)
        {
            return AddArgument<TArg>(null, default(TArg), predicate);
        }

        /// <summary>
        /// Adds an argument that will be passed to the named parameter
        /// </summary>
        /// <typeparam name="TArg">Type of parameter</typeparam>
        /// <param name="name">Name of the parameter</param>
        /// <param name="predicate">Value that will be passed to the parameter</param>
        /// <returns><see cref="IBindingExpression{T}"/></returns>
        public IBindingExpression<T> WithArgument<TArg>(string name, Expression<Func<TArg>> predicate)
        {
            return AddArgument<TArg>(name, default(TArg), predicate);
        }

        public IBindingExpression<T> As(Expression<Func<T>> predicate)
        {
            return new MappingExpression<T>(Container, Component).For<T>(predicate);
        }

        public IBoundExpression<T> WithConfiguration(InjectionFlags option)
        {
            var resolveValueOnMapping = (option & InjectionFlags.ResolveValueOnMapping) == InjectionFlags.ResolveValueOnMapping;
            var asConstant = (option & InjectionFlags.AsConstant) == InjectionFlags.AsConstant;
            var asSingleton = (option & InjectionFlags.AsSingleton) == InjectionFlags.AsSingleton;

            if (resolveValueOnMapping && !asConstant)
                asConstant = true;

            var component = Component.CreateComponent<T>();
            component.MappingConfiguration = new MappingConfiguration
            {
                ResolveValueOnMapping = resolveValueOnMapping,
                AsConstant = asConstant,
                AsSingleton = asSingleton
            };

            // remove previous instances...
            Container.AddOrReplace(component);
            if (asSingleton)
            {
                Container.ReplaceAll(component);
            }

            if (resolveValueOnMapping)
            {
                var provider = Container as IComponentProvider;
                using (var resolver = new ComponentResolver(provider))
                {
                    var value = resolver.Get(component.KeyType);
                    component.ValueCallback = () => (T)value;
                }
            }

            return new BoundExpression<T>(Container, component)
            {
                MappingConfiguration = component.MappingConfiguration
            };
        }

        /// <summary>
        /// Maps a expression that gets executed when the component has been resolved
        /// </summary>
        /// <param name="callback">The implementing type of the substitue</param>
        /// <returns>New IBindingExpression with the substitute</returns>
        public IBindingExpression<T> OnResolved(Action<T> callback)
        {
            Component.KeyType.EnsureMappingTypeMatches(typeof(T));

            var component = Component.CreateComponent<T>();
            component.OnResolvedCallback = callback;

            return component.CreateBindingExpression<T>(Container);
        }

        /// <summary>
        /// Defines the constructor that has to be used when resolving.
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public IBindingExpression<T> ForConstructor(Func<ConstructorCollection, ConstructorDefinition> selector)
        {
            // get all constructors of the mapped type
            var constructors = Component.ValueType.GetConststructorCollection();
            // get the selected constructor from the map
            var definition = selector.Invoke(constructors);
            foreach (var item in definition.Where(c => c.Value != null))
            {
                // add all arguments that were added in the mapping
                AddArgument(item.Name, item.Value, null);
            }

            // mark the constructor to be selected
            var component = Component.CreateComponent<T>();
            component.ConstructorDefinition = definition;
            Container.AddOrReplace(component);

            return new BindingExpression<T>(Container, component);
        }

        #endregion

        #region Private Implementation

        private IBindingExpression<T> AddArgument<TArg>(string name, TArg value, Expression<Func<TArg>> predicate)
        {
            Component.Arguments.Add(new BindingArgument<TArg>
            {
                Name = name,
                Value = value,
                Callback = predicate
            });

            return new BindingExpression<T>(Container, Component);
        }

        #endregion
    }
}
