using NUnit.Framework;

namespace InjectionMap.Test.Integration
{
    [TestFixture]
    public class IMappingExpressionExtensionsTest : TestBase
    {
        [SetUp]
        public void Initialize()
        {
            Mapper.Clean<IMappingExpressionExtension>();
        }

        [Test]
        [Description("Creates a mapping to the default container with the Extenisonmethods")]
        public void MappingExpressionExtensionWithDefaultContainer()
        {
            // create map
            var obj = new MappingExpressionExtensionMock(1);
            obj.MapTo<IMappingExpressionExtension>();

            // resolve
            var map = Resolver.Resolve<IMappingExpressionExtension>();

            Assert.AreSame(obj, map);
        }

        [Test]
        [Description("Creates a mapping to the default container with the Extenisonmethods that fails")]
        public void MappingExpressionExtensionWithDefaultContainer_Fail()
        {
            // create map
            var obj = new NonMappingExpressionExtensionMock(1);
            Assert.Throws<MappingMismatchException>(() => obj.MapTo<IMappingExpressionExtension>());
        }

        [Test]
        [Description("Creates a mapping to a custom container with the Extenisonmethods")]
        public void MappingExpressionExtensionWithCustomContainer()
        {
            var container = new MappingContext();

            // create map
            var obj = new MappingExpressionExtensionMock(1);
            obj.MapTo<IMappingExpressionExtension>(container);

            // resolve
            var map = Resolver.Resolve<IMappingExpressionExtension>();

            Assert.IsNull(map);

            var cr = new InjectionResolver(container);
            map = cr.Resolve<IMappingExpressionExtension>();

            Assert.AreSame(obj, map);
        }

        [Test]
        [Description("Creates a mapping to a custom container with the Extenisonmethods that fails")]
        public void MappingExpressionExtensionWithCustomContainer_Fail()
        {
            var container = new MappingContext();

            // create map
            var obj = new NonMappingExpressionExtensionMock(1);
            Assert.Throws<MappingMismatchException>(() => obj.MapTo<IMappingExpressionExtension>(container));
        }

        #region Mocks

        internal interface IMappingExpressionExtension
        {
            int ID { get; }
        }

        internal class MappingExpressionExtensionMock : IMappingExpressionExtension
        {
            public MappingExpressionExtensionMock(int id)
            {
                ID = id;
            }

            public int ID { get; internal set; }
        }

        internal class NonMappingExpressionExtensionMock
        {
            public NonMappingExpressionExtensionMock(int id)
            {
                ID = id;
            }

            public int ID { get; internal set; }
        }

        #endregion
    }
}
