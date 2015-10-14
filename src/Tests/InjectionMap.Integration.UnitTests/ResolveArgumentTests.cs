using NUnit.Framework;

namespace InjectionMap.Integration.UnitTests
{
    [TestFixture]
    public class ResolveArgumentTests : TestBase
    {
        [SetUp]
        public void Initialize()
        {
            Mapper.Clean<ITopConstuctor>();
        }
        
        [Test]
        public void ResolveArgumentTests_AddArgumentForSubItemTest()
        {
            Mapper.Map<ITopConstuctor, TopConstuctor>();
            Mapper.Map<ConstructorParameter>().ToSelf().WithArgument(() => "tmp");

            var top = Resolver.For<ITopConstuctor>()
                .WithArgument(() => "Test value")
                .Resolve();

            Assert.AreSame("tmp", top.Value);
        }

        [Test]
        public void ResolveArgumentTests_ResolveConcreteInSubargumentTest()
        {
            Mapper.Map<ITopConstuctor, TopConstuctor2>();

            var top = Resolver.Resolve<ITopConstuctor>();

            Assert.AreSame("Empty Constructor Parameter", top.Value);
        }

        #region Mocks

        internal class ConstructorParameter
        {
            public ConstructorParameter(string value)
            {
                Value = value;
            }

            public string Value { get; set; }
        }

        internal class EmptyConstructorParameter
        {
            public EmptyConstructorParameter()
            {
                Value = "Empty Constructor Parameter";
            }

            public string Value { get; set; }
        }

        internal interface ITopConstuctor
        {
            string Value { get; }
        }

        internal class TopConstuctor : ITopConstuctor
        {
            ConstructorParameter _cp;

            public TopConstuctor(ConstructorParameter cp)
            {
                _cp = cp;
            }

            public string Value
            {
                get
                {
                    return _cp.Value;
                }
            }
        }

        internal class SubItem
        {
            readonly EmptyConstructorParameter _empty;

            public SubItem(EmptyConstructorParameter empty)
            {
                _empty = empty;
            }

            public string Value
            {
                get
                {
                    return _empty.Value;
                }
            }
        }

        internal class TopConstuctor2 : ITopConstuctor
        {
            readonly SubItem _cp;

            public TopConstuctor2(SubItem cp)
            {
                _cp = cp;
            }

            public string Value
            {
                get
                {
                    return _cp.Value;
                }
            }
        }

        #endregion
    }
}
