using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using WickedFlame.InjectionMap.Substitution;

namespace WickedFlame.InjectionMap.Test
{
    [TestFixture]
    public class SubstitutionTest : TestBase
    {
        [SetUp]
        public void Initialize()
        {
            // clean all older mappings
            Mapper.Clean<ISubstitute>();

            // create new mapping
            Mapper.Map<ISubstitute, OriginalSubstitute>();
        }

        [Test]
        public void SimpleSubstitution()
        {
            // test original
            var map = Resolver.Resolve<ISubstitute>();
            Assert.IsInstanceOf(typeof(OriginalSubstitute), map);

            // substitute the original with the new
            Mapper.Map<ISubstitute>().Substitute<SubstituteMock>();

            // test new substitute
            map = Resolver.Resolve<ISubstitute>();
            Assert.IsInstanceOf(typeof(SubstituteMock), map);
        }

        [Test]
        public void SubstitutionWithDelegate()
        {
            // test original
            var map = Resolver.Resolve<ISubstitute>();
            Assert.IsInstanceOf(typeof(OriginalSubstitute), map);

            // substitute the original with the new
            Mapper.Map<ISubstitute>().Substitute(() => new SubstituteMock());

            // test new substitute
            map = Resolver.Resolve<ISubstitute>();
            Assert.IsInstanceOf(typeof(SubstituteMock), map);
        }
    }

    public interface ISubstitute
    {
    }

    public class OriginalSubstitute : ISubstitute
    {
    }

    public class SubstituteMock : ISubstitute
    {
    }
}
