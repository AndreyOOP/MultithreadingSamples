using System.Threading;

namespace MultithreadingSamples.Syncronization
{
    public interface IAccount
    {
        void Deposit(int amount);
        void Withdraw(int amount);
        int Balance { get; }
    }

    class BankAccountNoLock : IAccount
    {
        public int Balance { get; set; }

        public void Deposit(int amount)
        {
            // not atomic operation:
            // op1: temp <- get_Balance() + amount
            // op2: x <- set_Balance(temp)
            Balance += amount;
        }

        public void Withdraw(int amount)
        {
            Balance -= amount;
        }

        public void Transfer(IAccount to, int amount)
        {
            Withdraw(amount);
            to.Deposit(amount);
        }
    }

    class BankAccountWithLock : IAccount
    {
        public object padlock = new object();
        public int Balance { get; set; }

        public void Deposit(int amount)
        {
            lock (padlock) // only one thread at a time can be inside lock section
            {
                Balance += amount;
            }
        }

        public void Withdraw(int amount)
        {
            // is is a shorthand of
            // Monitor.Enter(padlock);
            // Monitor.Exit(padlock);
            lock (padlock)
            {
                Balance -= amount;
            }
        }
    }

    class BankAccountInterlock : IAccount
    {
        private int balance;
        public int Balance => balance;

        public void Deposit(int amount)
        {
            Interlocked.Add(ref balance, amount);
        }

        public void Withdraw(int amount)
        {
            Interlocked.Add(ref balance, -amount);
        }
    }

    class BankAccountReadWriteLock : IAccount
    {
        private static ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim(); // support lock reccursion - LockRecursionPolicy.SupportsRecursion

        public int Balance { get; private set; }

        public void Deposit(int amount)
        {
            rwLock.EnterWriteLock();
            Balance += amount;
            rwLock.ExitWriteLock();
        }

        public void Withdraw(int amount)
        {
            rwLock.EnterWriteLock();
            Balance -= amount;
            rwLock.ExitWriteLock();
        }
    }

    class BankAccountMutex : IAccount
    {
        private static Mutex mutex = new Mutex();

        public int Balance { get; private set; }

        public void Deposit(int amount)
        {
            var taken = mutex.WaitOne();
            try
            {
                Balance += amount;
            }
            finally
            {
                if (taken)
                    mutex.ReleaseMutex();
            }
        }

        public void Withdraw(int amount)
        {
            var taken = mutex.WaitOne();
            try
            {
                Balance -= amount;
            }
            finally
            {
                if (taken)
                    mutex.ReleaseMutex();
            }
        }
    }
}
