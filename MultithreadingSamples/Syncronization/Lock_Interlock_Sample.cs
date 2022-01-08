using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultithreadingSamples.Syncronization
{
    // Samples of usage: lock, Interlock when multiple threads manipulate data
    [TestClass]
    public class Lock_Interlock_Sample
    {
        [TestMethod]
        [DynamicData(nameof(GetBankAccountImplementation), DynamicDataSourceType.Method)]
        public void LockSamples(IAccount account)
        {
            var tasks = new List<Task>();

            for (int i = 0; i < 100; i++)
            {
                tasks.Add(Task.Factory.StartNew(() => {
                    for (int j = 0; j < 1000; j++)
                        account.Deposit(1);
                }));
                tasks.Add(Task.Factory.StartNew(() => {
                    for (int j = 0; j < 1000; j++)
                        account.Withdraw(1);
                }));
            }
            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"Account balance is {account.Balance}");
        }

        public static IEnumerable<object[]> GetBankAccountImplementation()
        {
            yield return new[] { new BankAccountNoLock() }; // always different Balance but has to be zero
            yield return new[] { new BankAccountWithLock() };
            yield return new[] { new BankAccountInterlock() };
        }
    }

}
