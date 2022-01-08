using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultithreadingSamples.Tasks
{
    [TestClass]
    public class ExceptionHndling
    {
        [TestMethod]
        public void SeemsNoException_SingleTask()
        {
            Task task;
            try
            {
                task = Task.Factory.StartNew(() => throw new AccessViolationException()); // looks like no exception thrown
            }
            catch (Exception ex)
            {
            }
        }

        [TestMethod]
        public void CatchAgregateException_SingleTaks()
        {
            Task task;
            try
            {
                task = Task.Factory.StartNew(() => throw new AccessViolationException());
                task.Wait(); // need to wait will result
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex is AggregateException);
            }
        }

        [TestMethod]
        public void HandleAggregateExceptions()
        {
            var t1 = Task.Factory.StartNew(() => throw new AccessViolationException());
            var t2 = Task.Factory.StartNew(() => throw new InvalidCastException());

            try
            {
                Task.WaitAll(t1, t2);
            }
            catch (AggregateException ex)
            {
                ex.Handle(e => {
                    if (e is AccessViolationException)
                    {
                        Console.WriteLine("AccessViolationException handled");
                        return true; // means the exception handled
                    }
                    if (e is InvalidCastException)
                    {
                        Console.WriteLine("InvalidCastException handled");
                        return true;
                    }
                    return false;
                });
            }
        }
    }
}
