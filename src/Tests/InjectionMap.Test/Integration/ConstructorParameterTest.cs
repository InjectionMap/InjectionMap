﻿using NUnit.Framework;

namespace InjectionMap.Test.Integration
{
    [TestFixture]
    public class ConstructorParameterTest : TestBase
    {
        [SetUp]
        public void Initialize()
        {
            Mapper.Clean<IConstuctorParameter>();
            Mapper.Clean<IMixedConstuctor>();
        }

        [Test]
        [Description("Composes a mapping with a constructor that has one parameter to compose and one passed as argument")]
        public void ConstuctParameterWithMixedConstuctor()
        {
            // create mapping
            Mapper.Map<IConstuctorParameter, ConstructorParameter>();
            Mapper.Map<IMixedConstuctor, MixedConstuctor>().WithArgument(() => 3);

            // resolve
            var map = Resolver.Resolve<IMixedConstuctor>();

            // assert
            Assert.AreEqual(map.ID, 5);
        }

        [Test]
        [Description("Composes a mapping with a constructor that has one parameter to compose and one passed as argument and a constuctor that only takes a argument. It should take the one with the argument.")]
        public void ConstuctParameterWithMixedWitTwoConstuctor()
        {
            // create mapping
            Mapper.Map<IConstuctorParameter, ConstructorParameter>();
            Mapper.Map<IMixedConstuctor, MixedWitTwoConstuctor>().WithArgument(() => 4);

            // resolve
            var map = Resolver.Resolve<IMixedConstuctor>();

            // assert
            Assert.AreEqual(map.ID, 4);
        }

        [Test]
        [Description("Composes a mapping with a constructor that has one parameter to compose and one passed as argument and a constructor that only takes a argument and a defaultconstructor. It should take the one with the argument.")]
        public void ConstuctParameterWithMixedWitDefaultConstuctor()
        {
            // create mapping
            Mapper.Map<IConstuctorParameter, ConstructorParameter>();
            Mapper.Map<IMixedConstuctor, MixedWitDefaultConstuctor>().WithArgument(() => 5);

            // resolve
            var map = Resolver.Resolve<IMixedConstuctor>();

            // assert
            Assert.AreEqual(map.ID, 5);
        }

        [Test]
        [Description("Composes a mapping with a constructor that has one parameter to compose and one passed as argument marked as InjectionConstructor and a constructor that only takes a argument and a defaultconstructor. It should take the one with the InjectionConstructor.")]
        public void ConstuctParameterWithMixedWitMarkedConstuctor()
        {
            // create mapping
            Mapper.Map<IConstuctorParameter, ConstructorParameter>();
            Mapper.Map<IMixedConstuctor, MixedWitMarkedConstuctor>().WithArgument(() => 6);

            // resolve
            var map = Resolver.Resolve<IMixedConstuctor>();

            // assert
            Assert.AreEqual(map.ID, 8);
        }

        [Test]
        [Description("Composes a mapping with a constructor that has one parameter to compose")]
        public void ConstuctParameterWithOnlyMapedParameter()
        {
            // create mapping
            Mapper.Map<IConstuctorParameter, ConstructorParameter>();
            Mapper.Map<IMixedConstuctor, WitOnlyMappableParameter>();

            // resolve
            var map = Resolver.Resolve<IMixedConstuctor>();

            // assert
            Assert.AreEqual(map.ID, 2);
        }

        [Test]
        [Description("Composes a mapping with a constructor that has one parameter to compose and a argument that is not used")]
        public void ConstuctParameterWithOnlyMapedParameterAndArgument()
        {
            // create mapping
            Mapper.Map<IConstuctorParameter, ConstructorParameter>();
            Mapper.Map<IMixedConstuctor, WitOnlyMappableParameter>().WithArgument(() => 5);

            // resolve
            var map = Resolver.Resolve<IMixedConstuctor>();

            // assert
            Assert.AreEqual(map.ID, 2);
        }

        [Test]
        public void MappingConstructorSelection()
        {
            Mapper.Map<IConstuctorParameter, ConstructorParameter>();
            Mapper.Map<IMixedConstuctor, MixedWitDefaultConstuctor>()
                .ForConstructor(cc => cc[2])
                .WithArgument(() => 2);

            var map = Resolver.Resolve<IMixedConstuctor>();

            Assert.AreEqual(map.ID, 4);
        }

