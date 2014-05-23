using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace InjectionMap.Test.Unit
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

                var map = new BoundExpression
                {
                    ID = 2
                };

                be.Component.OnResolvedCallback.Invoke(map);

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
