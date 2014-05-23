using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectionMap.Test.Unit
{
    [TestFixture]
    public class ResolverExpressionTest
    {
        //[Ignore]
        [Test]
        [Description("Creates a map that allready containes arguments. Overrides an argument by supplying the same name")]
        public void AddArgumentByNameThatExists()
        {
            var assembly = System.Reflection.Assembly.GetAssembly(this.GetType()).FullName;
            //var expression = new ResolverExpression(
            if (string.IsNullOrEmpty(assembly))
            {
            }
        }



        [Test]
        [Description("IResolverExpression<T> WithArgument<TArg>(TArg value);")]
        public void ResolverExpressionWithArgumentValue()
        {
            Assert.Fail();
        }

        [Test]
        [Description("IResolverExpression<T> WithArgument<TArg>(string name, TArg value);")]
        public void ResolverExpressionWithArgumentNameAndValue()
        {
            Assert.Fail();
        }

        [Test]
        [Description("IResolverExpression<T> WithArgument<TArg>(Expression<Func<TArg>> predicate);")]
        public void ResolverExpressionWithArgumentValueExpression()
        {
            Assert.Fail();
        }

        [Test]
        [Description("IResolverExpression<T> WithArgument<TArg>(string name, Expression<Func<TArg>> predicate);")]
        public void ResolverExpressionWithArgumentNameAndValueExpression()
        {
            Assert.Fail();
        }

        [Test]
        [Description("T Resolve();")]
        public void ResolverExpressionResolve()
        {
            Assert.Fail();
        }
    }
}
