using System;
using InjectionMap.Test.Mock;
using System.Linq;
using NUnit.Framework;

namespace InjectionMap.Test.Integration
{
    [TestFixture]
    public class MapInitializerTests
    {
        [SetUp]
        public void Initialize()
        {
            var mapper = new InjectionMapper();
            mapper.Clean<IInjectionMapperMock1>();
            mapper.Clean<IInjectionMapperMock2>();
        }

        [Test]
        public void MapInitializer_InitializeWithAssembly()
        {
            InjectionMapper.Initialize(this.GetType().Assembly);

            using (var resolver = new InjectionResolver())
            {
                var mapp1 = resolver.Resolve<IInjectionMapperMock1>();
                Assert.AreEqual(mapp1.ID, 1);

                var mapp2 = resolver.Resolve<IInjectionMapperMock2>();
                Assert.AreEqual(mapp2.ID, 2);
            }
        }

        [Test]
        public void MapInitializer_InitializeWithDirectInjectionMappingCall()
        {
            InjectionMapper.Initialize(new InjectionMapperMock());

            using (var resolver = new InjectionResolver())
            {
                var mapp1 = resolver.Resolve<IInjectionMapperMock1>();
                Assert.AreEqual(mapp1.ID, 1);

                var mapp2 = resolver.Resolve<IInjectionMapperMock2>();
                Assert.AreEqual(mapp2.ID, 2);
            }
        }

        [Test]
        public void MapInitializer_InitializeByType()
        {
            InjectionMapper.Initialize<InjectionMapperMock>();

            using (var resolver = new InjectionResolver())
            {
                var mapp1 = resolver.Resolve<IInjectionMapperMock1>();
                Assert.AreEqual(mapp1.ID, 1);

                var mapp2 = resolver.Resolve<IInjectionMapperMock2>();
                Assert.AreEqual(mapp2.ID, 2);
            }
        }

        [Test]
        public void MapInitializer_InitializeByAssemblyName()
        {

            var fullAssemblyName = this.GetType().Assembly.FullName;
            InjectionMapper.Initialize(fullAssemblyName);
            //InjectionMapper.Initialize("InjectionMap.Test.dll");


            //InjectionMapper.Initialize("InjectionMap.Test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            //InjectionMapper.Initialize("InjectionMap.Test, Version=1.0.0.0, Culture=neutral");
            //InjectionMapper.Initialize("InjectionMap.Test, Version=1.0.0.0");


            using (var resolver = new InjectionResolver())
            {
                var mapp1 = resolver.Resolve<IInjectionMapperMock1>();
                Assert.AreEqual(mapp1.ID, 1);

                var mapp2 = resolver.Resolve<IInjectionMapperMock2>();
                Assert.AreEqual(mapp2.ID, 2);
            }
        }







        [Test]
        public void MapInitializer_InitializeWithAssembly_WithMappingProvider()
        {
            var container = new MappingContext();
            InjectionMapper.Initialize(this.GetType().Assembly, container);

            // get mappings from default container
            using (var resolver = new InjectionResolver())
            {
                var mapp1 = resolver.Resolve<IInjectionMapperMock1>();
                Assert.IsNull(mapp1);

                var mapp2 = resolver.Resolve<IInjectionMapperMock2>();
                Assert.IsNull(mapp2);
            }

            // get mappings from custom container
            using (var resolver = new InjectionResolver(container))
            {
                var mapp1 = resolver.Resolve<IInjectionMapperMock1>();
                Assert.AreEqual(mapp1.ID, 1);

                var mapp2 = resolver.Resolve<IInjectionMapperMock2>();
                Assert.AreEqual(mapp2.ID, 2);
            }
        }

        [Test]
        public void MapInitializer_InitializeWithDirectInjectionMappingCall_WithMappingProvider()
        {
            var container = new MappingContext();
            InjectionMapper.Initialize(new InjectionMapperMock(), container);

            // get mappings from default container
            using (var resolver = new InjectionResolver())
            {
                var mapp1 = resolver.Resolve<IInjectionMapperMock1>();
                Assert.IsNull(mapp1);

                var mapp2 = resolver.Resolve<IInjectionMapperMock2>();
                Assert.IsNull(mapp2);
            }

            // get mappings from custom container
            using (var resolver = new InjectionResolver(container))
            {
                var mapp1 = resolver.Resolve<IInjectionMapperMock1>();
                Assert.AreEqual(mapp1.ID, 1);

                var mapp2 = resolver.Resolve<IInjectionMapperMock2>();
                Assert.AreEqual(mapp2.ID, 2);
            }
        }

        [Test]
        public void MapInitializer_InitializeByType_WithMappingProvider()
        {
            var container = new MappingContext();
            InjectionMapper.Initialize<InjectionMapperMock>(container);

            // get mappings from default container
            using (var resolver = new InjectionResolver())
            {
                var mapp1 = resolver.Resolve<IInjectionMapperMock1>();
                Assert.IsNull(mapp1);

                var mapp2 = resolver.Resolve<IInjectionMapperMock2>();
                Assert.IsNull(mapp2);
            }

            // get mappings from custom container
            using (var resolver = new InjectionResolver(container))
            {
                var mapp1 = resolver.Resolve<IInjectionMapperMock1>();
                Assert.AreEqual(mapp1.ID, 1);

                var mapp2 = resolver.Resolve<IInjectionMapperMock2>();
                Assert.AreEqual(mapp2.ID, 2);
            }
        }

        [Test]
        public void MapInitializer_InitializeByAssemblyName_WithMappingProvider()
        {
            var container = new MappingContext();
            InjectionMapper.Initialize(this.GetType().Assembly.FullName, container);
            //InjectionMapper.Initialize("InjectionMap.Test.dll");

            // get mappings from default container
            using (var resolver = new InjectionResolver())
            {
                var mapp1 = resolver.Resolve<IInjectionMapperMock1>();
                Assert.IsNull(mapp1);

                var mapp2 = resolver.Resolve<IInjectionMapperMock2>();
                Assert.IsNull(mapp2);
            }

            // get mappings from custom container
            using (var resolver = new InjectionResolver(container))
            {
                var mapp1 = resolver.Resolve<IInjectionMapperMock1>();
                Assert.AreEqual(mapp1.ID, 1);

                var mapp2 = resolver.Resolve<IInjectionMapperMock2>();
                Assert.AreEqual(mapp2.ID, 2);
            }
        }
        
        #region Mocks

        public interface ICustomMock
        {
            int ID { get; set; }
        }

        public class CustomMock : ICustomMock
        {
            public CustomMock()
            {
                ID = 1;
            }

            public int ID { get; set; }
        }

        private class InjectionMapperMock : IMapInitializer
        {
            public void InitializeMap(IMappingProvider container)
            {
                container.Map<IInjectionMapperMock1, InjectionMapperMock1>();
                container.Map<IInjectionMapperMock2>().For<InjectionMapperMock2>();
            }
        }

        public interface IInjectionMapperMock1
        {
            int ID { get; }
        }

        public class InjectionMapperMock1 : IInjectionMapperMock1
        {
            public int ID
            {
                get
                {
                    return 1;
                }
            }
        }

        public interface IInjectionMapperMock2
        {
            int ID { get; }
        }

        public class InjectionMapperMock2 : IInjectionMapperMock2
        {
            public int ID
            {
                get
                {
                    return 2;
                }
            }
        }

        #endregion
    }
}
