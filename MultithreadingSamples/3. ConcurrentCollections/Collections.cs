using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultithreadingSamples.ConcurrentCollections
{
    [TestClass]
    public class ConcurrentDictionarySample
    {
        [TestMethod]
        public void Dictionary()
        {
            var distinctResult = new int[10000];
            for (int i = 0; i < 10000; i++)
            {
                var dict = new ConcurrentDictionary<string, int>();
                var tasks = new List<Task>() {
                    Task.Factory.StartNew(() => dict.TryAdd("a", 1)),
                    Task.Factory.StartNew(() => dict["a"] = 2),
                    Task.Factory.StartNew(() => dict.AddOrUpdate("b", 4, (key, oldVal) => 5)),
                    Task.Factory.StartNew(() => dict.AddOrUpdate("a", 4, (key, oldVal) => oldVal+9)),
                    //Task.Factory.StartNew(() => dict.AddOrUpdate("a", 4, null)) function has to be present
                    
                };
                Task.WaitAll(tasks.ToArray());

                distinctResult[i] = dict.GetOrAdd("a", -1);
                dict.TryRemove("b", out var value); // value = dict[b]
            }

            distinctResult.Distinct().ToList().ForEach(a => Console.Write($"{a}, "));
        }

        [TestMethod]
        public void Queue()
        {
            var queue = new ConcurrentQueue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            // in (Enqueue) > 3, 2, 1 > out (Dequeue)

            if(queue.TryDequeue(out var i))
            {
                Console.WriteLine(i);
            }

            if(queue.TryPeek(out i))
            {
                Console.WriteLine(i);
            }
        }

        [TestMethod]
        public void Stack()
        {
            var stack = new ConcurrentStack<int>();
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);
            stack.Push(4);

            if(stack.TryPeek(out var result))
                Console.WriteLine(result);
            
            if (stack.TryPop(out result))
                Console.WriteLine(result);

            var items = new int[5];
            stack.TryPopRange(items);
        }

        [TestMethod]
        public void ConsurrentBag()
        {
            var bag = new ConcurrentBag<int>();
            bag.Add(1);
            bag.TryTake(out var result); // remove & return an object
            bag.TryPeek(out result);
        }
    }
}
