using NUnit.Framework;
using InjectionMap.Integration.UnitTests.Mock;

namespace InjectionMap.Integration.UnitTests
{
    [TestFixture]
    public class InjectionConstructorTests
    {
        [SetUp]
        public void Init()
        {
        }

        [Test]
        [Description("Resolves a instance with a constructor marked as InjectionConstructor with one parameter")]
        public void ConstructorInjection_WithOneParameterTest()
        {
            using (var mapper = new InjectionMapper())
            {
                // clean all previous mappings to ensure test
                mapper.Clean<IConstructorParameter>();
                mapper.Clean<IConstructorInjectionMock>();

                // mapping
                mapper.Map<IConstructorParameter, ConstructorParameter>();
                mapper.Map<IConstructorInjectionMock, ConstructorInjectionMock>();
            }

            using (var resolver = new InjectionResolver())
            {
                // resolve
                var map = resolver.Resolve<IConstructorInjectionMock>();

                Assert.IsTrue(map.ID == 2);
            }
        }

        [Test]
        [Description("Resolves a instance with a constructor marked as InjectionConstructor with two parameters")]
        public void ConstructorInjection_WithTwoParametersTest()
        {
            using (var mapper = new InjectionMapper())
            {
                // clean all previous mappings to ensure test
                mapper.Clean<IConstructorParameter>();
                mapper.Clean<IConstructorInjectionMock>();
                mapper.Clean<ISecondConstructorInjectionMock>();

                // mapping
                mapper.Map<IConstructorParameter, ConstructorParameter>();
                mapper.Map<IConstructorInjectionMock, ConstructorInjectionMock>();
                mapper.Map<ISecondConstructorInjectionMock, SecondConstructorInjectionMock>();
            }

            using (var resolver = new InjectionResolver())
            {
                // resolve
                var map = resolver.Resolve<ISecondConstructorInjectionMock>();

                Assert.IsTrue(map.ID == 4);

                map = resolver.Resolve<ISecondConstructorInjectionMock>();

                Assert.IsTrue(map.ID == 4);
            }
        }

        [Test]
        [Description("Resolves a instance with a constructor marked as InjectionConstructor with three parameters")]
        public void ConstructorInjection_WithThreeParametersTest()
        {
            using (var mapper = new InjectionMapper())
            {
                // clean all previous mappings to ensure test
                mapper.Clean<IConstructorParameter>();
                mapper.Clean<IConstructorInjectionMock>();
                mapper.Clean<ISecondConstructorInjectionMock>();
                mapper.Clean<ThirdConstructorInjectionMock>();

                // mapping
                mapper.Map<IConstructorParameter, ConstructorParameter>();
                mapper.Map<IConstructorInjectionMock, ConstructorInjectionMock>();
                mapper.Map<ISecondConstructorInjectionMock, SecondConstructorInjectionMock>();
                mapper.Map<IThirdConstructorInjectionMock, ThirdConstructorInjectionMock>();
            }

            using (var resolver = new InjectionResolver())
            {
                // resolve
                var map = resolver.Resolve<IThirdConstructorInjectionMock>();

                Assert.IsTrue(map.ID == 8);
            }
        }
    }
}
