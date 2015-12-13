using NUnit.Framework;

namespace InjectionMap.UnitTests.Expressions
{
    [TestFixture]
    public class BoundExpressionTest
    {
        [SetUp]
        public void Initialize()
        {
            using (var mapper = new InjectionMapper())
            {
                mapper.Clean<IBoundExpressionMock>();
            }
        }

        [Test]
        [Description("IBoundExpression<T> OnResolved(Action<T> callback);")]
        public void BoundExpressionOnResolved()
        {
            using (var mapper = new InjectionMapper())
            {
                var be = mapper.Map<IBoundExpressionMock, BoundExpression>().WithConfiguration(InjectionFlags.None);
                be = be.OnResolved(m => m.ID = 5);
                var ce = be as IComponentExpression;
                Assert.IsNotNull(ce);

                var map = new BoundExpression
                {
                    ID = 2
                };

                ce.Component.OnResolvedCallback.Invoke(map);

                Assert.IsTrue(map.ID == 5);
            }
        }


        internal interface IBoundExpressionMock
        {
            int ID { get; set; }
        }

        internal class BoundExpression : IBoundExpressionMock
        {
            public int ID { get; set; }
        }
    }
}
