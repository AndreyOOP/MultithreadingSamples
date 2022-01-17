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
    public class BarrierSample
    {
        Random random = new Random();
        Barrier barrier = new Barrier(2, b => Console.WriteLine($"Phase {b.CurrentPhaseNumber} completed{Environment.NewLine}"));

        // note that inside phases order changes from run to run
        [TestMethod]
        public void BarrierSampleTest()
        {
            var kettle = Task.Factory.StartNew(() => {
                Step("Putting kettle");
                //barrier.RemoveParticipant(); in case on the next phase required only one thread
                Step("Puoring water");
                Step("Puttin kettle away");
            });
            var cup = Task.Factory.StartNew(() => {
                Step("Finding cup");
                Step("Adding tea");
                Step("Adding sugar");
            });
            Task.WaitAll(kettle, cup);
        }

        private void Step(string description)
        {
            Thread.Sleep(random.Next(1000));
            Console.WriteLine(description);
            barrier.SignalAndWait();
        }
    }
}
