using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using InjectionMap.Expressions;

namespace InjectionMap.Test.Unit
{
    /// <summary>
    /// Tests the Implementaiton of IBindingExpression
    /// </summary>
    [TestFixture]
    public class BindingExpressionTest
    {
        //
        // Tests the Implementaiton of IBindingExpression
        //

        [SetUp]
        public void Initialize()
        {
            using (var mapper = new InjectionMapper())
            {
                mapper.Clean<IBindingExpressionMap>();
                mapper.Clean<BindingExpression>();
            }
        }

        private IBindingExpression<BindingExpression> GetMap()
        {
            using (var mapper = new InjectionMapper())
            {
                return mapper.Map<IBindingExpressionMap, BindingExpression>();
            }
        }

        [Test]
        [Description("IBindingExpression<T> WithArgument<TArg>(TArg value);")]
        public void BindingExpressionWithArgumentValue()
        {
            var be = GetMap();
            var argument = new BindingArgument();
            be = be.WithArgument<BindingArgument>(argument);

            Assert.IsInstanceOf<BindingArgument>(be.Component.Arguments.First().Value);
            Assert.AreSame(be.Component.Arguments.First().Value, argument);
            Assert.AreSame(be.Component.Arguments.First().Name, null);
        }

        [Test]
        [Description("IBindingExpression<T> WithArgument<TArg>(string name, TArg value);")]
        public void BindingExpressionWithArgumentNameAndValue()
        {
            var be = GetMap();
            var argument = new BindingArgument();
            be = be.WithArgument<BindingArgument>("id", argument);

            Assert.IsInstanceOf<BindingArgument>(be.Component.Arguments.First().Value);
            Assert.AreSame(be.Component.Arguments.First().Value, argument);
            Assert.AreSame(be.Component.Arguments.First().Name, "id");
        }

        [Test]
        [Description("IBindingExpression<T> WithArgument<TArg>(Expression<Func<TArg>> predicate);")]
        public void BindingExpressionWithArgumentWithExpressionValue()
        {
            var be = GetMap();
            var argument = new BindingArgument();
            be = be.WithArgument<BindingArgument>(() => argument);

            Assert.IsInstanceOf<BindingArgument>(be.Component.Arguments.First().Callback.Compile().Invoke());
            Assert.AreSame(be.Component.Arguments.First().Callback.Compile().Invoke(), argument);
            Assert.AreSame(be.Component.Arguments.First().Name, null);
        }

        [Test]
        [Description("IBindingExpression<T> WithArgument<TArg>(string name, Expression<Func<TArg>> predicate);")]
        public void BindingExpressionWithArgumentWithNameAndExpressionValue()
        {
            var be = GetMap();
            var argument = new BindingArgument();
            be = be.WithArgument<BindingArgument>("id", () => argument);

            Assert.IsInstanceOf<BindingArgument>(be.Component.Arguments.First().Callback.Compile().Invoke());
            Assert.AreSame(be.Component.Arguments.First().Callback.Compile().Invoke(), argument);
            Assert.AreSame(be.Component.Arguments.First().Name, "id");
        }

        [Test]
        [Description("IBindingExpression<T> As(Expression<Func<T>> predicate);")]
        public void BindingExpressionAs()
        {
            var be = GetMap();
            var map = new BindingExpression();
            be = be.As(() => map);

            Assert.AreSame(be.Component.ValueCallback.Compile().Invoke(), map);
        }

        [Test]
        [Description("IBoundExpression<T> WithConfiguration(InjectionFlags option); AsConstant")]
        public void BindingExpressionWithConfigurationAsConstant()
        {
            var be = GetMap();
            var bound = be.WithConfiguration(InjectionFlags.AsConstant);

            Assert.IsTrue(bound.Component.MappingConfiguration.AsConstant);
            Assert.IsFalse(bound.Component.MappingConfiguration.AsSingleton);
            Assert.IsFalse(bound.Component.MappingConfiguration.ResolveValueOnMapping);
        }

        [Test]
        [Description("IBoundExpression<T> WithConfiguration(InjectionFlags option); AsSingleton")]
        public void BindingExpressionWithConfigurationAsSingleton()
        {
            var be = GetMap();
            var bound = be.WithConfiguration(InjectionFlags.AsSingleton);

            Assert.IsFalse(bound.Component.MappingConfiguration.AsConstant);
            Assert.IsTrue(bound.Component.MappingConfiguration.AsSingleton);
            Assert.IsFalse(bound.Component.MappingConfiguration.ResolveValueOnMapping);
        }

        [Test]
        [Description("IBoundExpression<T> WithConfiguration(InjectionFlags option); ResolveValueOnMapping")]
        public void BindingExpressionWithConfigurationResolveValueOnMapping()
        {
            var be = GetMap();
            var bound = be.WithConfiguration(InjectionFlags.ResolveValueOnMapping);

            Assert.IsTrue(bound.Component.MappingConfiguration.AsConstant);
            Assert.IsFalse(bound.Component.MappingConfiguration.AsSingleton);
            Assert.IsTrue(bound.Component.MappingConfiguration.ResolveValueOnMapping);
        }

        [Test]
        [Description("IBindingExpression<T> OnResolved(Action<T> callback);")]
        public void BindingExpressionWithArgumentOnResolved()
        {
            var be = GetMap();
            be = be.OnResolved(m => m.ID = 5);
            var map = new BindingExpression
            {
                ID = 2
            };

            be.Component.OnResolvedCallback.Invoke(map);
            
            Assert.IsTrue(map.ID == 5);
        }


        internal interface IBindingExpressionMap
        {
            int ID { get; set; }
        }

        internal class BindingExpression : IBindingExpressionMap
        {
            public BindingExpression()
            {
            }

            public BindingExpression(int id)
            {
                ID = id;
            }

            public int ID { get; set; }
        }

        internal class  BindingArgument
        {
            public int ID { get; set; }
        }
    }
}
