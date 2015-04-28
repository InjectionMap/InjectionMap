using InjectionMap.Internal;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectionMap.UnitTests
{
    [TestFixture]
    public class MappingContextTests
    {
        [Test]
        public void MappingContextCreate()
        {
            var context = new MappingContext();

            Assert.IsNotNull(context.Components);
        }

        [Test]
        public void MappingContextCreateWithStringName()
        {
            var context = new MappingContext("TestContext");

            Assert.IsNotNull(context.Components);
        }

        [Test]
        public void MappingContextCreateWithNUll()
        {
            var context = new MappingContext((string)null);

            Assert.IsNotNull(context.Components);
        }

        [Test]
        public void MappingContextCreateWithEmptyString()
        {
            var context = new MappingContext("");

            Assert.IsNotNull(context.Components);
        }

        [Test]
        public void MappingContextResolveFromNamedContext()
        {
            var context = new MappingContext("TestContext");
            context.Add(new MappingComponent());
            context.Add(new MappingComponent());

            var context2 = new MappingContext("TestContext");
            Assert.IsTrue(context2.Components.Count() == 2);
        }

        [Test]
        public void MappingContextResolveFromNContextUll()
        {
            var context = new MappingContext((string)null);
            context.Add(new MappingComponent());
            context.Add(new MappingComponent());

            var context2 = new MappingContext((string)null);
            Assert.IsFalse(context2.Components.Any());
        }
    }
}
