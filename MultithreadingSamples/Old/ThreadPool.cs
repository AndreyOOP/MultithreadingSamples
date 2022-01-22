using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultithreadingSamples
{
    [TestClass]
    public class ThreadPoolSamples
    {
        [TestMethod]
        public void StartThreadInThreadPoolViaTask()
        {
            // main thread - not thread pool
            Console.WriteLine($"Id: {Thread.CurrentThread.ManagedThreadId}, Is thread pool - {Thread.CurrentThread.IsThreadPoolThread}");

            // thread from task is thread pool thread
            Task.Run(() => Console.WriteLine($"Task run, Id: {Thread.CurrentThread.ManagedThreadId}, Is thread pool - {Thread.CurrentThread.IsThreadPoolThread}"));
        }

        [TestMethod]
        public void StartThreadInThreadPoolDirectly()
        {
            ThreadPool.QueueUserWorkItem( obj => Console.WriteLine($"Id: {Thread.CurrentThread.ManagedThreadId}, Is thread pool - {Thread.CurrentThread.IsThreadPoolThread}"));
        }
    }
}
