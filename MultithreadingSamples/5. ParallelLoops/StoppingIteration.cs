using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultithreadingSamples._5._ParallelLoops
{
    [TestClass]
    public class StoppingIteration
    {
        [TestMethod]
        public void MyTestMethod()
        {
            var cts = new CancellationTokenSource();
            try
            {
                var options = new ParallelOptions
                {
                    CancellationToken = cts.Token
                };
                var result = Parallel.For(0, 20, options, (i, state) => {
                    if(i == 10)
                    {
                        //state.Break(); // break iterations after 10 - here 0-10 task are executed
                        //state.Stop(); // just stop execution ~ 3 tasks are executed
                        //throw new Exception(); // similar to stop + prpagates AggregateException exception
                        cts.Cancel(); // similar to stop + prpagates OperationCanceledException exception
                    }
                    Thread.Sleep(100);
                    Console.WriteLine($"{i}[{Task.CurrentId}]");
                });
                Console.WriteLine($"{nameof(result.IsCompleted)}: {result.IsCompleted}");
                if(result.LowestBreakIteration.HasValue)
                    Console.WriteLine($"{nameof(result.LowestBreakIteration)}: {result.LowestBreakIteration.Value}");
            }
            catch (AggregateException ae)
            {
                ae.Handle(e => { 
                    Console.WriteLine(e.Message);
                    return true;
                });
            }
            catch (OperationCanceledException ex) // required to handle cts.Cancel()
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
