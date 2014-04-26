
namespace WickedFlame.InjectionMap.Test.Mock
{
    class InjectionMapperMock : IInjectionMapping
    {
        public void InitializeMap(IMappingProvider container)
        {
            container.Map<IInjectionMapperMock1, InjectionMapperMock1>();
            container.Map<IInjectionMapperMock2>().For<InjectionMapperMock2>();
        }
    }

    public interface IInjectionMapperMock1
    {
        int ID { get; }
    }

    public class InjectionMapperMock1 : IInjectionMapperMock1
    {
        public int ID
        {
            get
            {
                return 1;
            }
        }
    }

    public interface IInjectionMapperMock2
    {
        int ID { get; }
    }

    public class InjectionMapperMock2 : IInjectionMapperMock2
    {
        public int ID
        {
            get
            {
                return 2;
            }
        }
    }
}
