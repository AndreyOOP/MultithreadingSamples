using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultithreadingSamples.Tasks
{
    [TestClass]
    public class WaitingInTask
    {
        [TestMethod]
        public void WaitSample()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            var task = new Task(() =>
            {
                Thread.Sleep(1000); // scheduler need no processor time anymore => but switch context
                Thread.SpinWait(10); // pause a thread, but scheduler is not going to get other task - CPU uses resources on this
                SpinWait.SpinUntil(() => true); // same as above

                bool cancelled = token.WaitHandle.WaitOne(3000); // will wait for 3 sec
                if(cancelled) // check if it was cancelled cts.Cancel() or just WaitHandle.WaitOne pass
                    Console.WriteLine("Cancelled");
            }, token);
            task.Start();

            Console.ReadLine();
            cts.Cancel();
        }
    }

    [TestClass]
    public class WaitForTask
    {
        [TestMethod]
        public void WaitTaks()
        {
            var token = new CancellationTokenSource().Token;
            var t1 = Task.Factory.StartNew(() => Thread.Sleep(2000));
            var t2 = Task.Factory.StartNew(() => Thread.Sleep(2000));

            t1.Wait(2000, token);
            Task.WaitAll(t1, t2);
            Task.WaitAll(new[] { t1, t2 }, token);
            Task.WaitAny(new[] { t1, t2 }, 1000); // timeout
        }
    }
}
