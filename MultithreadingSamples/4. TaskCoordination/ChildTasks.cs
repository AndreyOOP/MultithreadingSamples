using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultithreadingSamples.TaskCoordination
{
    [TestClass]
    public class ChildTasks
    {
        [TestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public void ChildTaskSample(bool fail)
        {
            var parent = Task.Factory.StartNew(() =>
            {
                var child = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("Child task execution");
                    if (fail)
                        throw new Exception();
                }, TaskCreationOptions.AttachedToParent);

                var successHandler = child.ContinueWith(
                    t => Console.WriteLine("OK"),
                    TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.OnlyOnRanToCompletion
                );

                var failHandler = child.ContinueWith(
                    t => Console.WriteLine("Fail"),
                    TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.OnlyOnFaulted
                );
            });

            parent.Wait(); // wait for child task completion as well
        }
    }
}
