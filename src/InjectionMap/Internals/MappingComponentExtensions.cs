using InjectionMap.Expressions;
using InjectionMap.Mapping;

namespace InjectionMap.Internals
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
                MappingOption = component.MappingOption,
                IsSubstitute = component.IsSubstitute
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

        internal static IBindingExpression<T> CreateBindingExpression<T>(this IMappingComponent component, IComponentCollection container)
        {
            container.AddOrReplace(component);
            if (component.MappingOption == null || component.MappingOption.AsSingleton)
            {
                container.ReplaceAll(component);
            }

            return new BindingExpression<T>(container, component);
        }

        internal static IBoundExpression<T> CreateBoundExpression<T>(this IMappingComponent component, IComponentCollection container)
        {
            container.AddOrReplace(component);
            if (component.MappingOption == null || component.MappingOption.AsSingleton)
            {
                container.ReplaceAll(component);
            }

            return new BoundExpression<T>(container, component);
        }

        internal static IMappingExpression<T> CreateMappingExpression<T>(this IMappingComponent component, IComponentCollection container)
        {
            container.AddOrReplace(component);
            if (component.MappingOption == null || component.MappingOption.AsSingleton)
            {
                container.ReplaceAll(component);
            }

            return new MappingExpression<T>(container, component);
        }
    }
}
