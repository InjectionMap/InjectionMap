using NUnit.Framework;

namespace InjectionMap.Test.Integration
{
    [TestFixture]
    public class ConstantScopeTest : TestBase
    {
        [SetUp]
        public void Initialize()
        {
            Mapper.Clean<IConstantScope>();
        }

        [Test]
        public void ChangeValueInConstantScope()
        {
            // mapping
            Mapper.Map<IConstantScope, ConstantScopeMock>().WithArgument("id", () => 2).WithConfiguration(InjectionFlags.AsConstant);

            // resolve
            var map = Resolver.Resolve<IConstantScope>();

            Assert.AreEqual(map.ID, 2);

            map.ID = 3;

            // resolving should deliver the same instance
            var map2 = Resolver.Resolve<IConstantScope>();

            Assert.AreSame(map, map2);
            Assert.AreEqual(map2.ID, 3);
            Assert.AreEqual(map2.ID, map.ID);
        }

        [Test]
        public void ChangeValueWihtoutCachingScope()
        {
            // mapping
            Mapper.Map<IConstantScope, ConstantScopeMock>().WithArgument("id", () => 2);

            // resolve
            var map = Resolver.Resolve<IConstantScope>();

            Assert.AreEqual(map.ID, 2);

            map.ID = 3;

            // resolving should deliver the same instance
            var map2 = Resolver.Resolve<IConstantScope>();

            Assert.AreNotSame(map, map2);
            Assert.AreNotEqual(map2.ID, map.ID);
        }

        #region Mocks

        internal interface IConstantScope
        {
            int ID { get; set; }
        }

        internal class ConstantScopeMock : IConstantScope
        {
            public ConstantScopeMock(int id)
            {
                ID = id;
            }

            public int ID { get; set; }
        }

        #endregion
    }
}
