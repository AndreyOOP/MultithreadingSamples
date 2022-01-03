using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

namespace MultithreadingSamples
{
    [TestClass]
    public class ThreadBasics
    {
        [TestMethod]
        public void StartThread()
        {
            // ThreadStart - contains delegate executed during thread run
            var thread = new Thread(new ThreadStart(
                () => Console.WriteLine("x")
            ));
            thread.Start();
        }

        [TestMethod]
        public void StatesOfThread()
        {
            // Thread.CurrentThread
            var thread = new Thread(new ThreadStart(() => Console.WriteLine("x"))); 
            thread.GetInfo();

            thread.Start();
            thread.GetInfo();

            thread.Suspend();
            thread.GetInfo();

            thread.Resume();
            thread.GetInfo();
        }

        [TestMethod]
        public void InterruptAndAbort()
        {
            var t = new Thread(new ThreadStart(() => {
                try
                {
                    while (true)
                    {
                        // without sleep Interrupt() may not work, but Abort() works
                        Thread.Sleep(10);
                    }
                }
                catch (Exception ex)
                {
                    // in case of Abort() => ex is ThreadAbortException
                    // in case of Interrupt() => ex is ThreadInterruptedException
                    throw;
                }
            }));
            t.Start();

            Thread.Sleep(100);
            t.Interrupt(); //or t.Abort();
            
            // without interrupt thread will always wait for 't'
            t.Join();
        }

        [TestMethod]
        public void GetApplicationDomain()
        {
            var domain = Thread.GetDomain();
        }

        [TestMethod]
        public void PassParameterToThread()
        {
            // via lambda - more powerfull way
            var i = 100;
            var t = new Thread(() => Console.WriteLine(i));
            t.Start();

            // via object in Start method
            var t2 = new Thread(s => Console.WriteLine(s));
            t2.Start("abc");
        }

        [TestMethod]
        public void CapturedVariable()
        {
            // output nondeterministic, like 11335678910
            // i is local variable for thread, so few threads reference to the same variable which changes
            for (int i = 0; i < 10; i++)
                new Thread(() => Console.Write($"'x{i}'")).Start();

            //Thread.Sleep(100);
            Console.WriteLine();

            // output 0123456789, temp is always local variable
            for (int j = 0; j < 10; j++)
            {
                var temp = j;
                new Thread(() => Console.Write(temp)).Start();
            }
        }
    }

    public static class ThreadExtensions
    {
        public static void GetInfo(this Thread thread)
        {
            Console.WriteLine($"Id: {thread.ManagedThreadId}, Name: {thread.Name}, Priority: {thread.Priority}, State: {thread.ThreadState}");
        }
    }
}
