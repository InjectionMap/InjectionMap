using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectionMap.Test.Integration
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
        public void InjectionMapperWithOverrideTest()
        {
            using (var mapper = new InjectionMapper())
            {
                // clean all previous mappings to ensure test
                mapper.Clean<ICustomMock>();

                //InjectionMapper.Map<ITestMock1, TestMock1>(m => m.For(new TestMock1(), o => o.WithOptions(InjectionOption.WithoutOverwrite)));
                mapper.Map<ICustomMock>().For(() => new CustomMock());
                mapper.Map<ICustomMock>().For(() => new CustomMock()).WithConfiguration(InjectionFlags.AsSingleton);
            }

            using (var resolver = new InjectionResolver())
            {
                // resolve
                var map1 = resolver.ResolveMultiple<ICustomMock>();

                Assert.IsTrue(map1.Count() == 1);
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
        public void InjectionMapperWithValueCallbackAndOptionTest()
        {
            using (var mapper = new InjectionMapper())
            {
                // clean all previous mappings to ensure test
                mapper.Clean<ICustomMock>();

                //InjectionMapper.Map<ITestMock1>().For(() => new TestMock1(), o => o.WithOptions(InjectionOption.WithOverwrite).WithOptions(InjectionOption.ResolveInstanceOnMapping));
                mapper.Map<ICustomMock>().For(() => new CustomMock()).WithConfiguration(InjectionFlags.AsSingleton | InjectionFlags.ResolveValueOnMapping);
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
