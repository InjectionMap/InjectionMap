using NUnit.Framework;

namespace InjectionMap.UnitTests.Expressions
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
            var ce = me as IComponentExpression;
            Assert.IsNotNull(ce);
            Assert.IsTrue(ce.Component.ValueType == typeof(IMappingExpressionMap));

            var be = me.For<MappingExpression>();
            Assert.IsNotNull(be);

            ce = be as IComponentExpression;
            Assert.IsNotNull(ce);
            Assert.IsTrue(ce.Component.ValueType == typeof(MappingExpression));
            Assert.IsNull(ce.Component.ValueCallback);
        }

        [Test]
        public void MapForWithValue()
        {
            var me = GetMap();
            var ce = me as IComponentExpression;
            Assert.IsNotNull(ce);
            Assert.IsTrue(ce.Component.ValueType == typeof(IMappingExpressionMap));

            var mock = new MappingExpression();
            var be = me.For<MappingExpression>(mock);
            Assert.IsNotNull(be);

            ce = be as IComponentExpression;
            Assert.IsNotNull(ce);
            Assert.IsTrue(ce.Component.ValueType == typeof(MappingExpression));
            Assert.IsNotNull(ce.Component.ValueCallback);
            Assert.AreSame(ce.Component.ValueCallback.Compile().Invoke(), mock);
        }

        [Test]
        public void MapForWithFunc()
        {
            var me = GetMap();
            var ce = me as IComponentExpression;
            Assert.IsNotNull(ce);
            Assert.IsTrue(ce.Component.ValueType == typeof(IMappingExpressionMap));

            var mock = new MappingExpression();
            var be = me.For(() => mock);
            Assert.IsNotNull(be);

            ce = be as IComponentExpression;
            Assert.IsNotNull(ce);
            Assert.IsTrue(ce.Component.ValueType == typeof(MappingExpression));
            Assert.IsNotNull(ce.Component.ValueCallback);
            Assert.AreSame(ce.Component.ValueCallback.Compile().Invoke(), mock);
        }

        [Test]
        public void MapToSelf()
        {
            using (var mapper = new InjectionMapper())
            {
                var me = mapper.Map<MappingExpression>();
                var ce = me as IComponentExpression;
                Assert.IsNotNull(ce);
                Assert.IsTrue(ce.Component.ValueType == typeof(MappingExpression));

                var be = me.ToSelf();
                Assert.IsNotNull(be);

                ce = be as IComponentExpression;
                Assert.IsNotNull(ce);
                Assert.IsTrue(ce.Component.ValueType == typeof(MappingExpression));
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
                var ce = be as IComponentExpression;
                Assert.IsNotNull(ce);
                ce.Component.OnResolvedCallback.Invoke(mock);

                Assert.IsTrue(ce.Component.ValueType == typeof(MappingExpression));
                Assert.IsTrue((mock as IMappingExpressionMap).ID == 5);
            }
        }

        [Test]
        public void MapSubstitute()
        {
            var me = GetMap();
            var ce = me as IComponentExpression;
            Assert.IsNotNull(ce);
            Assert.IsTrue(ce.Component.ValueType == typeof(IMappingExpressionMap));

            var be = me.Substitute<MappingExpression>();
            Assert.IsNotNull(be);

            ce = be as IComponentExpression;
            Assert.IsNotNull(ce);
            Assert.IsTrue(ce.Component.ValueType == typeof(MappingExpression));
            Assert.IsTrue(ce.Component.IsSubstitute);
        }

        [Test]
        public void MapSubstituteWithFunc()
        {
            var me = GetMap();
            var ce = me as IComponentExpression;
            Assert.IsNotNull(ce);
            Assert.IsTrue(ce.Component.ValueType == typeof(IMappingExpressionMap));

            var mock = new MappingExpression();
            var be = me.Substitute(() => mock);
            Assert.IsNotNull(be);

            ce = be as IComponentExpression;
            Assert.IsNotNull(ce);
            Assert.AreSame(ce.Component.ValueCallback.Compile().Invoke(), mock);
            Assert.IsTrue(ce.Component.IsSubstitute);
        }

        #region Mocks

        internal interface IMappingExpressionMap
        {
            int ID { get; set; }
        }

        internal class MappingExpression : IMappingExpressionMap
        {
            public int ID { get; set; }
        }

        #endregion
    }
}
