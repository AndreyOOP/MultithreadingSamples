using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace MultithreadingSamples.ConcurrentCollections
{
    [TestClass]
    public class ProducerConsumerSample
    {
        Random random = new Random();
        CancellationTokenSource cts = new CancellationTokenSource(); // just example, seems it is not correct to use the same cancellation token for all producer/consumers - Task.WaitAll - hang up
        BlockingCollection<int> messages = new BlockingCollection<int>(new ConcurrentBag<int>(), 10);

        // note output: quick start of producing w/o consuming, & after produce-consume ~at the same rate - it happens because of messages.Add(i) blocking
        [TestMethod]
        public void ProducerConsumer()
        {
            var producer1 = Task.Factory.StartNew(Producer<string>, "Producer_1");
            var producer2 = Task.Factory.StartNew(Producer<string>, "Producer_2");
            var producer3 = Task.Factory.StartNew(Producer<string>, "Producer_3");
            var consumer = Task.Factory.StartNew(Consumer);

            Thread.Sleep(5000);
            cts.Cancel();

            Task.WaitAny(new[] { producer1, producer2, producer3, consumer });
        }

        void Producer<T>(object producerName)
        {
            var name = producerName as string;
            while (true)
            {
                cts.Token.ThrowIfCancellationRequested();
                var i = random.Next(100);
                messages.Add(i); // if in the bag qty is 10 thread will be blocked here
                Thread.Sleep(random.Next(100));
                Console.WriteLine($"+{i} by {name}");
            }
        }

        void Consumer()
        {
            foreach(var item in messages.GetConsumingEnumerable())
            {
                cts.Token.ThrowIfCancellationRequested();
                Thread.Sleep(random.Next(500));
                Console.WriteLine($"-{item}");
            }
        }
    }
}
