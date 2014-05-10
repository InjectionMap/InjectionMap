using NUnit.Framework;

namespace InjectionMap.Test.Integration
{
    [TestFixture]
    public class ResolveValueOnMappingScopeTest : TestBase
    {
        [SetUp]
        public void Initialize()
        {
            Mapper.Clean<IResolveOnMapping>();
        }

        [Test]
        public void ResolveValueOnMapping()
        {
            var map = Mapper.Map<IResolveOnMapping, ResolveOnMappingMock>().WithConfiguration(InjectionFlags.ResolveValueOnMapping);

            Assert.IsNotNull(map.Component.ValueCallback);

            var value1 = map.Component.ValueCallback.Compile().Invoke();

            var map2 = Resolver.Resolve<IResolveOnMapping>();

            Assert.AreSame(value1, map2);

            var map3 = Resolver.Resolve<IResolveOnMapping>();

            Assert.AreSame(map2, map3);
        }
    }

    interface IResolveOnMapping
    {
    }

    class ResolveOnMappingMock : IResolveOnMapping
    {
    }
}
