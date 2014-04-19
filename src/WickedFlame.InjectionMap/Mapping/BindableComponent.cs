
namespace WickedFlame.InjectionMap.Mapping
{
    internal class BindableComponent
    {
        public BindableComponent(IComponentContainer container, IMappingComponent component)
        {
            _container = container;
            _component = component;
        }

        readonly IComponentContainer _container;
        public IComponentContainer Container
        {
            get
            {
                return _container;
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
