using NUnit.Framework;
using System;

namespace InjectionMap.Integration.UnitTests
{
    [TestFixture]
    public class MapToUnregisterdType : TestBase
    {
        [SetUp]
        public void Initialize()
        {
            Mapper.Clean<IUnregisteredTypeArgument>();
        }

        [Test]
        public void MapArgumentToUnregisteredType()
        {
            Mapper.Map<IUnregisteredTypeArgument, UnregisteredTypeArgument>().OnResolved(a => a.ID = 5);

            var map = Resolver.Resolve<UnregisteredType>();

            Assert.IsNotNull(map);
            Assert.IsTrue(map.ID == 5);
        }

        [Test]
        public void MapArgumentToUnregisteredClassWithExtendMap()
        {
            Mapper.Map<IUnregisteredTypeArgument, UnregisteredTypeArgument>().OnResolved(a => a.ID = 5);

            var map = Resolver.For<UnregisteredType>().Resolve();

            Assert.IsNotNull(map);
            Assert.IsTrue(map.ID == 5);
        }

        [Test]
        public void MapArgumentToUnregisteredClassWithExtendMapAndArgument()
        {
            Mapper.Map<IUnregisteredTypeArgument, UnregisteredTypeArgument>().OnResolved(a => a.ID = 1);

            var map = Resolver.For<UnregisteredMapMultyConstructor>().WithArgument("id", () => 5).Resolve();

            // the maped constructor is taken
            Assert.IsTrue(map.ID == 1);
        }

        [Test]
        public void MapArgumentToUnregisteredClassWithExtendMapAndArgumentInjectionConstructor()
        {
            Mapper.Map<IUnregisteredTypeArgument, UnregisteredTypeArgument>().OnResolved(a => a.ID = 1);

            var map = Resolver.For<UnregisteredMapInjectionConstructor>().WithArgument("id", () => 5).Resolve();

            // the injectionconstructor is taken
            Assert.IsTrue(map.ID == 5);
        }

        [Test]
        public void MapArgumentToUnregisteredClassWithDefaultConstructor()
        {
            var map = Resolver.Resolve<UnregisteredMapDefaultConstructor>();

            // the defaultconstructor is taken
            Assert.IsTrue(map.ID == 1);
        }

        [Test]
        public void MapArgumentToUnregisteredClassWithoutDefaultConstructor()
        {
            Mapper.Map<IUnregisteredTypeArgument, UnregisteredTypeArgument>().OnResolved(uta => uta.ID = 5);
            var map = Resolver.Resolve<UnregisteredType>();

            // the defaultconstructor is taken
            Assert.IsTrue(map.ID == 5);
        }

        [Test]
        public void MapArgumentToUnregisteredClassWithDefaultAndInjectionConstructor()
        {
            var map = Resolver.For<UnregisteredMapDefaultConstructor>().WithArgument("id", () => 5).Resolve();

            // the defaultconstructor is taken
            Assert.IsTrue(map.ID == 5);
        }

        #region Mocks

        internal interface IUnregisteredTypeArgument
        {
            int ID { get; set; }
        }

        internal class UnregisteredTypeArgument : IUnregisteredTypeArgument
        {
            public int ID { get; set; }
        }

        internal class UnregisteredType
        {
            public UnregisteredType(IUnregisteredTypeArgument argument)
            {
                if (argument == null)
                    throw new ArgumentNullException("argument");

                ID = argument.ID;
            }

            public int ID { get; set; }
        }

        internal class UnregisteredMapMultyConstructor
        {
            public UnregisteredMapMultyConstructor(IUnregisteredTypeArgument argument)
            {
                if (argument == null)
                    throw new ArgumentNullException("argument");

                ID = argument.ID;
            }

            public UnregisteredMapMultyConstructor(int id)
            {
                ID = id;
            }

            public int ID { get; set; }
        }

        internal class UnregisteredMapInjectionConstructor
        {
            public UnregisteredMapInjectionConstructor(IUnregisteredTypeArgument argument)
            {
                if (argument == null)
                    throw new ArgumentNullException("argument");

                ID = argument.ID;
            }

            [InjectionConstructor]
            public UnregisteredMapInjectionConstructor(int id)
            {
                ID = id;
            }

            public int ID { get; set; }
        }

        internal class UnregisteredMapDefaultConstructor
        {
            public UnregisteredMapDefaultConstructor()
            {
                ID = 1;
            }

            [InjectionConstructor]
            public UnregisteredMapDefaultConstructor(int id)
            {
                ID = id;
            }

            public int ID { get; set; }
        }

        #endregion
    }
}
