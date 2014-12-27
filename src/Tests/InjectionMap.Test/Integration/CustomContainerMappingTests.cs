using NUnit.Framework;

namespace InjectionMap.Test.Integration
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
            var container = new MappingContext();
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
            var container = new MappingContext();
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
            var container = new MappingContext();
            var customMapper = new InjectionMapper(container);
            var defaultMapper = new InjectionMapper();

            // mapping
            customMapper.Map<ICustomContainer, CustomContainerMapMock>();
            defaultMapper.Map<ICustomContainer, DefaultContainerMapMock>();

            // resolve
            var defaultResolver = new InjectionResolver();
            var map = defaultResolver.Resolve<ICustomContainer>();
            Assert.IsInstanceOf(typeof (DefaultContainerMapMock), map);

            var customResolver = new InjectionResolver(container);
            map = customResolver.Resolve<ICustomContainer>();
            Assert.IsInstanceOf(typeof (CustomContainerMapMock), map);
        }

        [Test]
        [Description("Creates 2 mappings with the same key in 2 differenct containers and resolves from these containers")]
        public void ResolveDirectlyFromCustomContainer()
        {
            // create contaienrs and mappers
            var container = new MappingContext();
            var customMapper = new InjectionMapper(container);
            var defaultMapper = new InjectionMapper();

            // mapping
            customMapper.Map<ICustomContainer, CustomContainerMapMock>();
            defaultMapper.Map<ICustomContainer, DefaultContainerMapMock>();

            // resolve
            var map = Resolver.Resolve<ICustomContainer>();
            Assert.IsInstanceOf(typeof (DefaultContainerMapMock), map);

            // resolve from the custom container
            map = Resolver.Resolve<ICustomContainer>(container);
            Assert.IsInstanceOf(typeof (CustomContainerMapMock), map);
        }

        [Test]
        public void ResolveFromCustomContainerWithConstructorInjection()
        {
            // create contaienrs and mappers
            var context = new MappingContext("context");
            var mapper = new InjectionMapper(context);

            // mapping
            mapper.Map<ICustomContainer, CustomContainerMapMock>();
            mapper.Map<IObjectWithConstructor, ObjectWithConstructor>();

            var resolver = new InjectionResolver("context");

            // resolve
            var map = resolver.Resolve<IObjectWithConstructor>();

            Assert.IsNotNull(map.CustomContainer);
        }

        #region Mocks

        public interface ICustomContainer
        {
        }

        public class CustomContainerMapMock : ICustomContainer
        {
        }

        public class DefaultContainerMapMock : ICustomContainer
        {
        }

        public interface IObjectWithConstructor
        {
            ICustomContainer CustomContainer { get; set; }
        }

        public class ObjectWithConstructor : IObjectWithConstructor
        {
            public ObjectWithConstructor(ICustomContainer customContainer)
            {
                CustomContainer = customContainer;
            }

            public ICustomContainer CustomContainer { get; set; }
        }

        #endregion
    }
}
