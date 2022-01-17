using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultithreadingSamples.TaskCoordination
{
    [TestClass]
    public class Continuation
    {
        [TestMethod]
        public void SingleContinueWith()
        {
            var task = Task.Factory.StartNew(() => { 
                Console.WriteLine("Step 1");
                return "Result of task1";
            });
            var task2 = task.ContinueWith(t =>
            {
                Console.WriteLine($"T1 = {t.Result}");
                Console.WriteLine("Step 2"); ;
            });
            task2.Wait();
        }

        [TestMethod]
        public void ContinueWhenAll()
        {
            var task1 = Task.Factory.StartNew(() => "result_1");
            var task2 = Task.Factory.StartNew(() => "result_2");

            var task3 = Task.Factory.ContinueWhenAll(new[] { task1, task2 }, tasks => { 
                foreach(var t in tasks)
                    Console.WriteLine($" - result is {t.Result}");
                Console.WriteLine("Task 3 - completed");
            });

            task3.Wait();
        }
    }
}
