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
    public class ResetEventSample
    {
        [TestMethod]
        public void ManualResetEventTest()
        {
            var resetEvent = new ManualResetEventSlim();
            Task.Factory.StartNew(() => {
                Console.WriteLine("Boiling water");
                Thread.Sleep(1000);
                resetEvent.Set(); // signaling that this step is complete
            });
            var task = Task.Factory.StartNew(() => {
                resetEvent.Wait(); // waiting the singnal
                resetEvent.Wait(); // few waits are ok
                Console.WriteLine("Boiled");
            });
            task.Wait();
        }

        [TestMethod]
        public void AutoResetEvent()
        {
            var resetEvent = new AutoResetEvent(false);
            Task.Factory.StartNew(() => {
                Console.WriteLine("Boiling water");
                Thread.Sleep(1000);
                resetEvent.Set(); // signaling that this step is complete
            });
            var task = Task.Factory.StartNew(() => {
                resetEvent.WaitOne(); // waiting the singnal
                // resetEvent.WaitOne(); // stack here, requiers next .Set()
                Console.WriteLine("Boiled");
            });
            task.Wait();
        }
    }
}
