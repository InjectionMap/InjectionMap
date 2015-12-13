using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectionMap.Integration.UnitTests
{
    /// <summary>
    /// This testclass tests deep and nested dependenc trees.
    /// </summary>
    [TestFixture]
    public class DeepDependencyTreeTests
    {
        [Test]
        public void DeepDependencyTree_ResolveByInterface_DependenciesAreSame_Test()
        {
            var context = new MappingContext();
            using (var mapper = new InjectionMapper(context))
            {
                mapper.Map<ITreeOne, TreeOne>();
                mapper.Map<ITreeOneDependencyOne, TreeOneDependencyOne>();
                mapper.Map<ITreeOneDependencyTwo, TreeOneDependencyTwo>();
                mapper.Map<ITreeTwoDependencyOne, TreeTwoDependencOne>().WithConfiguration(InjectionFlags.Singleton);
                mapper.Map<ITreeTwoDependencyTwo, TreeTwoDependencyTwo>();
            }

            using (var resolver = new InjectionResolver(context))
            {
                var tree = resolver.Resolve<ITreeOne>();

                Assert.IsNotNull(tree.DependencyOne.DependencyTwo.TreeTwo);
                Assert.IsNotNull(tree.DependencyOne.TreeTwo);
                Assert.AreSame(tree.DependencyOne.DependencyTwo.TreeTwo, tree.DependencyOne.TreeTwo);
            }
        }

        [Test]
        public void DeepDependencyTree_ResolveByInterface_NestedObjectsAreSame_Test()
        {
            var context = new MappingContext();
            using (var mapper = new InjectionMapper(context))
            {
                mapper.Map<ITreeOne, TreeOne>();
                mapper.Map<ITreeOneDependencyOne, TreeOneDependencyOne>();
                mapper.Map<ITreeOneDependencyTwo, TreeOneDependencyTwo>();
                mapper.Map<ITreeTwoDependencyOne, TreeTwoDependencOne>().WithConfiguration(InjectionFlags.Singleton);
                mapper.Map<ITreeTwoDependencyTwo, TreeTwoDependencyTwo>();
            }

            using (var resolver = new InjectionResolver(context))
            {
                var tree = resolver.Resolve<ITreeOne>();
                
                Assert.IsNotNull(tree.DependencyOne.DependencyTwo.TreeTwo.TreeTwoOne);
                Assert.IsNotNull(tree.DependencyOne.TreeTwo.TreeTwoOne);
                Assert.AreSame(tree.DependencyOne.DependencyTwo.TreeTwo.TreeTwoOne, tree.DependencyOne.TreeTwo.TreeTwoOne);
            }
        }

        private interface ITreeOne { ITreeOneDependencyOne DependencyOne { get; } }

        private interface ITreeOneDependencyOne
        {
            ITreeOneDependencyTwo DependencyTwo { get; }

            ITreeTwoDependencyOne TreeTwo { get; }
        }

        private interface ITreeOneDependencyTwo { ITreeTwoDependencyOne TreeTwo { get; }  }

        private interface ITreeTwoDependencyOne { ITreeTwoDependencyTwo TreeTwoOne { get; } }

        private interface ITreeTwoDependencyTwo { }

        private class TreeOne : ITreeOne
        {
            public TreeOne(ITreeOneDependencyOne dependency)
            {
                DependencyOne = dependency;
            }

            public ITreeOneDependencyOne DependencyOne { get; }
        }

        private class TreeOneDependencyOne : ITreeOneDependencyOne
        {
            public TreeOneDependencyOne(ITreeOneDependencyTwo dependency, ITreeTwoDependencyOne two)
            {
                DependencyTwo = dependency;
                TreeTwo = two;
            }

            public ITreeOneDependencyTwo DependencyTwo { get; }

            public ITreeTwoDependencyOne TreeTwo { get; }
        }

        private class TreeOneDependencyTwo : ITreeOneDependencyTwo
        {
            public TreeOneDependencyTwo(ITreeTwoDependencyOne dependency)
            {
                TreeTwo = dependency;
            }

            public ITreeTwoDependencyOne TreeTwo { get; }
        }

        private class TreeTwoDependencOne : ITreeTwoDependencyOne
        {
            public TreeTwoDependencOne(ITreeTwoDependencyTwo dependency)
            {
                TreeTwoOne = dependency;
            }

            public ITreeTwoDependencyTwo TreeTwoOne { get; }
        }

        private class TreeTwoDependencyTwo : ITreeTwoDependencyTwo
        {
        }
    }
}
