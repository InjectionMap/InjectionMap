using NUnit.Framework;
using System;
using System.Linq;

namespace WickedFlame.InjectionMap.NuGetPackage.Integration.Test
{
    [TestFixture]
    public class NugetPckageTest
    {
        [SetUp]
        public void Initialize()
        {
            // clean previous mappings
            using (var mapper = new InjectionMapper())
            {
                mapper.Clean<IInjectionMappingTest>();
            }

            // relocate mappings
            InjectionMapper.Initialize(this.GetType().Assembly);
        }

        [Test]
        public void IInjectionMappingTest()
        {
            using (var resolver = new InjectionResolver())
            {
                var maps = resolver.ResolveMultiple<IInjectionMappingTest>().ToList();

                Assert.IsInstanceOf<FirstInjectionMappingTestMock>(maps[0]);
                Assert.IsInstanceOf<SecondInjectionMappingTestMock>(maps[1]);

                Assert.IsTrue(maps[0].ID == 1);
                Assert.IsTrue(maps[1].ID == 5);
            }
        }

        [Test]
        public void InjectionMappingTest()
        {
            var container = new MappingContainer();
            using (var defMapper = new InjectionMapper())
            {
                // register the container to the default container so it can be retrieved later
                defMapper.Map<IMappableContainer>().For(() => container);
            }

            using (var mapper = new InjectionMapper(container))
            {
                // add the container to the mapping
                mapper.Map<IMappableContainer>().For(() => container);

                // create a complex map (map doesn't make sense... but it's complexer)
                mapper.Map<IInjectionMappingTest, SecondInjectionMappingTestMock>().WithArgument("id", () => 5).OnResolved(m => m.ID = 6);
            }

            using (var defResolver = new InjectionResolver())
            {
                // retrieve the original container
                var resolvedContainer = defResolver.Resolve<IMappableContainer>();
                using (var resolver = new InjectionResolver(resolvedContainer))
                {
                    // resolve the mapping
                    var map = resolver.Resolve<IInjectionMappingTest>();
                    Assert.IsTrue(map.ID == 6);
                }
            }
        }
    }

    class InjectionMapperMock : IInjectionMapping
    {
        public void Register(IMappingProvider container)
        {
            container.Map<IInjectionMappingTest, FirstInjectionMappingTestMock>();
            container.Map<IInjectionMappingTest>().For<SecondInjectionMappingTestMock>().WithArgument(() => 5);
        }
    }

    interface IInjectionMappingTest
    {
        int ID { get; }
    }

    class FirstInjectionMappingTestMock : IInjectionMappingTest
    {
        public int ID
        {
            get
            {
                return 1;
            }
        }
    }

    class SecondInjectionMappingTestMock : IInjectionMappingTest
    {
        public SecondInjectionMappingTestMock(int id)
        {
            ID = id;
        }

        public int ID { get; internal set; }
    }
}
