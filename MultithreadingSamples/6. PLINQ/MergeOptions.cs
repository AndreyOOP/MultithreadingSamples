using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultithreadingSamples._6._PLINQ
{
    [TestClass]
    public class MergeOptions
    {
        [TestMethod]
        [DataRow(ParallelMergeOptions.FullyBuffered)]
        [DataRow(ParallelMergeOptions.NotBuffered)] // not clear why behave same as fully buffered
        [DataRow(ParallelMergeOptions.AutoBuffered)]
        [DataRow(ParallelMergeOptions.Default)]
        public void MergeOptionsTest(ParallelMergeOptions mergeOptions)
        {
            var producer = Enumerable.Range(1, 100).ToArray().AsParallel().WithDegreeOfParallelism(30).WithMergeOptions(mergeOptions).Select(i => {
                var tmp = (int)Math.Log10(Math.Pow(i, 3));
                Console.Write($"P{tmp} ");
                return tmp;
            });

            foreach (var result in producer) {
                Console.Write($"C{result} ");
            }
        }
    }
}
