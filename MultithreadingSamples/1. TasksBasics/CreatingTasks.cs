using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/*
 * Start task via new Task, TaskFactory, pass parameter to the task, get task result
 */
namespace MultithreadingSamples
{
    class Write
    {
        public static string Result;

        public static void WriteCharacter(char c)
        {
            for (int i = 0; i < 10000; i++)
            {
                // Just one line make output to look synchronus
                Console.Write(c);
                Result += c;
            }
        }

        public static void WriteCharacter(object c)
        {
            for (int i = 0; i < 100; i++)
            {
                Console.Write(c.ToString());
            }
        }

        public static int GetTextLength(string text)
        {
            Console.WriteLine($"Task {Task.CurrentId} start working");
            return text.Length;
        }
    }

    // Result in test output
    [TestClass]
    public class TaskBasics
    {
        [TestMethod]
        public void CreateTaskViaFactory()
        {
            Task.Factory.StartNew(() => Write.WriteCharacter('.'));
        }

        [TestMethod]
        public void CreateTaskInstance()
        {
            var task = new Task(() => Write.WriteCharacter('?'));
            task.Start();
        }

        [TestMethod]
        public void RunFewTasksTogether()
        {
            Task.Factory.StartNew(() => Write.WriteCharacter('.'));
            
            var task = new Task(() => Write.WriteCharacter('?'));
            task.Start();

            Write.WriteCharacter('-');

            Console.WriteLine(Write.Result); // Here mix of .?-
        }

        [TestMethod]
        public void PassParameterToTheTask()
        {
            var task = new Task(Write.WriteCharacter, "z");
            task.Start();
        }

        [TestMethod]
        public void GetResultOfTheTask()
        {
            var task1 = new Task<int>(() => Write.GetTextLength("abc"));
            task1.Start();

            var task2 = Task.Factory.StartNew<int>(() => Write.GetTextLength("abcd"));

            Assert.AreEqual(3, task1.Result);
            Assert.AreEqual(4, task2.Result);
        }

        [TestMethod]
        public void Exception()
        {
            Foo();
        }

        IEnumerable<int> Foo()
        {
            throw new NotImplementedException();
            yield return 123;
        }

        [TestMethod]
        public void MyTestMethod()
        {
            var t = new Task<string>(() => "hello");
            t.Start();
            var zz = t.Result;
        }
    }
}
