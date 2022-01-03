using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/*
 * Sample of calcellation token usage
 * Composite cancellation token
 * How to react/check if cancellation have been called
 */
namespace MultithreadingSamples.Tasks
{
    [TestClass]
    public class CancellingTask
    {
        [TestMethod]
        public void CancellationTokenSample()
        {
            int i = 0;
            var cts = new CancellationTokenSource();

            Task.Factory.StartNew(() => {
                while (true)
                {
                    //if (cts.Token.IsCancellationRequested)
                    //{
                    //    Console.WriteLine("Task cancelled");
                    //    //break;
                    //    // or more canonical way:
                    //    throw new OperationCanceledException();
                    //}
                    cts.Token.ThrowIfCancellationRequested(); // shortcut of the above
                    i++;
                }
            }, cts.Token);

            Thread.Sleep(3000);
            cts.Cancel(); // just itself it does not calcell the task, it has to be handled via Token.IsCancellationRequested

            Console.WriteLine($"Cancelled after 3 sec, i = {i}");
        }

        [TestMethod]
        public void ReactOnTaskCancelation_Register()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            token.Register(() => Console.WriteLine("Method executed after task cancelled"));

            var t = new Task(() => {
                while (true)
                    cts.Token.ThrowIfCancellationRequested();
            }, token);

            t.Start(); Console.WriteLine("Task started");

            cts.Cancel();
        }

        [TestMethod]
        public void ReactOnTaskCancelation_WaitHandle()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            Task.Factory.StartNew(() => {
                token.WaitHandle.WaitOne();
                Console.WriteLine("Method executed after WaitHandle");
            });

            var t = new Task(() => {
                while (true)
                    cts.Token.ThrowIfCancellationRequested();
            }, token);

            t.Start(); Console.WriteLine("Task started");

            cts.Cancel();
        }

        [TestMethod]
        public void CombinedCancellationToken()
        {
            int i = 0;
            var ctsFirst = new CancellationTokenSource();
            var ctsSecond = new CancellationTokenSource();

            var combined = CancellationTokenSource.CreateLinkedTokenSource(ctsFirst.Token, ctsSecond.Token);

            Task.Factory.StartNew(() => {
                while (true)
                {
                    i++;
                    combined.Token.ThrowIfCancellationRequested();
                }
            }, combined.Token);

            Thread.Sleep(100);
            
            // any of below will cancel the task
            ctsFirst.Cancel();
            //ctsSecond.Cancel();
            //combined.Cancel();

            Console.WriteLine($"Cancelled, i = {i}");
        }
    }
}
