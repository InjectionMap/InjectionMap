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
    }
}
