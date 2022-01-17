using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultithreadingSamples._5._ParallelLoops
{
    [TestClass]
    public class ParallelSamples
    {
        [TestMethod]
        public void Invoke()
        {
            var t1 = new Action(() => Console.WriteLine("Task 1"));
            var t2 = new Action(() => Console.WriteLine("Task 2"));
            var t3 = new Action(() => Console.WriteLine("Task 3"));
            var t4 = new Action(() => Console.WriteLine("Task 4"));

            var options = new ParallelOptions
            {
                CancellationToken = default,
                MaxDegreeOfParallelism = 5
            };
            // wrap actions into tasks
            // blocking - wait till completed
            Parallel.Invoke(options, t1, t2, t3, t4); 
        }

        [TestMethod]
        public void ParallelFor()
        {
            // step is always 1, for anotehr step use foreach + custom step provider
            Parallel.For(1, 11, i => Console.WriteLine($"{i * i}"));
        }

        [TestMethod]
        public void ParallelForEach()
        {
            // as well allowed options
            Parallel.ForEach(
                new[] { "T1", "T2", "T3", "T4" },
                s => Console.WriteLine($"Proceed {s}")
            );
        }
    }
}
