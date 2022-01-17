using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultithreadingSamples.TaskCoordination
{
    [TestClass]
    public class SemaphoreSample
    {
        [TestMethod]
        public void SemSample()
        {
            var semaphore = new SemaphoreSlim(2, 2);
            for(int i=0; i<20; i++)
            {
                var temp = i;
                Task.Factory.StartNew(() => {
                    Console.WriteLine($"{temp} - start execution");
                    semaphore.Wait(); // reduce internal counter
                    Thread.Sleep(100);
                    Console.WriteLine($"{temp} - complete execution");
                });
            }

            for(int i=0; i<7; i++)
            {
                Task.Delay(500).Wait();
                // allows next n threads to execute
                // increase internal counter by 2 => two more tasks will be allowed to execute
                var prev = semaphore.Release(2);
                Console.WriteLine($"Semaphore count: previous = {prev}, current = {semaphore.CurrentCount}");
            }
        }
    }
}
