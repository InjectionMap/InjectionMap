using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Diagnostics;

namespace InjectionMap.Integration.UnitTests.Multithreading
{
    [TestFixture]
    public class MultithreadTests
    {
        private const int TestCount = 100;

        [Test]
        public void MultithreadedCallToMultipleContainers()
        {
            var objs = new IThreadedObject[]
            {
                new ThreadedObject(), new ThreadedObject(), new ThreadedObject(), new ThreadedObject(), new ThreadedObject(),
                new ThreadedObject(), new ThreadedObject(), new ThreadedObject(), new ThreadedObject(), new ThreadedObject(),
                new ThreadedObject(), new ThreadedObject(), new ThreadedObject(), new ThreadedObject(), new ThreadedObject(),
                new ThreadedObject(), new ThreadedObject(), new ThreadedObject(), new ThreadedObject(), new ThreadedObject()
            };

            var allGo = new ManualResetEvent(false);
            
            int waiting = 20;

            int failures = 0;
            Exception firstException = null;
            var threads = new Thread[objs.Length];
            for (int j = 0; j < threads.Length; j++)
            {
                var index = j;
                threads[index] = new Thread(oIndex =>
                {
                    try
                    {
                        var obj = objs[index];

                        if (Interlocked.Decrement(ref waiting) == 0)
                        {
                            allGo.Set();
                        }
                        else
                        {
                            allGo.WaitOne();
                        }

                        try
                        {
                            using (var mapper = new InjectionMapper(index.ToString()))
                            {
                                mapper.Map<IThreadedObject>(() => obj);
                                Trace.WriteLine(string.Format("Added map to Context {0}", index));
                            }
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }

                        for (int i = 0; i < TestCount; i++)
                        {
                            try
                            {
                                using (var resolver = new InjectionResolver(index.ToString()))
                                {
                                    var map = resolver.Resolve<IThreadedObject>();
                                    Trace.WriteLine(string.Format("Resolved map from Context {0} for count {1}", index, i));

                                    if (!obj.Equals(map))
                                    {
                                        throw new InvalidDataException(string.Format("Map not equal in thread {0} on count {1}", index, i));
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                throw e;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Interlocked.CompareExchange(ref firstException, ex, null);
                        Interlocked.Increment(ref failures);
                    }
                });
            }

            for (int j = 0; j < threads.Length; j++)
            {
                threads[j].Start(j % objs.Length);
            }

            for (int j = 0; j < threads.Length; j++)
            {
                threads[j].Join();
            }

            Assert.IsNull(firstException);
            Assert.AreEqual(0, Interlocked.CompareExchange(ref failures, 0, 0));
        }

        [Test]
        public void MultithreadedCallToDefaultContainer()
        {
            using (var mapper = new InjectionMapper())
            {
                mapper.Map<IThreadedObject, ThreadedObject>();
            }

            var allGo = new ManualResetEvent(false);

            int waiting = 20;

            int failures = 0;
            Exception firstException = null;
            var threads = new Thread[20];
            for (int j = 0; j < threads.Length; j++)
            {
                var index = j;
                threads[index] = new Thread(oIndex =>
                {
                    try
                    {
                        if (Interlocked.Decrement(ref waiting) == 0)
                        {
                            allGo.Set();
                        }
                        else
                        {
                            allGo.WaitOne();
                        }

                        for (int i = 0; i < TestCount; i++)
                        {
                            try
                            {
                                using (var resolver = new InjectionResolver())
                                {
                                    var map = resolver.Resolve<IThreadedObject>();
                                    Trace.WriteLine(string.Format("Resolved map from Context {0} for count {1}", index, i));

                                    Assert.IsNotNull(map);
                                }
                            }
                            catch (Exception e)
                            {
                                throw e;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Interlocked.CompareExchange(ref firstException, ex, null);
                        Interlocked.Increment(ref failures);
                    }
                });
            }

            for (int j = 0; j < threads.Length; j++)
            {
                threads[j].Start(j % 20);
            }

            for (int j = 0; j < threads.Length; j++)
            {
                threads[j].Join();
            }

            Assert.IsNull(firstException);
            Assert.AreEqual(0, Interlocked.CompareExchange(ref failures, 0, 0));
        }

        [Test]
        public void MultithreadedCallToDefaultContainerWithStaticObjectMap()
        {
            var obj = new ThreadedObject();
            using (var mapper = new InjectionMapper())
            {
                mapper.Map<IThreadedObject>(() => obj);
            }

            var allGo = new ManualResetEvent(false);

            int waiting = 20;

            int failures = 0;
            Exception firstException = null;
            var threads = new Thread[20];
            for (int j = 0; j < threads.Length; j++)
            {
                var index = j;
                threads[index] = new Thread(oIndex =>
                {
                    try
                    {
                        if (Interlocked.Decrement(ref waiting) == 0)
                        {
                            allGo.Set();
                        }
                        else
                        {
                            allGo.WaitOne();
                        }

                        for (int i = 0; i < TestCount; i++)
                        {
                            try
                            {
                                using (var resolver = new InjectionResolver())
                                {
                                    var map = resolver.Resolve<IThreadedObject>();
                                    Trace.WriteLine(string.Format("Resolved map from Context {0} for count {1}", index, i));

                                    Assert.IsNotNull(map);
                                }
                            }
                            catch (Exception e)
                            {
                                throw e;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Interlocked.CompareExchange(ref firstException, ex, null);
                        Interlocked.Increment(ref failures);
                    }
                });
            }

            for (int j = 0; j < threads.Length; j++)
            {
                threads[j].Start(j % 20);
            }

            for (int j = 0; j < threads.Length; j++)
            {
                threads[j].Join();
            }

            Assert.IsNull(firstException);
            Assert.AreEqual(0, Interlocked.CompareExchange(ref failures, 0, 0));
        }

        [Test]
        public void MultithreadedCallToDefaultContainerWithGlobalResolver()
        {
            using (var mapper = new InjectionMapper())
            {
                mapper.Map<IThreadedObject, ThreadedObject>();
            }

            var allGo = new ManualResetEvent(false);

            int waiting = 20;

            int failures = 0;
            Exception firstException = null;
            var threads = new Thread[20];

            using (var resolver = new InjectionResolver())
            {
                for (int j = 0; j < threads.Length; j++)
                {
                    var index = j;
                    threads[index] = new Thread(oIndex =>
                    {
                        try
                        {
                            if (Interlocked.Decrement(ref waiting) == 0)
                            {
                                allGo.Set();
                            }
                            else
                            {
                                allGo.WaitOne();
                            }

                            for (int i = 0; i < TestCount; i++)
                            {
                                try
                                {

                                    var map = resolver.Resolve<IThreadedObject>();
                                    Trace.WriteLine(string.Format("Resolved map from Context {0} for count {1}", index, i));
                                    
                                    Assert.IsNotNull(map);
                                }
                                catch (Exception e)
                                {
                                    throw e;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Interlocked.CompareExchange(ref firstException, ex, null);
                            Interlocked.Increment(ref failures);
                        }
                    });
                }
            }

            for (int j = 0; j < threads.Length; j++)
            {
                threads[j].Start(j % 20);
            }

            for (int j = 0; j < threads.Length; j++)
            {
                threads[j].Join();
            }

            Assert.IsNull(firstException);
            Assert.AreEqual(0, Interlocked.CompareExchange(ref failures, 0, 0));
        }

        private interface IThreadedObject
        {
        }

        private class ThreadedObject : IThreadedObject
        {
        }
    }
}
