using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultithreadingSamples._6._PLINQ
{
    [TestClass]
    public class ParallelCancellationToken
    {
        [TestMethod]
        public void ParallelCt()
        {
            var cts = new CancellationTokenSource();

            try
            {
                foreach(var res in ParallelEnumerable.Range(1, 50).AsOrdered().WithCancellation(cts.Token).Select(i => Math.Log10(i)))
                {
                    if (res > 1)
                        cts.Cancel(); // note that some time pass till execution stopped, so few other values will be calculated
                    Console.WriteLine(res);
                }
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine("Cancelled");
            }
        }

        [TestMethod]
        public void ParallelError()
        {
            try
            {
                foreach (var res in ParallelEnumerable.Range(1, 50).AsOrdered().Select(i => Math.Log10(i)))
                {
                    if (res > 1)
                        throw new InvalidOperationException();
                    Console.WriteLine(res);
                }
            }
            catch (AggregateException e)
            {
                e.Handle(ex => true);
            }
        }
    }
}
