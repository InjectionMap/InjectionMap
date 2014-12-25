﻿using NUnit.Framework;

namespace InjectionMap.Test.Integration
{
    [TestFixture]
    public class SubstitutionTests : TestBase
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
            Assert.IsInstanceOf(typeof (OriginalSubstitute), map);

            // substitute the original with the new
            Mapper.Map<ISubstitute>().Substitute<SubstituteMock>();

            // test new substitute
            map = Resolver.Resolve<ISubstitute>();
            Assert.IsInstanceOf(typeof (SubstituteMock), map);
        }

        [Test]
        public void SubstitutionWithDelegate()
        {
            // test original
            var map = Resolver.Resolve<ISubstitute>();
            Assert.IsInstanceOf(typeof (OriginalSubstitute), map);

            // substitute the original with the new
            Mapper.Map<ISubstitute>().Substitute(() => new SubstituteMock());

            // test new substitute
            map = Resolver.Resolve<ISubstitute>();
            Assert.IsInstanceOf(typeof (SubstituteMock), map);
        }

        [Test]
        public void SubstitutionWithAsExpression()
        {
            // test original
            var map = Resolver.Resolve<ISubstitute>();
            Assert.IsInstanceOf(typeof (OriginalSubstitute), map);

            // substitute the original with the new
            Mapper.Map<ISubstitute>().Substitute<SubstituteMock>().As(() => new SubstituteMock());

            // test new substitute
            map = Resolver.Resolve<ISubstitute>();
            Assert.IsInstanceOf(typeof (SubstituteMock), map);
        }

        [Test]
        public void SubstitutionWithOverrideAsExpression()
        {
            // test original
            var map = Resolver.Resolve<ISubstitute>();
            Assert.IsInstanceOf(typeof (OriginalSubstitute), map);

            // substitute the original with the new and readd the original
            Mapper.Map<ISubstitute>().Substitute<ISubstitute>().As(() => new OriginalSubstitute());

            // test new substitute
            map = Resolver.Resolve<ISubstitute>();
            Assert.IsInstanceOf(typeof (OriginalSubstitute), map);
        }

        [Test]
        public void SubstitutionWithOptionsExpression()
        {
            // test original
            var map = Resolver.Resolve<ISubstitute>();
            Assert.IsInstanceOf(typeof (OriginalSubstitute), map);

            // substitute the original with the new
            Mapper.Map<ISubstitute>().Substitute<SubstituteMock>().WithConfiguration(InjectionFlags.OverrideAllExisting);

            // test new substitute
            map = Resolver.Resolve<ISubstitute>();
            Assert.IsInstanceOf(typeof (SubstituteMock), map);
        }

        [Test]
        public void SubstitutionWithArgumentExpression()
        {
            // test original
            var map = Resolver.Resolve<ISubstitute>();
            Assert.IsInstanceOf(typeof (OriginalSubstitute), map);

            // substitute the original with the new
            Mapper.Map<ISubstitute>().Substitute<SubstituteWithArgumentMock>().WithArgument<int>(() => 5);

            // test new substitute
            map = Resolver.Resolve<ISubstitute>();
            Assert.IsInstanceOf(typeof (SubstituteWithArgumentMock), map);
        }

        #region Mocks

        public interface ISubstitute
        {
        }

        public class OriginalSubstitute : ISubstitute
        {
        }

        public class SubstituteMock : ISubstitute
        {
        }

        public class SubstituteWithArgumentMock : ISubstitute
        {
            public SubstituteWithArgumentMock(int id)
            {
                Assert.AreEqual(id, 5);
            }
        }

        #endregion
    }
}
