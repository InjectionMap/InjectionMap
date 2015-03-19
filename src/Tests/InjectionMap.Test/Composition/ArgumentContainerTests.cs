using InjectionMap.Composition;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectionMap.UnitTests.Composition
{
    [TestFixture]
    public class ArgumentContainerTests
    {
        [Test]
        public void ArgumentContainerPushArgumentTest()
        {
            var container = new ArgumentContainer(null);
            var argument = new Argument
            {
                Name = "Arg",
                Value = 1
            };


            // Act
            var ok = container.PushArgument(argument);

            Assert.IsTrue(ok);
            Assert.IsTrue(container.Parameters.Count() == 1);
            Assert.AreSame(container.Parameters.First(), argument);
        }

        [Test]
        public void ArgumentContainerPushArgumentTwiceTest()
        {
            var container = new ArgumentContainer(null);
            var argument = new Argument
            {
                Name = "Arg",
                Value = 1
            };


            // Act
            var ok = container.PushArgument(argument);

            Assert.IsTrue(ok);

            // Act
            ok = container.PushArgument(argument);

            Assert.IsFalse(ok);

            Assert.IsTrue(container.Parameters.Count() == 1);
            Assert.AreSame(container.Parameters.First(), argument);
        }

        [Test]
        public void ArgumentContainerIsArgumentInUseTest()
        {
            var container = new ArgumentContainer(null);
            var argument = new Argument
            {
                Name = "Arg",
                Value = 1
            };
            
            // Act
            container.PushArgument(argument);
            var inUse = container.IsArgumentInUse(argument.Value);
            
            Assert.IsTrue(inUse);
            
            // Act
            inUse = container.IsArgumentInUse("test");

            Assert.IsFalse(inUse);

            // Act
            inUse = container.IsArgumentInUse(2);

            Assert.IsFalse(inUse);            
        }

        private class Argument : IArgument
        {
            public string Name { get; set; }

            public object Value { get; set; }
        }
    }
}
