using NUnit.Framework;
using System.Linq;

namespace InjectionMap.Test.Integration
{
    [TestFixture]
    public class SingletonScopeTest : TestBase
    {
        [SetUp]
        public void Initialize()
        {
            Mapper.Clean<ISingletonScope>();
        }

        [Test]
        [Description("Creates a singleton mapping and overrides it with a normal mapping. There should only be the singleton left.")]
        public void SingletonScopeWithTryOverrideExistingSingleton()
        {
            // create singleton map
            Mapper.Map<ISingletonScope, SingletonScopeMock>().WithArgument("id", () => 1).WithConfiguration(InjectionFlags.AsSingleton);

            // try create second map
            Mapper.Map<ISingletonScope, SingletonScopeMock>().WithArgument("id", () => 2);

            // resolve
            var maps = Resolver.ResolveMultiple<ISingletonScope>();

            Assert.IsTrue(maps.Count() == 1);
            Assert.IsTrue(maps.First().ID == 1);
        }

        [Test]
        [Description("Creates a mapping and overrides it with a singleton. There should only be the singleton left.")]
        public void SingletonScopeWithTryOverrideExistingMappingWithSingleton()
        {
            // create multiple maps
            Mapper.Map<ISingletonScope, SingletonScopeMock>().WithArgument("id", () => 2);
            Mapper.Map<ISingletonScope, SingletonScopeMock>().WithArgument("id", () => 3);

            // resolve
            var maps = Resolver.ResolveMultiple<ISingletonScope>();
            Assert.IsTrue(maps.Count() == 2);

            // override with singleton map
            Mapper.Map<ISingletonScope, SingletonScopeMock>().WithArgument("id", () => 1).WithConfiguration(InjectionFlags.AsSingleton);

            // resolve
            maps = Resolver.ResolveMultiple<ISingletonScope>();

            Assert.IsTrue(maps.Count() == 1);
            Assert.IsTrue(maps.First().ID == 1);
        }
    }

    internal interface ISingletonScope
    {
        int ID { get; set; }
    }

    internal class SingletonScopeMock : ISingletonScope
    {
        public SingletonScopeMock(int id)
        {
            ID = id;
        }

        public int ID { get; set; }
    }
}
