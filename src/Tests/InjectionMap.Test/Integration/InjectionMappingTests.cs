using System;
using InjectionMap.Test.Mock;
using System.Linq;
using NUnit.Framework;

namespace InjectionMap.Test.Integration
{
    [TestFixture]
    public class InjectionMappingTests
    {
        [SetUp]
        public void Initialize()
        {
            var mapper = new InjectionMapper();
            mapper.Clean<IInjectionMapperMock1>();
            mapper.Clean<IInjectionMapperMock2>();
        }

        [Test]
        public void IInjectionMapping_InitializeWithAssembly()
        {
            InjectionMapper.Initialize(this.GetType().Assembly);

            using (var resolver = new InjectionResolver())
            {
                var mapp1 = resolver.Resolve<IInjectionMapperMock1>();
                Assert.AreEqual(mapp1.ID, 1);

                var mapp2 = resolver.Resolve<IInjectionMapperMock2>();
                Assert.AreEqual(mapp2.ID, 2);
            }
        }

        [Test]
        public void IInjectionMapping_InitializeWithDirectInjectionMappingCall()
        {
            InjectionMapper.Initialize(new InjectionMapperMock());

            using (var resolver = new InjectionResolver())
            {
                var mapp1 = resolver.Resolve<IInjectionMapperMock1>();
                Assert.AreEqual(mapp1.ID, 1);

                var mapp2 = resolver.Resolve<IInjectionMapperMock2>();
                Assert.AreEqual(mapp2.ID, 2);
            }
        }

        [Test]
        public void IInjectionMapping_InitializeByType()
        {
            InjectionMapper.Initialize<InjectionMapperMock>();

            using (var resolver = new InjectionResolver())
            {
                var mapp1 = resolver.Resolve<IInjectionMapperMock1>();
                Assert.AreEqual(mapp1.ID, 1);

                var mapp2 = resolver.Resolve<IInjectionMapperMock2>();
                Assert.AreEqual(mapp2.ID, 2);
            }
        }

        [Test]
        public void IInjectionMapping_InitializeByAssemblyName()
        {

            var fullAssemblyName = this.GetType().Assembly.FullName;
            InjectionMapper.Initialize(fullAssemblyName);
            //InjectionMapper.Initialize("InjectionMap.Test.dll");


            //InjectionMapper.Initialize("InjectionMap.Test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            //InjectionMapper.Initialize("InjectionMap.Test, Version=1.0.0.0, Culture=neutral");
            //InjectionMapper.Initialize("InjectionMap.Test, Version=1.0.0.0");


            using (var resolver = new InjectionResolver())
            {
                var mapp1 = resolver.Resolve<IInjectionMapperMock1>();
                Assert.AreEqual(mapp1.ID, 1);

                var mapp2 = resolver.Resolve<IInjectionMapperMock2>();
                Assert.AreEqual(mapp2.ID, 2);
            }
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

        private class InjectionMapperMock : IMapInitializer
        {
            public void InitializeMap(IMappingProvider container)
            {
                container.Map<IInjectionMapperMock1, InjectionMapperMock1>();
                container.Map<IInjectionMapperMock2>().For<InjectionMapperMock2>();
            }
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
