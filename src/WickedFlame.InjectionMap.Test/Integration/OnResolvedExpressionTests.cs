using NUnit.Framework;

namespace WickedFlame.InjectionMap.Test.Integration
{
    [TestFixture]
    public class OnResolvedExpressionTests : TestBase
    {
        [SetUp]
        public void InitializeTest()
        {
            Mapper.Clean<IOnResolved>();
        }

        [Test]
        public void IBoundExpressionOnResolved()
        {
            Mapper.Map<IOnResolved>().For<OnResolvedMock>().WithArgument(() => 1).WithOptions(InjectionFlags.AsSingleton).OnResolved<IOnResolved>(m => m.ID = 5);

            var map = Resolver.Resolve<IOnResolved>();
            Assert.IsTrue(map.ID == 5);
        }

        [Test]
        public void IBindingExpressionOnResolved()
        {
            Mapper.Map<OnResolvedMock>().ToSelf().OnResolved(m => m.ID = 5).WithArgument(() => 1);

            var map = Resolver.Resolve<IOnResolved>();
            Assert.IsTrue(map.ID == 5);
        }

        [Test]
        public void IBindingExpressionGenericOnResolved()
        {
            Mapper.Map<IOnResolved>().For<OnResolvedMock>().OnResolved(m => m.ID = 5).WithArgument(() => 1);

            var map = Resolver.Resolve<IOnResolved>();
            Assert.IsTrue(map.ID == 5);
        }

        [Test]
        public void IMappingExpressionGenericOnResolved()
        {
            Mapper.Map<IOnResolved>().OnResolved(m => m.ID = 5).For<OnResolvedMock>().WithArgument(() => 1);

            var map = Resolver.Resolve<IOnResolved>();
            Assert.IsTrue(map.ID == 5);
        }
    }

    public interface IOnResolved
    {
        int ID { get; set; }
    }

    public class OnResolvedMock : IOnResolved
    {
        public OnResolvedMock(int id)
        {
            ID = id;
        }

        public int ID { get; set; }
    }
}
