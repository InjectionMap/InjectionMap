using NUnit.Framework;
using System;

namespace InjectionMap.Integration.UnitTests
{
    [TestFixture]
    public class PropertyInjectionTest : TestBase
    {
        [SetUp]
        public void Initialize()
        {
            // clean all previous mappings to ensure test
            Mapper.Clean<IProperty>();
            Mapper.Clean<IClassWithProperty>();
            Mapper.Clean<PropertyClass>();
            Mapper.Clean<IClassWithPlainProperty>();
            Mapper.Clean<ClassWithPlainProperty>();
        }

        [Test]
        public void PropertyInjection()
        {
            Mapper.Map<IProperty, PropertyClass>().WithArgument(() => 5);
            Mapper.Map<IClassWithProperty, ClassWithProperty>().InjectProperty(p => p.Property);

            var cls = Resolver.Resolve<IClassWithProperty>();

            Assert.IsNotNull(cls.Property);
            Assert.IsTrue(cls.Property.Value == 5);
        }

        [Test]
        public void PropertyInjectionDirectResolving()
        {
            Mapper.Map<IProperty, PropertyClass>().WithArgument(() => 5);

            var cls = Resolver.For<ClassWithProperty>().InjectProperty(p => p.Property).Resolve();

            Assert.IsNotNull(cls.Property);
            Assert.IsTrue(cls.Property.Value == 5);
        }

        [Test]
        public void PropertyInjectionWithoutMappingPropertyType_Fail()
        {
            // don't map IProperty!
            Mapper.Map<IClassWithProperty, ClassWithProperty>().InjectProperty(p => p.Property);

            var cls = Resolver.Resolve<IClassWithProperty>();

            // property is null because it can't be resolved
            Assert.IsNull(cls.Property);
        }

        [Test]
        public void PropertyInjectionWithDirectObjectInjection()
        {
            Mapper.Map<PropertyClass>().ToSelf().WithArgument(() => 5);

            var cls = Resolver.For<ClassWithPlainProperty>().InjectProperty(p => p.Property).Resolve();

            Assert.IsNotNull(cls.Property);
            Assert.IsTrue(cls.Property.Value == 5);
        }

        [Test]
        public void PropertyInjectionInExtendMap()
        {
            Mapper.Map<IProperty, PropertyClass>().WithArgument(() => 5);
            Mapper.Map<IClassWithProperty, ClassWithProperty>();
            Resolver.ExtendMap<IClassWithProperty>().InjectProperty(p => p.Property);
            //Resolver.ExtendMap<IClassWithProperty>();

            var cls = Resolver.Resolve<IClassWithProperty>();

            Assert.IsNotNull(cls.Property);
            Assert.IsTrue(cls.Property.Value == 5);
        }

        [Test]
        public void PropertyInjectionWithWrongType()
        {
            Mapper.Map<IProperty, PropertyClass>().WithArgument(() => 5);
            Mapper.Map<IClassWithPlainProperty, ClassWithPlainProperty>().InjectProperty(p => p.Property);

            var cls = Resolver.Resolve<IClassWithPlainProperty>();

            // has to be null because InjectionMap can't resolve the property of key type PropertyClass
            Assert.IsNull(cls.Property);
        }

        [Test]
        public void PropertyInjectionWithInvalidExpression()
        {
            // expression that is passed to the injectproperty is a object and not a property
            Assert.Throws<ArgumentException>(() => Mapper.Map<IClassWithProperty, ClassWithProperty>().InjectProperty(p => new { Fail = 0 }));
        }




        private interface IProperty
        {
            int Value { get; set; }
        }

        private interface IClassWithProperty
        {
            IProperty Property { get; set; }
        }

        private class PropertyClass : IProperty
        {
            public PropertyClass(int value)
            {
                Value = value;
            }

            public int Value { get; set; }
        }

        private class ClassWithProperty : IClassWithProperty
        {
            public IProperty Property { get; set; }
        }

        private interface IClassWithPlainProperty
        {
            PropertyClass Property { get; set; }
        }

        private class ClassWithPlainProperty : IClassWithPlainProperty
        {
            public PropertyClass Property { get; set; }
        }
    }
}
