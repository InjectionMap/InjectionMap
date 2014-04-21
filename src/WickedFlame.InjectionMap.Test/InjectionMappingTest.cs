using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using WickedFlame.InjectionMap.Test.Mock;
using System.Linq;
using NUnit.Framework;

namespace WickedFlame.InjectionMap.Test
{
    [TestFixture]
    public class InjectionMappingTest
    {
        [Test]
        public void IInjectionMapping_InitializeTest()
        {
            using (var mapper = new InjectionMapper())
            {
                mapper.InitializeMappings(this.GetType().Assembly);
            }

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
                mapper.Map<ICustomMock>().For(() => new CustomMock()).WithOptions(InjectionFlags.WithOverwrite);
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
                mapper.Map<ICustomMock>().For(() => new CustomMock()).WithOptions(InjectionFlags.WithOverwrite | InjectionFlags.ResolveInstanceOnMapping);
            }

            using (var resolver = new InjectionResolver())
            {
                // resolve
                var map1 = resolver.ResolveMultiple<ICustomMock>();

                Assert.IsTrue(map1.Count() == 1);
            }
        }
    }
}
