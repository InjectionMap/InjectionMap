using NUnit.Framework;
using InjectionMap.Test.Mock;
using System.Reflection;

namespace InjectionMap.Test.Integration
{
    [TestFixture]
    public class ConstructorWithArgumentsTest : TestBase
    {
        [Test]
        [Description("Create a object that takes multiple arguments in the constructor and pass the argument using the injection with naming the arguments")]
        public void ConstructorWithArgumentsWithName()
        {
            Mapper.Clean<IConstructorArgumentMock>();

            // mapping
            Mapper.Map<IConstructorArgumentMock, ConstructorArgumentMock>().WithArgument("message", () => "Number").WithArgument("id", 1);

            // resolve
            var map = Resolver.Resolve<IConstructorArgumentMock>();

            Assert.IsTrue(map.ID == "Number 1");
        }


        [Test]
        [Description("Create a object that takes multiple arguments in the constructor and pass the argument using the injection without naming the arguments")]
        public void ConstructorWithArgumentsWithoutName()
        {
            // clean all previous mappings to ensure test
            Mapper.Clean<IConstructorArgumentMock>();

            // mapping
            Mapper.Map<IConstructorArgumentMock, ConstructorArgumentMock>().WithArgument(2).WithArgument(() => "Number");

            // resolve
            var map = Resolver.Resolve<IConstructorArgumentMock>();

            Assert.IsTrue(map.ID == "Number 2");
        }

        [Test]
        [Description("Create a object that takes multiple arguments in the constructor without passing any arguments. Creates an exception")]
        public void ConstructorWithParameters_WithoutPassingArgument()
        {
            //TODO: This test has to fail!
            // clean all previous mappings to ensure test
            Mapper.Clean<IConstructorArgumentMock>();

            // mapping
            Mapper.Map<IConstructorArgumentMock, ConstructorArgumentMock>();

            // resolve
            Assert.Throws<TypeCompositionException>(() => Resolver.Resolve<IConstructorArgumentMock>());
        }

        [Test]
        [Description("Create a object that takes multiple arguments in the constructor without passing all arguments. Creates an exception")]
        public void ConstructorWithParameters_WithoutEnoughArgument()
        {
            // clean all previous mappings to ensure test
            Mapper.Clean<IConstructorArgumentMock>();

            // mapping
            Mapper.Map<IConstructorArgumentMock, ConstructorArgumentMock>().WithArgument(2);

            // resolve
            Assert.Throws<TypeCompositionException>(() => Resolver.Resolve<IConstructorArgumentMock>());
        }

        [Test]
        [Description("Create a object that takes no arguments in the constructor and an arguments. Resolving has to work")]
        public void ConstructorWithoutParameters_PassingOneArgument()
        {
            //TODO: This test has to fail!
            // clean all previous mappings to ensure test
            Mapper.Clean<IConstructorArgumentMock>();

            // mapping
            Mapper.Map<IConstructorArgumentMock, ConstructorWithoutArgumentMock>().WithArgument(2);

            // resolve
            var map = Resolver.Resolve<IConstructorArgumentMock>();

            Assert.IsInstanceOf<ConstructorWithoutArgumentMock>(map);
        }

        [Test]
        [Description("Create a object that takes multiple arguments in the constructor with passing wrong arguments. Creates an exception")]
        public void ConstructorWithParameters_WithWrongArgument()
        {
            // clean all previous mappings to ensure test
            Mapper.Clean<IConstructorArgumentMock>();

            // mapping
            Mapper.Map<IConstructorArgumentMock, ConstructorArgumentMock>().WithArgument(2m);

            // resolve
            Assert.Throws<TypeCompositionException>(() => Resolver.Resolve<IConstructorArgumentMock>());
        }

        [Test]
        public void ConstructorWithParameterInResolver()
        {
            Mapper.Clean<IConstructorArgumentMock>();
            Mapper.Map<IConstructorArgumentMock,ConstructorArgumentMock>();

            var map = Resolver.Resolve<IConstructorArgumentMock>(() => 2, () => "Number");
            Assert.IsTrue(map.ID == "Number 2");

            map = Resolver.Resolve<IConstructorArgumentMock>(() => "Number", () => 3);
            Assert.IsTrue(map.ID == "Number 3");

            map = Resolver.Resolve<IConstructorArgumentMock>(() => "Number", () => 4, () => "test", () => new ConstructorArgument());
            Assert.IsTrue(map.ID == "Number 4");
        }

        [Test]
        public void ConstructorWithParameterInResolverWithCustomContext()
        {
            var context = new MappingContext();

            using (var mapper = new InjectionMapper(context))
            {
                mapper.Clean<IConstructorArgumentMock>();
                mapper.Map<IConstructorArgumentMock, ConstructorArgumentMock>();
            }

            var map = Resolver.Resolve<IConstructorArgumentMock>(context, () => 2, () => "Number");
            Assert.IsTrue(map.ID == "Number 2");

            map = Resolver.Resolve<IConstructorArgumentMock>(context, () => "Number", () => 3);
            Assert.IsTrue(map.ID == "Number 3");

            map = Resolver.Resolve<IConstructorArgumentMock>(context, () => "Number", () => 4, () => "test", () => new ConstructorArgument());
            Assert.IsTrue(map.ID == "Number 4");
        }
    }
}
