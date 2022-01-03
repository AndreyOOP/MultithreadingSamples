using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultithreadingSamples.Threads.Syncronization
{
    interface IAccount
    {
        int Balance { get; }
        void Deposit(int amount);
        void Withdrawal(int amount);
    }

    class AccountNoLock : IAccount
    {
        public int Balance { get; private set; }

        public void Deposit(int amount)
        {
            Balance += amount;
        }

        public void Withdrawal(int amount)
        {
            Balance -= amount;
        }
    }

    class AccountWithLock : IAccount
    {
        private object locker = new object();
        public int Balance { get; private set; }

        public void Deposit(int amount)
        {
            lock (locker)
            {
                Balance += amount;
            }
        }

        public void Withdrawal(int amount)
        {
            lock (locker)
            {
                Balance -= amount;
            }
        }
    }

    class AccountInterlocked : IAccount
    {
        private int balance;
        public int Balance => balance;

        public void Deposit(int amount)
        {
            Interlocked.Add(ref balance, amount);
        }

        public void Withdrawal(int amount)
        {
            Interlocked.Add(ref balance, -amount);
        }
    }

    [TestClass]
    public class LockBankAccountSample
    {
        [TestMethod]
        public void WithoutLock()
        {
            Console.WriteLine(
                $"No synchronization: {DepositWithdrawal(new AccountNoLock())}"
            );

            Console.WriteLine(
                $"Lock used: {DepositWithdrawal(new AccountWithLock())}"
            );

            Console.WriteLine(
                $"Interlocker used: {DepositWithdrawal(new AccountInterlocked())}"
            );
        }

        [TestMethod]
        public void SpinLock()
        {
            Console.WriteLine(
                $"SpinLock used: {DepositWithdrawalSpinLock(new AccountNoLock())}"
            );
        }

        [TestMethod]
        public void Mutex()
        {
            Console.WriteLine(
                $"Mutex used: {DepositWithdrawalMutex(new AccountNoLock())}"
            );
        }

        private int DepositWithdrawal(IAccount account)
        {
            var tasks = new List<Task>();

            for (int j = 0; j < 10; j++)
            {
                for (int i = 0; i < 1000; i++)
                {
                    tasks.Add(
                        Task.Factory.StartNew(() => account.Deposit(1))
                    );
                }

                for (int i = 0; i < 1000; i++)
                {
                    tasks.Add(
                        Task.Factory.StartNew(() => account.Withdrawal(1))
                    );
                }
            }

            Task.WaitAll(tasks.ToArray());

            return account.Balance;
        }

        private int DepositWithdrawalSpinLock(IAccount account)
        {
            var tasks = new List<Task>();
            var spinLock = new SpinLock();
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
                                if(lockTaken)
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
                                account.Withdrawal(1);
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

            return account.Balance;
        }

        private int DepositWithdrawalMutex(IAccount account)
        {
            var tasks = new List<Task>();
            var mutex = new Mutex();

            for (int j = 0; j < 10; j++)
            {
                tasks.Add(
                    Task.Factory.StartNew(() =>
                    {
                        for (int i = 0; i < 1000; i++)
                        {
                            var lockTaken = mutex.WaitOne();
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
                                account.Withdrawal(1);
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

            return account.Balance;
        }
    }
}
