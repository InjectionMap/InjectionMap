using NUnit.Framework;
using InjectionMap.Test.Mock;
using InjectionMap.Exceptions;
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
        [ExpectedException(typeof(TypeCompositionException))]
        public void ConstructorWithParameters_WithoutPassingArgument()
        {
            //TODO: This test has to fail!
            // clean all previous mappings to ensure test
            Mapper.Clean<IConstructorArgumentMock>();

            // mapping
            Mapper.Map<IConstructorArgumentMock, ConstructorArgumentMock>();

            // resolve
            var map = Resolver.Resolve<IConstructorArgumentMock>();
        }

        [Test]
        [Description("Create a object that takes multiple arguments in the constructor without passing all arguments. Creates an exception")]
        [ExpectedException(typeof(TypeCompositionException))]
        public void ConstructorWithParameters_WithoutEnoughArgument()
        {
            // clean all previous mappings to ensure test
            Mapper.Clean<IConstructorArgumentMock>();

            // mapping
            Mapper.Map<IConstructorArgumentMock, ConstructorArgumentMock>().WithArgument(2);

            // resolve
            var map = Resolver.Resolve<IConstructorArgumentMock>();
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
        [ExpectedException(typeof(TypeCompositionException))]
        public void ConstructorWithParameters_WithWrongArgument()
        {
            // clean all previous mappings to ensure test
            Mapper.Clean<IConstructorArgumentMock>();

            // mapping
            Mapper.Map<IConstructorArgumentMock, ConstructorArgumentMock>().WithArgument(2m);

            // resolve
            var map = Resolver.Resolve<IConstructorArgumentMock>();
        }

        //[Test]
        //[Description("Create a object that takes multiple arguments in the constructor with partialy passing arguments where rest is nullable")]
        //[ExpectedException(typeof(TargetParameterCountException))]
        //public void ConstructorWithParameters_WithPartCorectAndPartNullableArgument()
        //{
        //    // clean all previous mappings to ensure test
        //    Mapper.Clean<IConstructorArgumentMock>();

        //    // mapping
        //    Mapper.Map<IConstructorArgumentMock, ConstructorArgumentMock>().WithArgument(2);

        //    // resolve
        //    var map = Resolver.Resolve<IConstructorArgumentMock>();
        //}
    }



    public interface IConstructorArgumentMock
    {
        string ID { get; }
    }

    public class ConstructorArgumentMock : IConstructorArgumentMock
    {
        public ConstructorArgumentMock(int id, string message)
        {
            ID = string.Format("{0} {1}", message, id);
        }

        public string ID { get; private set; }
    }

    public class ConstructorWithoutArgumentMock : IConstructorArgumentMock
    {
        public ConstructorWithoutArgumentMock()
        {
            ID = "Test without arguments";
        }

        public string ID { get; private set; }
    }
}
