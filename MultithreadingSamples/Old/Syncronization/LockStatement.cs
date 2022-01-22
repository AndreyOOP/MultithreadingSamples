using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MultithreadingSamples.Threads.Syncronization
{
    [TestClass  ]
    public class LockStatement
    {
        // as well show ok result
        [TestMethod]
        public void ThreadUnsafe()
        {
            var list = new List<int>();

            for (int i = 0; i < 1000; i++)
            {
                new Thread(() => list.Add(list.Count)).Start();
            }

            Thread.Sleep(500);

            foreach(var item in list)
                Console.Write($"{item}, ");
        }

        [TestMethod]
        public void ThreadSafeLockFix()
        {
            var locker = new object();
            var list = new List<int>();

            for (int i = 0; i < 100; i++)
            {
                new Thread(() => {
                    // only one thread could be inside the lock block
                    lock (locker)
                    {
                        list.Add(list.Count);
                    }
                }).Start();
            }

            Thread.Sleep(500);

            foreach (var item in list)
                Console.Write($"{item}, ");
        }

        [TestMethod]
        public void MonitorSampleWaitAndPulse()
        {
            var locker = new object();
            var t = new Thread(() => {
                lock (locker)
                {
                    Console.WriteLine("Thread: before Wait");
                    Monitor.Wait(locker);
                    Console.WriteLine("Thread: after Wait");

                }
            });
            
            t.Start(); 
            Thread.Sleep(200);

            lock (locker)
            {
                Console.WriteLine("Before Pulse");
                Monitor.Pulse(locker);
                Console.WriteLine("After Pulse");
            }

            t.Join();
        }
    }
}
