using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultithreadingSamples.Syncronization
{
    [TestClass]
    public class MutexSample
    {
        [TestMethod]
        public void MutextSample()
        {
            var account = new BankAccountNoLock();
            var tasks = new List<Task>();
            var mutex = new Mutex();

            for (int j = 0; j < 10; j++)
            {
                tasks.Add(
                    Task.Factory.StartNew(() =>
                    {
                        for (int i = 0; i < 1000; i++)
                        {
                            var lockTaken = mutex.WaitOne(); // wait till mutext available (no other thread in)
                            try
                            {
                                account.Deposit(1);
                            }
                            finally
                            {
                                if (lockTaken)
                                    mutex.ReleaseMutex();
                            }
                        }
                    })
                );

                tasks.Add(
                    Task.Factory.StartNew(() =>
                    {
                        for (int i = 0; i < 1000; i++)
                        {
                            var lockTaken = mutex.WaitOne();
                            try
                            {
                                account.Withdraw(1);
                            }
                            finally
                            {
                                if (lockTaken)
                                    mutex.ReleaseMutex();
                            }
                        }
                    })
                );
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"Account balance is {account.Balance}");
            Assert.AreEqual(0, account.Balance);
        }

        [TestMethod]
        public void TwoMutexSample()
        {
            var tasks = new List<Task>();
            var mutexFrom = new Mutex();
            var mutexTo = new Mutex();
            var pl = new ReaderWriterLockSlim();
            var accountFrom = new BankAccountNoLock();
            var accountTo = new BankAccountNoLock();
            accountFrom.Deposit(10000);

            for (int j = 0; j < 10; j++)
            {
                tasks.Add(Task.Factory.StartNew(() => {
                    for(int i=0; i<1000; i++)
                    {
                        var lockTaken = WaitHandle.WaitAll(new[] { mutexFrom, mutexTo });
                        try
                        {
                            accountFrom.Transfer(accountTo, 1);
                        }
                        finally
                        {
                            if (lockTaken)
                            {
                                mutexFrom.ReleaseMutex();
                                mutexTo.ReleaseMutex();
                            }
                        }
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"AccountFrom balance is {accountFrom.Balance}; AccountTo balance is {accountTo.Balance}");
            Assert.AreEqual(0, accountFrom.Balance);
            Assert.AreEqual(10000, accountTo.Balance);
        }
    }
}
