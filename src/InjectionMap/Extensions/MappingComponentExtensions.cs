using InjectionMap.Internal;

namespace InjectionMap.Extensions
{
    public static class MappingComponentExtensions
    {
        internal static MappingComponent<T> CreateComponent<T>(this IMappingComponent component)
        {
            var copy = new MappingComponent<T>(component.ID)
            {
                KeyType = component.KeyType,
                //ValueCallback = component.ValueCallback,
                //OnResolvedCallback = component.OnResolvedCallback,
                //ValueType = component.ValueType,
                MappingConfiguration = component.MappingConfiguration,
                IsSubstitute = component.IsSubstitute,
                ConstructorDefinition = component.ConstructorDefinition
            };

            var cast = component as MappingComponent<T>;
            if (cast != null)
            {
                copy.ValueCallback = cast.ValueCallback;
                copy.OnResolvedCallback = cast.OnResolvedCallback;
            }
            else
            {
                if (component.ValueCallback != null)
                    copy.ValueCallback = () => (T)component.ValueCallback.Compile().Invoke();

                if (component.OnResolvedCallback != null)
                    copy.OnResolvedCallback = m => component.OnResolvedCallback(m);
            }

            copy.Arguments.AddFrom<IBindingArgument>(component.Arguments);

            return copy;
        }

        internal static MappingComponent CreateComponent(this IMappingComponent component)
        {
            var copy = new MappingComponent(component.ID)
            {
                KeyType = component.KeyType,
                ValueType = component.ValueType,
                MappingConfiguration = component.MappingConfiguration,
                IsSubstitute = component.IsSubstitute,
                ConstructorDefinition = component.ConstructorDefinition
            };

            copy.ValueCallback = component.ValueCallback;
            copy.OnResolvedCallback = component.OnResolvedCallback;
            
            copy.Arguments.AddFrom<IBindingArgument>(component.Arguments);

            return copy;
        }

        internal static IBindingExpression<T> CreateBindingExpression<T>(this IMappingComponent component, IComponentCollection container)
        {
            container.AddOrReplace(component);
            if (component.MappingConfiguration == null || component.MappingConfiguration.AsSingleton)
            {
                container.ReplaceAll(component);
            }

            return new BindingExpression<T>(container, component);
        }

        internal static IBoundExpression<T> CreateBoundExpression<T>(this IMappingComponent component, IComponentCollection container)
        {
            container.AddOrReplace(component);
            if (component.MappingConfiguration == null || component.MappingConfiguration.AsSingleton)
            {
                container.ReplaceAll(component);
            }

            return new BoundExpression<T>(container, component);
        }

        internal static IMappingExpression<T> CreateMappingExpression<T>(this IMappingComponent component, IComponentCollection container)
        {
            container.AddOrReplace(component);
            if (component.MappingConfiguration == null || component.MappingConfiguration.AsSingleton)
            {
                container.ReplaceAll(component);
            }

            return new MappingExpression<T>(container, component);
        }


        internal static IResolverExpression<T> CreateExtendedResolverExpression<T>(this IMappingComponent component, IComponentCollection container)
        {
            typeof(T).EnsureMappingTypeMatches(component.KeyType);

            // create a copy of the component to leave the original as is
            //var copy = component.CreateComponent();

            // create the ResolverExpression
            return new ResolverExpression<T>(container, component);
        }

        internal static IMultiResolverExpression<T> CreateExtendedMultiResolverExpression<T>(this IMappingComponent component, IComponentCollection container)
        {
            typeof(T).EnsureMappingTypeMatches(component.KeyType);

            // create the ResolverExpression
            return new MultiResolverExpression<T>(container);
        }

        internal static IResolverExpression<T> CreateResolverExpression<T>(this IMappingComponent component, IComponentCollection container)
        {
            typeof(T).EnsureMappingTypeMatches(component.KeyType);

            // create a copy of the component to leave the original as is
            var copy = component.CreateComponent();

            // create the ResolverExpression
            return new ResolverExpression<T>(container, copy);
        }
         
    }
}
