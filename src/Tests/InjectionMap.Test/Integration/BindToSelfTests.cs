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
            Mapper.Clean<BindToSelfMockWithArguments>();
        }

        [Test]
        public void OnBindToSelfTest()
        {
            // mapping
            Mapper.Map<BindToSelfMock>().ToSelf();

            // resolve
            var map = Resolver.Resolve<BindToSelfMock>();

            Assert.IsNotNull(map);
            Assert.IsInstanceOf<BindToSelfMock>(map);
            Assert.IsTrue(map.ID == 1);
        }

        [Test]
        public void OnBindToSelfWithArguments()
        {
            // mapping
            Mapper.Map<BindToSelfMockWithArguments>().ToSelf().WithArgument(() => 2);

            // resolve
            var map = Resolver.Resolve<BindToSelfMockWithArguments>();

            Assert.IsNotNull(map);
            Assert.IsInstanceOf<BindToSelfMockWithArguments>(map);
            Assert.IsTrue(map.ID == 2);
        }

        [Test]
        public void OnBindToSelfWithArgumentsAndAs()
        {
            // mapping
            Mapper.Map<BindToSelfMockWithArguments>().ToSelf().WithArgument(() => 1).As(() => new BindToSelfMockWithArguments(2));

            // resolve
            var map = Resolver.Resolve<BindToSelfMockWithArguments>();

            Assert.IsNotNull(map);
            Assert.IsInstanceOf<BindToSelfMockWithArguments>(map);
            Assert.IsTrue(map.ID == 2);
        }

        #region Mocks

        private class BindToSelfMock
        {
            public BindToSelfMock()
            {
                ID = 1;
            }

            public int ID { get; private set; }
        }

        private class BindToSelfMockWithArguments
        {
            public BindToSelfMockWithArguments(int id)
            {
                ID = id;
            }

            public int ID { get; private set; }
        }

        #endregion
    }
}
