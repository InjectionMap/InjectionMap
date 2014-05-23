using InjectionMap.Expressions;
using NUnit.Framework;

namespace InjectionMap.Test.Unit
{
    /// <summary>
    /// Tests the Implementaiton of IMappingExpression
    /// </summary>
    [TestFixture]
    public class MappingExpressionTest
    {
        //
        // Tests the Implementaiton of IMappingExpression
        //

        [SetUp]
        public void Initialize()
        {
            using (var mapper = new InjectionMapper())
            {
                mapper.Clean<IMappingExpressionMap>();
                mapper.Clean<MappingExpression>();
            }
        }

        private IMappingExpression<IMappingExpressionMap> GetMap()
        {
            using (var mapper = new InjectionMapper())
            {
                return mapper.Map<IMappingExpressionMap>();
            }
        }

        [Test]
        public void MapFor()
        {
            var me = GetMap();
            Assert.IsTrue(me.Component.ValueType == typeof(IMappingExpressionMap));

            var be = me.For<MappingExpression>();

            Assert.IsTrue(be.Component.ValueType == typeof(MappingExpression));
            Assert.IsNull(be.Component.ValueCallback);
        }

        [Test]
        public void MapForWithValue()
        {
            var me = GetMap();
            Assert.IsTrue(me.Component.ValueType == typeof(IMappingExpressionMap));

            var mock = new MappingExpression();
            var be = me.For<MappingExpression>(mock);

            Assert.IsTrue(be.Component.ValueType == typeof(MappingExpression));
            Assert.IsNotNull(be.Component.ValueCallback);
            Assert.AreSame(be.Component.ValueCallback.Compile().Invoke(), mock);
        }

        [Test]
        public void MapForWithFunc()
        {
            var me = GetMap();
            Assert.IsTrue(me.Component.ValueType == typeof(IMappingExpressionMap));

            var mock = new MappingExpression();
            var be = me.For(() => mock);

            Assert.IsTrue(be.Component.ValueType == typeof(MappingExpression));
            Assert.IsNotNull(be.Component.ValueCallback);
            Assert.AreSame(be.Component.ValueCallback.Compile().Invoke(), mock);
        }

        [Test]
        public void MapToSelf()
        {
            using (var mapper = new InjectionMapper())
            {
                var me = mapper.Map<MappingExpression>();
                Assert.IsTrue(me.Component.ValueType == typeof(MappingExpression));

                var be = me.ToSelf();
                Assert.IsTrue(be.Component.ValueType == typeof(MappingExpression));
            }
        }

        [Test]
        public void MapOnResolved()
        {
            using (var mapper = new InjectionMapper())
            {
                var me = mapper.Map<IMappingExpressionMap, MappingExpression>();
                var be = me.OnResolved(e => e.ID = 5);
                var mock = new MappingExpression();
                be.Component.OnResolvedCallback.Invoke(mock);

                Assert.IsTrue(be.Component.ValueType == typeof(MappingExpression));
                Assert.IsTrue((mock as IMappingExpressionMap).ID == 5);
            }
        }

        [Test]
        public void MapSubstitute()
        {
            var me = GetMap();
            Assert.IsTrue(me.Component.ValueType == typeof(IMappingExpressionMap));

            var be = me.Substitute<MappingExpression>();

            Assert.IsTrue(be.Component.ValueType == typeof(MappingExpression));
            Assert.IsTrue(be.Component.IsSubstitute);
        }

        [Test]
        public void MapSubstituteWithFunc()
        {
            var me = GetMap();
            Assert.IsTrue(me.Component.ValueType == typeof(IMappingExpressionMap));

            var mock = new MappingExpression();
            var be = me.Substitute(() => mock);

            Assert.AreSame(be.Component.ValueCallback.Compile().Invoke(), mock);
            Assert.IsTrue(be.Component.IsSubstitute);
        }




        internal interface IMappingExpressionMap
        {
            int ID { get; set; }
        }

        internal class MappingExpression : IMappingExpressionMap
        {
            public int ID { get; set; }
        }
    }
}
