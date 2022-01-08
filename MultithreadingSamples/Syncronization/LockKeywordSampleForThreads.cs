using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MultithreadingSamples.Syncronization
{
    //Note: Enumerating .NET collections is also thread-unsafe in the sense that an exception is thrown if the list is modified during enumeration
    [TestClass]
    public class LockKeywordSampleForThreads
    {
        // run in debug mode, then it works
        [TestMethod]
        public void WithoutLock()
        {
            for (int i = 0; i < 100; i++)
                new Thread(TestMethods.AddItemThreadUnsafe).Start();

            Thread.Sleep(1000);

            // some elements repeated
            foreach (var item in TestMethods.List)
                Console.WriteLine(item);

            Console.WriteLine($"Expected: {TestMethods.List.Count}, actual: {TestMethods.List.Distinct().Count()}");
            Assert.AreNotEqual(TestMethods.List.Count, TestMethods.List.Distinct().Count());
        }

        [TestMethod]
        public void WithLock()
        {
            for (int i = 0; i < 100; i++)
                new Thread(TestMethods.AddItemThreadSafe).Start();

            Thread.Sleep(1000);

            foreach (var item in TestMethods.List)
                Console.WriteLine(item);

            Assert.AreEqual(TestMethods.List.Count, TestMethods.List.Distinct().Count());
        }

    }

    static class TestMethods
    {
        public static List<string> List = new List<string>();

        public static void AddItemThreadUnsafe()
        {
            List.Add($"Item {List.Count}");
        }

        public static void AddItemThreadSafe()
        {
            lock (List)
                List.Add($"Item {List.Count}");
        }
    }
}
