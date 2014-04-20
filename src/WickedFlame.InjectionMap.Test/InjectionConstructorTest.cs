using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WickedFlame.InjectionMap.Test.Mock;

namespace WickedFlame.InjectionMap.Test
{
    [TestFixture]
    public class InjectionConstructorTest
    {
        [SetUp]
        public void Init()
        {
        }

        [Test]
        [Description("Resolves a instance with a constructor marked as InjectionConstructor with one parameter")]
        public void ConstructorInjection_WithOneParameterTest()
        {
            // clean all previous mappings to ensure test
            InjectionResolver.Clean<IConstructorParameter>();
            InjectionResolver.Clean<IConstructorInjectionMock>();

            // mapping
            InjectionMapper.Map<IConstructorParameter, ConstructorParameter>();
            InjectionMapper.Map<IConstructorInjectionMock, ConstructorInjectionMock>();

            // resolve
            var map = InjectionResolver.Resolve<IConstructorInjectionMock>();

            Assert.IsTrue(map.ID == 2);
        }

        [Test]
        [Description("Resolves a instance with a constructor marked as InjectionConstructor with two parameters")]
        public void ConstructorInjection_WithTwoParametersTest()
        {
            // clean all previous mappings to ensure test
            InjectionResolver.Clean<IConstructorParameter>();
            InjectionResolver.Clean<IConstructorInjectionMock>();
            InjectionResolver.Clean<ISecondConstructorInjectionMock>();

            // mapping
            InjectionMapper.Map<IConstructorParameter, ConstructorParameter>();
            InjectionMapper.Map<IConstructorInjectionMock, ConstructorInjectionMock>();
            InjectionMapper.Map<ISecondConstructorInjectionMock, SecondConstructorInjectionMock>();

            // resolve
            var map = InjectionResolver.Resolve<ISecondConstructorInjectionMock>();
            
            Assert.IsTrue(map.ID == 4);

            map = InjectionResolver.Resolve<ISecondConstructorInjectionMock>();

            Assert.IsTrue(map.ID == 4);
        }

        [Test]
        [Description("Resolves a instance with a constructor marked as InjectionConstructor with three parameters")]
        public void ConstructorInjection_WithThreeParametersTest()
        {
            // clean all previous mappings to ensure test
            InjectionResolver.Clean<IConstructorParameter>();
            InjectionResolver.Clean<IConstructorInjectionMock>();
            InjectionResolver.Clean<ISecondConstructorInjectionMock>();
            InjectionResolver.Clean<ThirdConstructorInjectionMock>();

            // mapping
            InjectionMapper.Map<IConstructorParameter, ConstructorParameter>();
            InjectionMapper.Map<IConstructorInjectionMock, ConstructorInjectionMock>();
            InjectionMapper.Map<ISecondConstructorInjectionMock, SecondConstructorInjectionMock>();
            InjectionMapper.Map<IThirdConstructorInjectionMock, ThirdConstructorInjectionMock>();

            // resolve
            var map = InjectionResolver.Resolve<IThirdConstructorInjectionMock>();
            
            Assert.IsTrue(map.ID == 8);
        }

        [Test]
        [Description("Resolves a instance without a constructor marked as InjectionConstructor with one parameter")]
        [Ignore("Not complete")]
        public void ConstructorWithOneParameter_WithoutAttributeTest()
        {
        }

        [Test]
        [Description("Resolves a instance without a constructor marked as InjectionConstructor with two parameters")]
        [Ignore("Not complete")]
        public void ConstructorWithTwoParameters_WithoutAttributeTest()
        {
        }

        [Test]
        [Description("Resolves a instance without a constructor marked as InjectionConstructor with multiple constructors")]
        [Ignore("Not complete")]
        public void MultipleConstructors_WithoutAttribute_WithoutDefaultTest()
        {
        }
    }
}
