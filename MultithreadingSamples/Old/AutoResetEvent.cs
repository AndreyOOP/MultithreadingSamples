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
    public class AutoResetEventSample
    {
        [TestMethod]
        public void AutoResetWaitHandlerExecution()
        {
            var autoResetWaitHandler = new AutoResetWaitHandler();
            AutoResetWaitHandler.Main();
        }
    }

    class AutoResetWaitHandler
    {
        static EventWaitHandle waitHandle = new AutoResetEvent(false);

        public static void Main()
        {
            Console.WriteLine("[Main] Start thread");
            new Thread(Waiter).Start();
            Thread.Sleep(1000);
            Console.WriteLine("[Main] send signal");
            waitHandle.Set();
        }

        public static void Waiter()
        {
            Console.WriteLine("[Wait] Blocked");
            waitHandle.WaitOne();
            Console.WriteLine("[Wait] Signal received");
        }
    }
}
