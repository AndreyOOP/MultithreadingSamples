using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// Spin lock & lock reccursion samples
namespace MultithreadingSamples.Syncronization
{
    [TestClass]
    public class SpinLockSample
    {
        [TestMethod]
        public void SpinLockTest()
        {
            var tasks = new List<Task>();
            var spinLock = new SpinLock();
            var account = new BankAccountNoLock();

            for (int j = 0; j < 10; j++)
            {
                tasks.Add(
                    Task.Factory.StartNew(() => {
                        for (int i = 0; i < 1000; i++)
                        {
                            var lockTaken = false;
                            try
                            {
                                spinLock.Enter(ref lockTaken);
                                account.Deposit(1);
                            }
                            finally
                            {
                                if (lockTaken)
                                    spinLock.Exit();
                            }
                        }
                    })
                );

                tasks.Add(
                    Task.Factory.StartNew(() => {
                        for (int i = 0; i < 1000; i++)
                        {
                            var lockTaken = false;
                            try
                            {
                                spinLock.Enter(ref lockTaken);
                                account.Withdraw(1);
                            }
                            finally
                            {
                                if (lockTaken)
                                    spinLock.Exit();
                            }
                        }
                    })
                );
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"Account balance is {account.Balance}");
        }
    }

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
