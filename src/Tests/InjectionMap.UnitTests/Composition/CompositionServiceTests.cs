using InjectionMap.Composition;
using InjectionMap.Internal;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectionMap.UnitTests.Composition
{
    [TestFixture]
    public class CompositionServiceTests
    {
        [Test]
        public void CompositionServiceComposeGenericWithValueCallbackTest()
        {
            var context = new Mock<IComponentProvider>();
            context.SetupAllProperties();

            var value = new Composee();
            var component = new MappingComponent
            {
                ValueCallback = () => value,
            };

            // Act
            var composed = CompositionService.Compose<Composee>(component, context.Object);

            Assert.IsNotNull(composed);

            // Act
            var composed2 = CompositionService.Compose<Composee>(component, context.Object);

            Assert.IsNotNull(composed2);
            Assert.AreSame(composed, composed2);
        }

        [Test]
        public void CompositionServiceComposeGenericWithValueCallbackAsConstantTest()
        {
            var context = new Mock<IComponentProvider>();
            context.SetupAllProperties();

            var value = new Composee();
            var component = new MappingComponent
            {
                ValueCallback = () => value,
                MappingConfiguration = new MappingConfiguration { AsConstant = true }
            };

            // Act
            var composed = CompositionService.Compose<Composee>(component, context.Object);

            Assert.IsNotNull(composed);

            // Act
            var composed2 = CompositionService.Compose<Composee>(component, context.Object);

            Assert.IsNotNull(composed2);
            Assert.AreSame(composed, composed2);
        }

        [Test]
        public void CompositionServiceComposeGenericWithValueCallbackOnResolvedTest()
        {
            var context = new Mock<IComponentProvider>();
            context.SetupAllProperties();

            var value = new Composee();
            var component = new MappingComponent
            {
                ValueCallback = () => value
            };

            var onResolved = false;
            component.OnResolvedCallback = a => onResolved = true;

            // Act
            var composed = CompositionService.Compose<Composee>(component, context.Object);

            Assert.IsNotNull(composed);
            Assert.IsTrue(onResolved);
        }

        private class Composee
        {
        }
    }
}
