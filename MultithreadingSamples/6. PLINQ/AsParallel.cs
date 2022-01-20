using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultithreadingSamples._6._PLINQ
{
    [TestClass]
    public class AsParallel
    {
        [TestMethod]
        public void AsParallelTest1()
        {
            var range = Enumerable.Range(0, 50);
            var cubes = new int[51];

            range.AsParallel().ForAll(i => {
                cubes[i] = (int)Math.Pow(i, 3);
                Console.Write($"{cubes[i]}\t");
            });
            Console.WriteLine(); Console.WriteLine();
            print(cubes);
        }

        [TestMethod]
        public void AsParallelTest2() {
            var range = Enumerable.Range(0, 50);

            var ordered = range.AsParallel().AsOrdered().Select(i => i * i * i).ToArray();
            print(ordered);
            Console.WriteLine();

            var unOrdered = range.AsParallel().Select(i => i * i * i).ToArray();
            print(unOrdered);
        }

        private void print(int[] arr)
        {
            foreach(var el in arr)
                Console.Write($"{el}\t");
        }
    }
}
