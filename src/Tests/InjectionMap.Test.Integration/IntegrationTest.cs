using System;
using NUnit.Framework;
using System.Linq;

namespace InjectionMap.Test.Integration
{
    [TestFixture]
    public class IntegrationTest
    {
        static IntegrationTest()
        {
            InjectionMap.Tracing.LoggerFactory.LoggerCallback = () => new TraceLogger();
        }

        [SetUp]
        public void Initialize()
        {
            // clean previous mappings
            using (var mapper = new InjectionMapper())
            {
                mapper.Clean<IInjectionMappingTest>();
            }

            // relocate mappings
            var initializer = new MapInitializer();
            initializer.Initialize(this.GetType().Assembly);
        }

        [Test]
        public void IInjectionMappingRegistrationTest()
        {
            using (var resolver = new InjectionResolver())
            {
                var maps = resolver.ResolveMultiple<IInjectionMappingTest>().ToList();

                Assert.IsNotNull(maps[0] as FirstInjectionMappingTestMock);
                Assert.IsNotNull(maps[1] as SecondInjectionMappingTestMock);

                Assert.IsTrue(maps[0].ID == 1);
                Assert.IsTrue(maps[1].ID == 5);
            }
        }

        [Test]
        public void InjectionMappingTest()
        {
            using (var mapper = new InjectionMapper())
            {
                mapper.Clean<IInjectionMappingTest>();

                // create a complex map (map doesn't make sense... but it's complexer)
                mapper.Map<IInjectionMappingTest, SecondInjectionMappingTestMock>().WithArgument("id", () => 5).OnResolved(m => m.ID = 6);
            }

            using (var resolver = new InjectionResolver())
            {
                var map = resolver.Resolve<IInjectionMappingTest>();
                Assert.IsTrue(map.ID == 6);
            }
        }

        [Test]
        public void MapToCustomContainer()
        {
            var container = new MappingContext();
            using (var defMapper = new InjectionMapper())
            {
                // register the container to the default container so it can be retrieved later
                defMapper.Map<IMappingContext>().For(() => container);
            }

            using (var mapper = new InjectionMapper(container))
            {
                // add the container to the mapping
                mapper.Map<IMappingContext>().For(() => container);

                // create a complex map (map doesn't make sense... but it's complexer)
                mapper.Map<IInjectionMappingTest, SecondInjectionMappingTestMock>().WithArgument("id", () => 5).OnResolved(m => m.ID = 6);
            }

            using (var defResolver = new InjectionResolver())
            {
                // retrieve the original container
                var resolvedContainer = defResolver.Resolve<IMappingContext>();
                using (var resolver = new InjectionResolver(resolvedContainer))
                {
                    // resolve the mapping
                    var map = resolver.Resolve<IInjectionMappingTest>();
                    Assert.IsTrue(map.ID == 6);
                }
            }
        }

        [Test]
        public void MapArgumentToUnregisteredType()
        {
            using (var mapper = new InjectionMapper())
            {
                mapper.Map<IUnregisteredTypeArgument, UnregisteredTypeArgument>().OnResolved(a => a.ID = 5);
            }

            using (var resolver = new InjectionResolver())
            {
                var map = resolver.Resolve<UnregisteredType>();

                Assert.IsNotNull(map);
                Assert.IsTrue(map.ID == 5);
            }
        }

        [Test]
        public void MapMultipleHyrarchies()
        {
            using (var mapper = new InjectionMapper())
            {
                mapper.Map<TypeOne>().ToSelf();
                mapper.Map<TypeTwo>().ToSelf();
                mapper.Map<TypeThree>().ToSelf();
                mapper.Map<TypeFour>().ToSelf();
            }

            using (var resolver = new InjectionResolver())
            {
                var four = resolver.Resolve<TypeFour>();

                Assert.IsNotNull(four);
                Assert.IsNotNull(four.TypeThree);
                Assert.IsNotNull(four.TypeThree.TypeTwo);
                Assert.IsNotNull(four.TypeThree.TypeTwo.TypeOne);
            }
        }

        #region Mocks

        internal interface IInjectionMappingTest
        {
            int ID { get; }
        }

        internal class InjectionMapperMock : IMapInitializer
        {
            public void InitializeMap(IMappingProvider container)
            {
                container.Map<IInjectionMappingTest, FirstInjectionMappingTestMock>();
                container.Map<IInjectionMappingTest>().For<SecondInjectionMappingTestMock>().WithArgument(() => 5);
            }
        }

        internal class FirstInjectionMappingTestMock : IInjectionMappingTest
        {
            public int ID
            {
                get
                {
                    return 1;
                }
            }
        }

        internal class SecondInjectionMappingTestMock : IInjectionMappingTest
        {
            public SecondInjectionMappingTestMock(int id)
            {
                ID = id;
            }

            public int ID { get; internal set; }
        }


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

        internal class TypeOne
        {
        }

        internal class TypeTwo
        {
            public TypeTwo(TypeOne typeOne)
            {
                TypeOne = typeOne;
            }

            public TypeOne TypeOne { get; set; }
        }

        internal class TypeThree
        {
            public TypeThree(TypeTwo typeTwo)
            {
                TypeTwo = typeTwo;
            }

            public TypeTwo TypeTwo { get; set; }
        }

        internal class TypeFour
        {
            public TypeFour(TypeThree typeThree)
            {
                TypeThree = typeThree;
            }

            public TypeThree TypeThree { get; set; }
        }

        #endregion
    }
}
