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
            var ce = map as IComponentExpression;
            Assert.IsNotNull(ce);
            Assert.IsNotNull(ce.Component.ValueCallback);

            var value1 = ce.Component.ValueCallback.Compile().Invoke();

            var map2 = Resolver.Resolve<IResolveOnMapping>();

            Assert.AreSame(value1, map2);

            var map3 = Resolver.Resolve<IResolveOnMapping>();

            Assert.AreSame(map2, map3);
        }

        #region Mocks

        private interface IResolveOnMapping
        {
        }

        private class ResolveOnMappingMock : IResolveOnMapping
        {
        }

        #endregion
    }
}
