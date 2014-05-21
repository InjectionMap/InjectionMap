using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectionMap.Test.Integration
{
    [TestFixture]
    public class ExtendMapTest : TestBase
    {
        [SetUp]
        public void Initialize()
        {
            Mapper.Clean<IExtendMap>();
        }

        [Test]
        public void ExtendMapWithArguments()
        {
            Mapper.Map<IExtendMap, ExtendMapMock>();

            var value = Resolver.ExtendMap<IExtendMap>().WithArgument("id", () => 1).WithArgument("name", () => "test1").Resolve();
            Assert.IsTrue(value.ID == 1);
            Assert.IsTrue(value.Name == "test1");

            value = Resolver.ExtendMap<IExtendMap>().WithArgument("id", 2).WithArgument("name", "test2").Resolve();
            Assert.IsTrue(value.ID == 2);
            Assert.IsTrue(value.Name == "test2");

            value = Resolver.ExtendMap<IExtendMap>().WithArgument(() => 3).WithArgument(() => "test3").Resolve();
            Assert.IsTrue(value.ID == 3);
            Assert.IsTrue(value.Name == "test3");

            value = Resolver.ExtendMap<IExtendMap>().WithArgument(4).WithArgument("test4").Resolve();
            Assert.IsTrue(value.ID == 4);
            Assert.IsTrue(value.Name == "test4");            
        }

        [Test]
        public void ExtendMapThatHasExistingArguments()
        {
            Mapper.Map<IExtendMap, ExtendMapMock>().WithArgument("test");

            var value = Resolver.ExtendMap<IExtendMap>().WithArgument("id", () => 1).Resolve();
            Assert.IsTrue(value.ID == 1);
            Assert.IsTrue(value.Name == "test");

            value = Resolver.ExtendMap<IExtendMap>().WithArgument("id", () => 1).WithArgument("name", () => "test1").Resolve();
            Assert.IsTrue(value.ID == 1);
            Assert.IsTrue(value.Name == "test1");

            value = Resolver.ExtendMap<IExtendMap>().WithArgument("id", 2).WithArgument("name", "test2").Resolve();
            Assert.IsTrue(value.ID == 2);
            Assert.IsTrue(value.Name == "test2");

            value = Resolver.ExtendMap<IExtendMap>().WithArgument(() => 3).WithArgument(() => "test3").Resolve();
            Assert.IsTrue(value.ID == 3);
            // original argument is named, new is not names so original has to be taken
            Assert.IsTrue(value.Name == "test");

            value = Resolver.ExtendMap<IExtendMap>().WithArgument(4).WithArgument("test4").Resolve();
            Assert.IsTrue(value.ID == 4);
            // original argument is named, new is not names so original has to be taken
            Assert.IsTrue(value.Name == "test"); 
        }

        [Test]
        public void ExtendMapWithOverrideExistingArguments()
        {
            Mapper.Map<IExtendMap, ExtendMapMock>().WithArgument("id", () => 1);

            var value = Resolver.ExtendMap<IExtendMap>().WithArgument("name", () => "test").WithArgument("id", () => 2).Resolve();
            Assert.IsTrue(value.ID == 2);
            Assert.IsTrue(value.Name == "test");
        }
    }

    interface IExtendMap
    {
        int ID { get; set; }

        string Name { get; set; }
    }

    class ExtendMapMock : IExtendMap
    {
        public ExtendMapMock(int id, string name)
        {
            ID = id;
            Name = name;
        }

        public int ID { get; set; }

        public string Name { get; set; }
    }
}
