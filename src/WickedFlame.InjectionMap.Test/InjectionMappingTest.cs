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
        public void InjectionMapperTest()
        {
            InjectionMapper.InitializeMappings(this.GetType().Assembly);

            var mapp1 = InjectionResolver.Resolve<IInjectionMapperMock1>();
            Assert.AreEqual(mapp1.ID, 1);

            var mapp2 = InjectionResolver.Resolve<IInjectionMapperMock2>();
            Assert.AreEqual(mapp2.ID, 2);
        }

        [Test]
        public void InjectionMapperWithValueTest()
        {
            // clean all previous mappings to ensure test
            InjectionResolver.Clean<ICustomMock>();

            //InjectionMapper.Map<ITestMock1, TestMock1>(m => m.For(new TestMock1()));
            InjectionMapper.Map<ICustomMock, CustomMock>().For(new CustomMock());
            var map1 = InjectionResolver.Resolve<ICustomMock>();

            Assert.AreEqual(map1.ID, 1);
        }

        [Test]
        public void InjectionMapperWithManyTest()
        {
            // clean all previous mappings to ensure test
            InjectionResolver.Clean<ICustomMock>();

            //InjectionMapper.Map<ITestMock1, TestMock1>(m => m.For(new TestMock1(), o => o.WithOptions(InjectionOption.WithoutOverwrite)));
            InjectionMapper.Map<ICustomMock, CustomMock>().For(new CustomMock());
            InjectionMapper.Map<ICustomMock, CustomMock>().For(new CustomMock());

            // resolve
            var map1 = InjectionResolver.ResolveMultiple<ICustomMock>();

            Assert.IsTrue(map1.Count() == 2);
        }

        [Test]
        public void InjectionMapperWithOverrideTest()
        {
            // clean all previous mappings to ensure test
            InjectionResolver.Clean<ICustomMock>();

            //InjectionMapper.Map<ITestMock1, TestMock1>(m => m.For(new TestMock1(), o => o.WithOptions(InjectionOption.WithoutOverwrite)));
            InjectionMapper.Map<ICustomMock>().For(() => new CustomMock());
            InjectionMapper.Map<ICustomMock>().For(() => new CustomMock()).WithOptions(InjectionFlags.WithOverwrite);

            // resolve
            var map1 = InjectionResolver.ResolveMultiple<ICustomMock>();

            Assert.IsTrue(map1.Count() == 1);
        }

        [Test]
        public void InjectionMapperWithValueCallbackTest()
        {
            // clean all previous mappings to ensure test
            InjectionResolver.Clean<ICustomMock>();

            InjectionMapper.Map<ICustomMock>().For(() => new CustomMock());

            // resolve
            var map1 = InjectionResolver.ResolveMultiple<ICustomMock>();

            Assert.IsTrue(map1.Count() == 1);
        }

        [Test]
        public void InjectionMapperWithValueCallbackAndOptionTest()
        {
            // clean all previous mappings to ensure test
            InjectionResolver.Clean<ICustomMock>();

            //InjectionMapper.Map<ITestMock1>().For(() => new TestMock1(), o => o.WithOptions(InjectionOption.WithOverwrite).WithOptions(InjectionOption.ResolveInstanceOnMapping));
            InjectionMapper.Map<ICustomMock>().For(() => new CustomMock()).WithOptions(InjectionFlags.WithOverwrite | InjectionFlags.ResolveInstanceOnMapping);

            // resolve
            var map1 = InjectionResolver.ResolveMultiple<ICustomMock>();

            Assert.IsTrue(map1.Count() == 1);
        }
    }
}
