using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

namespace MultithreadingSamples.Threads.Syncronization
{

    [TestClass]
    public class LockReccursionTest
    {
        static SpinLock spinLock = new SpinLock(true); // without true - could be just a deadlock

        static void LockReccursion(int n)
        {
            var lockTaken = false;
            try
            {
                spinLock.Enter(ref lockTaken);
            }
            catch (Exception ex) // lock reccursion exception
            {
                throw;
            }
            finally
            {
                if (lockTaken)
                {
                    LockReccursion(n - 1);
                    spinLock.Exit(); // will not exit
                }
            }
        }

        [TestMethod]
        public void MyTestMethod()
        {
            LockReccursion(3);
        }
    }

}
