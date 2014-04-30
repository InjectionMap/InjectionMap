using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectionMap.Test.Integration
{
    [TestFixture]
    public class CacheScopeTest : TestBase
    {
        [SetUp]
        public void Initialize()
        {
            Mapper.Clean<ICacheScope>();
        }

        [Test]
        public void ChangeValueInCachedScope()
        {
            // mapping
            Mapper.Map<ICacheScope, CacheScopeMock>().WithArgument("id", () => 2).WithConfiguration(InjectionFlags.CacheValue);

            // resolve
            var map = Resolver.Resolve<ICacheScope>();

            Assert.AreEqual(map.ID, 2);

            map.ID = 3;

            // resolving should deliver the same instance
            var map2 = Resolver.Resolve<ICacheScope>();

            Assert.AreSame(map, map2);
            Assert.AreEqual(map2.ID, 3);
            Assert.AreEqual(map2.ID, map.ID);
        }

        [Test]
        public void ChangeValueWihtoutCachingScope()
        {
            // mapping
            Mapper.Map<ICacheScope, CacheScopeMock>().WithArgument("id", () => 2);

            // resolve
            var map = Resolver.Resolve<ICacheScope>();

            Assert.AreEqual(map.ID, 2);

            map.ID = 3;

            // resolving should deliver the same instance
            var map2 = Resolver.Resolve<ICacheScope>();

            Assert.AreNotSame(map, map2);
            Assert.AreNotEqual(map2.ID, map.ID);
        }
    }

    internal interface ICacheScope
    {
        int ID { get; set; }
    }

    internal class CacheScopeMock : ICacheScope
    {
        public CacheScopeMock(int id)
        {
            ID = id;
        }

        public int ID { get; set; }
    }
}
