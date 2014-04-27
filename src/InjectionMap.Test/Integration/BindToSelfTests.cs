using NUnit.Framework;

namespace InjectionMap.Test.Integration
{
    [TestFixture]
    public class BindToSelfTests : TestBase
    {
        [SetUp]
        public void Initialize()
        {
            // clean all previous mappings to ensure test
            Mapper.Clean<BindToSelfMock>();
        }

        [Test]
        public void OnBindToSelfTest()
        {
            // mapping
            Mapper.Map<BindToSelfMock>().ToSelf();

            // resolve
            var map = Resolver.Resolve<BindToSelfMock>();

            Assert.IsNotNull(map);
            Assert.IsTrue(map.ID == 1);
        }
    }

    public class BindToSelfMock
    {
        public BindToSelfMock()
        {
            ID = 1;
        }

        public int ID { get; private set; }
    }


}
