
namespace InjectionMap.Test.Integration
{
    public class TestBase
    {
        InjectionMapper _mapper;
        protected InjectionMapper Mapper
        {
            get
            {
                if (_mapper == null)
                    _mapper = new InjectionMapper();
                return _mapper;
            }
        }

        InjectionResolver _resolver;
        protected InjectionResolver Resolver
        {
            get
            {
                if (_resolver == null)
                    _resolver = new InjectionResolver();
                return _resolver;
            }
        }

    }
}
