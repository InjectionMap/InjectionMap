using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectionMap.Integration.UnitTests
{
    [TestFixture]
    public class MappingContextTests
    {
        [SetUp]
        public void Initialize()
        {
            using (var mapper = new InjectionMapper())
            {
                mapper.Clean<ICustomMock>();
            }

            using (var mapper = new InjectionMapper("namedContext"))
            {
                mapper.Clean<ICustomMock>();
            }

            using (var mapper = new InjectionMapper("unmappedContext"))
            {
                mapper.Clean<ICustomMock>();
            }
        }

        [Test]
        [Description("Map to a custom named context. Make shure the mapping is not stored in the default context and can be retrieved")]
        public void MappingContext_CustomContext()
        {
            var customContext = new MappingContext();
            var namedContext = new MappingContext("namedContext");

            var defaultObject = new CustomMock();
            var customObject = new CustomMock
            {
                ID = 2
            };
            var namedObject = new CustomMock
            {
                ID = 3
            };

            using (var mapper = new InjectionMapper())
            {
                mapper.Map<ICustomMock>(() => defaultObject);
            }

            using (var mapper = new InjectionMapper(customContext))
            {
                mapper.Map<ICustomMock>(() => customObject);
            }

            using (var mapper = new InjectionMapper(namedContext))
            {
                mapper.Map<ICustomMock>(() => namedObject);
            }


            using (var resolver = new InjectionResolver())
            {
                var map = resolver.ResolveMultiple<ICustomMock>();

                // make sure there is only 1 map
                Assert.IsTrue(map.Count() == 1);
                Assert.AreSame(map.First(), defaultObject);
                Assert.IsTrue(map.First().ID == 1);
            }

            using (var resolver = new InjectionResolver(customContext))
            {
                var map = resolver.ResolveMultiple<ICustomMock>();

                // make sure there is only 1 map
                Assert.IsTrue(map.Count() == 1);
                Assert.AreSame(map.First(), customObject);
                Assert.IsTrue(map.First().ID == 2);
            }

            // this has to return the same map as with the namedContext
            var namedContextForResolver = new MappingContext("namedContext");
            using (var resolver = new InjectionResolver(namedContextForResolver))
            {
                var map = resolver.ResolveMultiple<ICustomMock>();

                // make sure there is only 1 map
                Assert.IsTrue(map.Count() == 1);
                Assert.AreSame(map.First(), namedObject);
                Assert.IsTrue(map.First().ID == 3);
            }

            // this has to return the same map as with the namedContext
            using (var resolver = new InjectionResolver("namedContext"))
            {
                var map = resolver.ResolveMultiple<ICustomMock>();

                // make sure there is only 1 map
                Assert.IsTrue(map.Count() == 1);
                Assert.AreSame(map.First(), namedObject);
                Assert.IsTrue(map.First().ID == 3);
            }
        }

        [Test]
        [Description("Map to a custom named context. Make shure the mapping is not stored in the default context and can be retrieved")]
        public void MappingContext_CustomContextInInjectionMapper()
        {
            var namedObject = new CustomMock
            {
                ID = 3
            };

            using (var mapper = new InjectionMapper("namedContext"))
            {
                mapper.Map<ICustomMock>(() => namedObject);
            }
            
            using (var resolver = new InjectionResolver())
            {
                var map = resolver.ResolveMultiple<ICustomMock>();

                // make sure there is no map in default context
                Assert.IsTrue(map.Count() == 0);
            }
            
            // this has to return the same map as with the namedContext
            var namedContextForResolver = new MappingContext("namedContext");
            using (var resolver = new InjectionResolver(namedContextForResolver))
            {
                var map = resolver.ResolveMultiple<ICustomMock>();

                // make sure there is only 1 map
                Assert.IsTrue(map.Count() == 1);
                Assert.AreSame(map.First(), namedObject);
            }

            // this has to return the same map as with the namedContext
            using (var resolver = new InjectionResolver("namedContext"))
            {
                var map = resolver.ResolveMultiple<ICustomMock>();

                // make sure there is only 1 map
                Assert.IsTrue(map.Count() == 1);
                Assert.AreSame(map.First(), namedObject);
            }


            // resolving with different name should contain no mappings
            using (var resolver = new InjectionResolver("unmappedContext"))
            {
                var map = resolver.ResolveMultiple<ICustomMock>();

                Assert.IsTrue(map.Count() == 0);
            }

            var unnamedContextForResolver = new MappingContext("unmappedContext");
            using (var resolver = new InjectionResolver(unnamedContextForResolver))
            {
                var map = resolver.ResolveMultiple<ICustomMock>();

                Assert.IsTrue(map.Count() == 0);
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

        #endregion
    }
}
