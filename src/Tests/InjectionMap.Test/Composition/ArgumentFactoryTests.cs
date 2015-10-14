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
    public class ArgumentFactoryTests
    {
        [Test]
        public void ArgumentFactoryCtxTest()
        {
            var context = new Mock<IComponentProvider>();
            context.SetupAllProperties();

            var ctor = typeof(ArgumentFactoryTestItem).GetConstructors().First();

            var factory = new ArgumentFactory(new ArgumentContainer(ctor), context.Object);

        }

        [Test]
        public void ArgumentFactoryCreateArgument()
        {
            var context = new Mock<IComponentProvider>();
            context.SetupAllProperties();

            var component = new Mock<IMappingComponent>();
            component.SetupAllProperties();
            component.Setup(c => c.Arguments).Returns(() => new List<IBindingArgument>
            {
                new BindingArgument<string>
                {
                    Name = "parameter2",
                    Value= "test parameter"
                }
            });

            var ctor = typeof(ArgumentFactoryTestItem).GetConstructors().First();
            var factory = new ArgumentFactory(component.Object, new ArgumentContainer(ctor), context.Object);
            

            var arguments = typeof(ArgumentFactoryTestItem).GetConstructors().First().GetParameters();

            // Act
            var argument = factory.CreateArgument(arguments.FirstOrDefault(a => a.Name == "parameter2"));

            Assert.IsNotNull(argument);
            Assert.AreEqual(argument.Value, "test parameter");
        }

        [Test]
        public void ArgumentFactoryCreateArgumentWithoutMappingComponent()
        {
            var context = new Mock<IComponentProvider>();
            context.SetupAllProperties();

            var ctor = typeof(ArgumentFactoryTestItem).GetConstructors().First();
            var factory = new ArgumentFactory(new ArgumentContainer(ctor), context.Object);

            var arguments = typeof(ArgumentFactoryTestItem).GetConstructors().First().GetParameters();

            // Act
            //var argument = factory.CreateArgument(arguments.FirstOrDefault(a => a.Name == "parameter2"));

            //Assert.IsNull(argument);

            // the type string can not be constructed/instantiated
            Assert.Throws<TypeCompositionException>(() => factory.CreateArgument(arguments.FirstOrDefault(a => a.Name == "parameter2")));
        }

        [Test]
        public void ArgumentFactoryCreateArgumentWithCallback()
        {
            var context = new Mock<IComponentProvider>();
            context.SetupAllProperties();

            var component = new Mock<IMappingComponent>();
            component.SetupAllProperties();
            component.Setup(c => c.Arguments).Returns(() => new List<IBindingArgument>
            {
                new BindingArgument<string>
                {
                    Name = "parameter2",
                    Callback = () => "test parameter"
                }
            });

            var ctor = typeof(ArgumentFactoryTestItem).GetConstructors().First();
            var factory = new ArgumentFactory(component.Object, new ArgumentContainer(ctor), context.Object);


            var arguments = typeof(ArgumentFactoryTestItem).GetConstructors().First().GetParameters();

            // Act
            var argument = factory.CreateArgument(arguments.FirstOrDefault(a => a.Name == "parameter2"));

            Assert.IsNotNull(argument);
            Assert.AreEqual(argument.Value, "test parameter");
        }

        [Test]
        public void ArgumentFactoryCreateArgumentWithCallbackAndValue()
        {
            var context = new Mock<IComponentProvider>();
            context.SetupAllProperties();

            var component = new Mock<IMappingComponent>();
            component.SetupAllProperties();
            component.Setup(c => c.Arguments).Returns(() => new List<IBindingArgument>
            {
                new BindingArgument<string>
                {
                    Name = "parameter2",
                    Value = "value",
                    Callback = () => "test parameter"
                }
            });

            var ctor = typeof(ArgumentFactoryTestItem).GetConstructors().First();
            var factory = new ArgumentFactory(component.Object, new ArgumentContainer(ctor), context.Object);


            var arguments = typeof(ArgumentFactoryTestItem).GetConstructors().First().GetParameters();

            // Act
            var argument = factory.CreateArgument(arguments.FirstOrDefault(a => a.Name == "parameter2"));

            Assert.IsNotNull(argument);
            Assert.AreEqual(argument.Value, "test parameter");
        }

        [Test]
        public void ArgumentFactoryCreateArgumentWithoutName()
        {
            var context = new Mock<IComponentProvider>();
            context.SetupAllProperties();

            var component = new Mock<IMappingComponent>();
            component.SetupAllProperties();
            component.Setup(c => c.Arguments).Returns(() => new List<IBindingArgument>
            {
                new BindingArgument<string>
                {
                    Value = "value",
                    Callback = () => "test parameter"
                }
            });

            var ctor = typeof(ArgumentFactoryTestItem).GetConstructors().First();
            var factory = new ArgumentFactory(component.Object, new ArgumentContainer(ctor), context.Object);


            var arguments = typeof(ArgumentFactoryTestItem).GetConstructors().First().GetParameters();

            // Act
            var argument = factory.CreateArgument(arguments.FirstOrDefault(a => a.Name == "parameter2"));

            Assert.IsNotNull(argument);
            Assert.AreEqual(argument.Value, "test parameter");
        }

        private class ArgumentFactoryTestItem
        {
            public ArgumentFactoryTestItem(int parameter1, string parameter2, DateTime parameter3)
            {
            }
        }
    }
}
