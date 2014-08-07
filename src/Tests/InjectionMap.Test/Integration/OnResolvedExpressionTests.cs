using NUnit.Framework;

namespace InjectionMap.Test.Integration
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
        [Description("Tests for IBoundExpression.OnResolved")]
        public void IBoundExpressionOnResolved()
        {
            Mapper.Map<IOnResolved>().For<OnResolvedMock>().WithArgument(() => 1).WithConfiguration(InjectionFlags.AsSingleton).OnResolved(m => m.ID = 5);

            var map = Resolver.Resolve<IOnResolved>();
            Assert.IsTrue(map.ID == 5);
        }

        [Test]
        [Description("Tests for IMappingExpression.OnResolved with ToSelf mapping")]
        public void IMappingExpressionOnResolved()
        {
            Mapper.Map<OnResolvedMock>().ToSelf().OnResolved(m => m.ID = 5).WithArgument(() => 1);

            var map = Resolver.Resolve<OnResolvedMock>();
            Assert.IsTrue(map.ID == 5);
        }

        [Test]
        [Description("Tests for IMappingExpression.OnResolved after the For statement")]
        public void IBindingExpressionGenericOnResolved_1()
        {
            Mapper.Map<IOnResolved>().For<OnResolvedMock>().OnResolved(m => m.ID = 5).WithArgument(() => 1);

            var map = Resolver.Resolve<IOnResolved>();
            Assert.IsTrue(map.ID == 5);
        }

        [Test]
        [Description("Tests for IMappingExpression.OnResolved before the For statement")]
        public void IMappingExpressionGenericOnResolved_2()
        {
            Mapper.Map<IOnResolved>().OnResolved(m => m.ID = 5).For<OnResolvedMock>().WithArgument(() => 1);

            var map = Resolver.Resolve<IOnResolved>();
            Assert.IsTrue(map.ID == 5);
        }

        [Test]
        [Description("Tests for IBindingExpression.OnResolved")]
        public void IBindingExpressionGenericOnResolved()
        {
            Mapper.Map<IOnResolved, OnResolvedMock>().OnResolved(m => m.ID = 5).WithArgument(() => 1);
            
            var map = Resolver.Resolve<IOnResolved>();
            Assert.IsTrue(map.ID == 5);
        }

        #region Mocks

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

        #endregion
    }
}
