
namespace WickedFlame.InjectionMap.Mapping
{
    internal class BindableComponent
    {
        public BindableComponent(IComponentContainer container, IMappingComponent component)
        {
            _componentContainer = container;
            _component = component;
        }

        readonly IComponentContainer _componentContainer;
        public IComponentContainer ComponentContainer
        {
            get
            {
                return _componentContainer;
            }
        }

        readonly IMappingComponent _component;
        public IMappingComponent Component
        {
            get
            {
                return _component;
            }
        }
    }
}
