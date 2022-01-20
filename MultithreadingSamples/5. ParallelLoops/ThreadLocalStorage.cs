using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultithreadingSamples._5._ParallelLoops
{
    [TestClass]
    public class ThreadLocalStorage
    {
        [TestMethod]
        public void NoThreadLocalStorage()
        {
            int sum = 0;
            Parallel.For(1, 1001, i =>
            {
                // the problem here is a lot of calls to Interlocked which decrease performance
                Interlocked.Add(ref sum, i);
            });
            Console.WriteLine($"Total {sum}");
        }

        [TestMethod]
        public void ThreadLocalStorageTest()
        {
            int sum = 0;
            Parallel.For(1, 1000001, // actually Parallel.For<T>
                () => 0, // this defines which local storage varaible will be used
                (i, state, tls) => {
                    return ++tls; // thread safe operation, tls is local for thread
                },
                tlsValue => {
                    Interlocked.Add(ref sum, tlsValue);
                    Console.WriteLine($"Task {Task.CurrentId} updated value of sum to {sum}.");
                }
            );
            Console.WriteLine($"Total {sum}");
        }
    }
}
