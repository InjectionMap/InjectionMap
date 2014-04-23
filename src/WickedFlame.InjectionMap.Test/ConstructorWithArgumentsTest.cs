using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WickedFlame.InjectionMap.Test.Mock;

namespace WickedFlame.InjectionMap.Test
{
    [TestFixture]
    public class ConstructorWithArgumentsTest : TestBase
    {
        //[Test]
        //[Description("Create a object that takes one argument in the constructor and pass the argument using the injection")]
        //[Ignore()]
        //public void ConstructorWithOneArgument()
        //{
        //}


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
        [Ignore()]
        public void ConstructorWithParameters_WithoutPassingArgument()
        {
            //TODO: This test has to fail!
        }

        [Test]
        [Ignore()]
        public void ConstructorWithoutParameters_PassingOneArgument()
        {
            //TODO: This test has to fail!
        }
    }
}