        [Test]
        public void MappingConstructorSelectionWithEditingArgumentsByIndex()
        {
            Mapper.Map<IConstuctorParameter, ConstructorParameter>();
            Mapper.Map<IMixedConstuctor, MixedWitDefaultConstuctor>()
                .ForConstructor(cc =>
                {
                    var constr = cc[2];

                    constr[1].Value = 2;

                    return constr;
                });

            var map = Resolver.Resolve<IMixedConstuctor>();

            Assert.AreEqual(map.ID, 4);
        }

        [Test]
        public void MappingConstructorSelectionWithEditingArgumentsByName()
        {
            Mapper.Map<IConstuctorParameter, ConstructorParameter>();
            Mapper.Map<IMixedConstuctor, MixedWitDefaultConstuctor>()
                .ForConstructor(cc =>
                {
                    var constr = cc[2];

                    constr["value"].Value = 2;

                    return constr;
                });

            var map = Resolver.Resolve<IMixedConstuctor>();

            Assert.AreEqual(map.ID, 4);
        }



        [Test]
        public void ResolvingConstructorSelection()
        {
            Mapper.Map<IConstuctorParameter, ConstructorParameter>();
            Mapper.Map<IMixedConstuctor, MixedWitDefaultConstuctor>();

            var map = Resolver.For<IMixedConstuctor>()
                .ForConstructor(cc => cc[2])
                .WithArgument(() => 2)
                .Resolve();

            Assert.AreEqual(map.ID, 4);
        }

        [Test]
        public void ResolvingConstructorSelectionWithEditingArgumentsByIndex()
        {
            Mapper.Map<IConstuctorParameter, ConstructorParameter>();
            Mapper.Map<IMixedConstuctor, MixedWitDefaultConstuctor>();

            var map = Resolver.For<IMixedConstuctor>()
                .ForConstructor(cc =>
                {
                    var constr = cc[2];

                    constr[1].Value = 2;

                    return constr;
                })
                .Resolve();

            Assert.AreEqual(map.ID, 4);
        }

        [Test]
        public void ResolvingConstructorSelectionWithEditingArgumentsByName()
        {
            Mapper.Map<IConstuctorParameter, ConstructorParameter>();
            Mapper.Map<IMixedConstuctor, MixedWitDefaultConstuctor>();

            var map = Resolver.For<IMixedConstuctor>()
                .ForConstructor(cc =>
                {
                    var constr = cc[2];

                    constr["value"].Value = 2;

                    return constr;
                })
                .Resolve();

            Assert.AreEqual(map.ID, 4);
        }



        #region Mocks

        internal interface IConstuctorParameter
        {
            int ID { get; }
        }


        internal class ConstructorParameter : IConstuctorParameter
        {
            public int ID
            {
                get
                {
                    return 2;
                }
            }
        }

        internal interface IMixedConstuctor
        {
            int ID { get; }
        }

        internal class MixedConstuctor : IMixedConstuctor
        {
            public MixedConstuctor(IConstuctorParameter cp, int value)
            {
                ID = cp.ID + value;
            }

            public int ID { get; private set; }
        }

        internal class MixedWitTwoConstuctor : IMixedConstuctor
        {
            public MixedWitTwoConstuctor(int id)
            {
                ID = id;
            }

            public MixedWitTwoConstuctor(IConstuctorParameter cp, int value)
            {
                ID = cp.ID + value;
            }

            public int ID { get; private set; }
        }

        internal class MixedWitDefaultConstuctor : IMixedConstuctor
        {
            public MixedWitDefaultConstuctor()
            {
                ID = 1;
            }

            public MixedWitDefaultConstuctor(int id)
            {
                ID = id;
            }

            public MixedWitDefaultConstuctor(IConstuctorParameter cp, int value)
            {
                ID = cp.ID + value;
            }

            public int ID { get; private set; }
        }

        internal class MixedWitMarkedConstuctor : IMixedConstuctor
        {
            public MixedWitMarkedConstuctor()
            {
                ID = 1;
            }

            public MixedWitMarkedConstuctor(int id)
            {
                ID = id;
            }

            [InjectionConstructor]
            public MixedWitMarkedConstuctor(IConstuctorParameter cp, int value)
            {
                ID = cp.ID + value;
            }

            public int ID { get; private set; }
        }

        internal class WitOnlyMappableParameter : IMixedConstuctor
        {
            public WitOnlyMappableParameter(IConstuctorParameter cp)
            {
                ID = cp.ID;
            }

            public int ID { get; private set; }
        }

        #endregion
    }
}
