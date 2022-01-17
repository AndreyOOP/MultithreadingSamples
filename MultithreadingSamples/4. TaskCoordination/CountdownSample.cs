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
    public class CountdownSample
    {
        [TestMethod]
        public void CountdownSampleTest()
        {
            var random = new Random();
            var cde = new CountdownEvent(5);
            new[] { 1, 2, 3, 4, 5 }.ToList().ForEach(
                i => Task.Factory.StartNew(() => {
                    Thread.Sleep(random.Next(2000));
                    Console.WriteLine($"Task {i} completed");
                    cde.Signal(); // reduces internal countdown counter by one
                })
            );

            var taskDoAfter = Task.Factory.StartNew( () =>
            {
                Console.WriteLine("Final task starting");
                cde.Wait(); // block thread till counter become 0
                Console.WriteLine("Final task completed");
            });
            taskDoAfter.Wait();
        }
    }
}
