using NUnit.Framework;
using System;

namespace InjectionMap.Test.Integration
{
    [TestFixture]
    public class MapToUnregisterdType : TestBase
    {
        [SetUp]
        public void Initialize()
        {
            Mapper.Clean<IUnregisteredMapArgument>();
        }

        [Test]
        public void MapArgumentToUnregisteredClass()
        {
            Mapper.Map<IUnregisteredMapArgument, UnregisteredMapArgument>().OnResolved(a => a.ID = 5);

            var map = Resolver.Resolve<UnregisteredMap>();

            Assert.IsNotNull(map);
            Assert.IsTrue(map.ID == 5);
        }

        [Test]
        public void MapArgumentToUnregisteredClassWithExtendMap()
        {
            Mapper.Map<IUnregisteredMapArgument, UnregisteredMapArgument>().OnResolved(a => a.ID = 5);

            var map = Resolver.ExtendMap<UnregisteredMap>().Resolve();

            Assert.IsNotNull(map);
            Assert.IsTrue(map.ID == 5);
        }

        [Test]
        public void MapArgumentToUnregisteredClassWithExtendMapAndArgument()
        {
            Mapper.Map<IUnregisteredMapArgument, UnregisteredMapArgument>().OnResolved(a => a.ID = 1);

            var map = Resolver.ExtendMap<UnregisteredMap2>().WithArgument("id", () => 5).Resolve();

            Assert.IsNotNull(map);
            // the maped constructor is taken
            Assert.IsTrue(map.ID == 1);
        }






        internal interface IUnregisteredMapArgument
        {
            int ID { get; set; }
        }

        internal class UnregisteredMapArgument : IUnregisteredMapArgument
        {
            public int ID { get; set; }
        }

        internal class UnregisteredMap
        {
            public UnregisteredMap(IUnregisteredMapArgument argument)
            {
                if (argument == null)
                    throw new ArgumentNullException("argument");

                ID = argument.ID;
            }

            public int ID { get; set; }
        }

        internal class UnregisteredMap2
        {
            public UnregisteredMap2(IUnregisteredMapArgument argument)
            {
                if (argument == null)
                    throw new ArgumentNullException("argument");

                ID = argument.ID;
            }

            public UnregisteredMap2(int id)
            {
                ID = id;
            }

            public int ID { get; set; }
        }
    }
}
