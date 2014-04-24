using NUnit.Framework;

namespace WickedFlame.InjectionMap.Test.Integration
{
    [TestFixture]
    public class CustomContainerMappingTests : TestBase
    {
        [SetUp]
        public void TestInitialize()
        {
            Mapper.Clean<ICustomContainer>();
        }

        [Test]
        [Description("Adds a mapping to a custom container")]
        public void MapToCustomContainer()
        {
            // create contaienrs and mappers
            var container = new MappingContainer();
            var customMapper = new InjectionMapper(container);

            // mapping
            customMapper.Map<ICustomContainer, CustomContainerMapMock>();

            // resolve
            var defaultResolver = new InjectionResolver();
            var map = defaultResolver.Resolve<ICustomContainer>();
            Assert.IsNull(map);

            var customResolver = new InjectionResolver(container);
            map = customResolver.Resolve<ICustomContainer>();
            Assert.IsNotNull(map);
        }

        [Test]
        [Description("Adds a mapping to the default container")]
        public void MapToDefaultContainer()
        {
            // create contaienrs and mappers
            var container = new MappingContainer();
            var defaultMapper = new InjectionMapper();

            // mapping
            defaultMapper.Map<ICustomContainer, CustomContainerMapMock>();

            // resolve
            var customResolver = new InjectionResolver(container);
            var map = customResolver.Resolve<ICustomContainer>();
            Assert.IsNull(map);

            var defaultResolver = new InjectionResolver();
            map = defaultResolver.Resolve<ICustomContainer>();
            Assert.IsNotNull(map);
        }

        [Test]
        [Description("Adds a mapping to a custom and the default container")]
        public void MapToCustomAndDefaultContainer()
        {
            // create contaienrs and mappers
            var container = new MappingContainer();
            var customMapper = new InjectionMapper(container);
            var defaultMapper = new InjectionMapper();

            // mapping
            customMapper.Map<ICustomContainer, CustomContainerMapMock>();
            defaultMapper.Map<ICustomContainer, DefaultContainerMapMock>();

            // resolve
            var defaultResolver = new InjectionResolver();
            var map = defaultResolver.Resolve<ICustomContainer>();
            Assert.IsInstanceOf(typeof(DefaultContainerMapMock), map);

            var customResolver = new InjectionResolver(container);
            map = customResolver.Resolve<ICustomContainer>();
            Assert.IsInstanceOf(typeof(CustomContainerMapMock), map);
        }
    }

    public interface ICustomContainer
    {
    }

    public class CustomContainerMapMock : ICustomContainer
    {
    }

    public class DefaultContainerMapMock : ICustomContainer
    {
    }
}
