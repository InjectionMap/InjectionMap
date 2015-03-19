using NUnit.Framework;
using System.Linq;

namespace InjectionMap.Integration.UnitTests
{
    [TestFixture]
    public class InjectionMapperTests
    {
        [SetUp]
        public void Initialize()
        {
            var mapper = new InjectionMapper();
            mapper.Clean<IInjectionMapperMock1>();
            mapper.Clean<IInjectionMapperMock2>();
        }

        [Test]
        public void InjectionMapperWithValueTest()
        {
            using (var mapper = new InjectionMapper())
            {
                // clean all previous mappings to ensure test
                mapper.Clean<ICustomMock>();

                //InjectionMapper.Map<ITestMock1, TestMock1>(m => m.For(new TestMock1()));
                mapper.Map<ICustomMock, CustomMock>().As(() => new CustomMock());
            }

            using (var resolver = new InjectionResolver())
            {
                var map1 = resolver.Resolve<ICustomMock>();

                Assert.AreEqual(map1.ID, 1);
            }
        }

        [Test]
        [Description("Adds a instance that gets returned to the complete mapping")]
        public void InjectionMapperWithManyTest()
        {
            using (var mapper = new InjectionMapper())
            {
                // clean all previous mappings to ensure test
                mapper.Clean<ICustomMock>();

                //InjectionMapper.Map<ITestMock1, TestMock1>(m => m.For(new TestMock1(), o => o.WithOptions(InjectionOption.WithoutOverwrite)));
                mapper.Map<ICustomMock, CustomMock>().As(() => new CustomMock());
                mapper.Map<ICustomMock, CustomMock>().As(() => new CustomMock());
            }

            using (var resolver = new InjectionResolver())
            {
                // resolve
                var map1 = resolver.ResolveMultiple<ICustomMock>();

                Assert.IsTrue(map1.Count() == 2);
            }
        }

        [Test]
        [Description("Setting a map to singleton replaces all other maps")]
        public void InjectionMapperWithOverrideTest()
        {
            using (var mapper = new InjectionMapper())
            {
                // clean all previous mappings to ensure test
                mapper.Clean<ICustomMock>();

                //InjectionMapper.Map<ITestMock1, TestMock1>(m => m.For(new TestMock1(), o => o.WithOptions(InjectionOption.WithoutOverwrite)));
                mapper.Map<ICustomMock>().For(() => new CustomMock());
                mapper.Map<ICustomMock>().For(() => new CustomMock()).WithConfiguration(InjectionFlags.OverrideAllExisting);
            }

            using (var resolver = new InjectionResolver())
            {
                // resolve
                var map1 = resolver.ResolveMultiple<ICustomMock>();

                Assert.IsTrue(map1.Count() == 1);
            }
        }

        [Test]
        [Description("Creates a map and adds a predicate that returnes the mapping")]
        public void InjectionMapperWithPredicateInMap()
        {
            using (var mapper = new InjectionMapper())
            {
                // clean all previous mappings to ensure test
                mapper.Clean<ICustomMock>();

                mapper.Map<ICustomMock>(() => new CustomMock { ID = 5 });
            }

            using (var resolver = new InjectionResolver())
            {
                // resolve
                var map = resolver.Resolve<ICustomMock>();

                Assert.IsTrue(map.ID == 5);
            }
        }

        [Test]
        [Description("Creates a map and adds a instance that returnes the mapping")]
        public void InjectionMapperWithInstanceInMap()
        {
            var mappedObject = new CustomMock { ID = 5 };

            using (var mapper = new InjectionMapper())
            {
                // clean all previous mappings to ensure test
                mapper.Clean<ICustomMock>();

                mapper.Map<ICustomMock>(mappedObject);
            }

            using (var resolver = new InjectionResolver())
            {
                // resolve
                var map = resolver.Resolve<ICustomMock>();

                Assert.AreSame(map, mappedObject);
                Assert.IsTrue(map.ID == 5);
            }
        }

        [Test]
        public void InjectionMapperWithValueCallbackTest()
        {
            using (var mapper = new InjectionMapper())
            {
                // clean all previous mappings to ensure test
                mapper.Clean<ICustomMock>();

                mapper.Map<ICustomMock>().For(() => new CustomMock());
            }

            using (var resolver = new InjectionResolver())
            {
                // resolve
                var map1 = resolver.ResolveMultiple<ICustomMock>();

                Assert.IsTrue(map1.Count() == 1);
            }
        }

        [Test]
        public void InjectionMapperWithValueCallbackInMapTest()
        {
            using (var mapper = new InjectionMapper())
            {
                // clean all previous mappings to ensure test
                mapper.Clean<ICustomMock>();

                mapper.Map<ICustomMock>(() => new CustomMock());
            }

            using (var resolver = new InjectionResolver())
            {
                // resolve
                var map1 = resolver.ResolveMultiple<ICustomMock>();

                Assert.IsTrue(map1.Count() == 1);
            }
        }

        [Test]
        public void InjectionMapperWithValueCallbackAndOptionTest()
        {
            using (var mapper = new InjectionMapper())
            {
                // clean all previous mappings to ensure test
                mapper.Clean<ICustomMock>();

                //InjectionMapper.Map<ITestMock1>().For(() => new TestMock1(), o => o.WithOptions(InjectionOption.WithOverwrite).WithOptions(InjectionOption.ResolveInstanceOnMapping));
                mapper.Map<ICustomMock>().For(() => new CustomMock()).WithConfiguration(InjectionFlags.OverrideAllExisting | InjectionFlags.ResolveValueOnMapping);
            }

            using (var resolver = new InjectionResolver())
            {
                // resolve
                var map1 = resolver.ResolveMultiple<ICustomMock>();

                Assert.IsTrue(map1.Count() == 1);
            }
        }

        #region Mocks

        public interface ICustomMock
        {
            int ID { get; set; }
        }

        public class CustomMock : ICustomMock
        {
            public CustomMock()
            {
                ID = 1;
            }

            public int ID { get; set; }
        }
        
        public interface IInjectionMapperMock1
        {
            int ID { get; }
        }

        public class InjectionMapperMock1 : IInjectionMapperMock1
        {
            public int ID
            {
                get
                {
                    return 1;
                }
            }
        }

        public interface IInjectionMapperMock2
        {
            int ID { get; }
        }

        public class InjectionMapperMock2 : IInjectionMapperMock2
        {
            public int ID
            {
                get
                {
                    return 2;
                }
            }
        }

        #endregion
    }
}
