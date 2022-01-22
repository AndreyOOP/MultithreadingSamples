using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultithreadingSamples._7.Async
{
    [TestClass]
    public class TaskRunSample
    {
        [TestMethod]
        public async Task TaskFactoryTest()
        {
            //Task<Task<int>> i = Task.Factory.StartNew(async () => {
            //    await Task.Delay(500);
            //    return 10;
            //});

            // Note double await
            int i = await await Task.Factory.StartNew(async () => {
                await Task.Delay(500);
                return 10;
            });

            // Unwarap() is kind of analog of await
            int n = await Task.Factory.StartNew(async () => {
                await Task.Delay(500);
                return 10;
            }).Unwrap();
        }

        [TestMethod]
        // TaskRun wrapper of Task.Factory.Start
        // TaskRun makes execution of async & normal Action similar - return Task<T>, at the same time Task.Factory return for async lambda Task<Task<T>>
        public async Task TaskRun() 
        {
            var i = await Task.Run(async () =>
            {
                await Task.Delay(500);
                return 10;
            });

            var n = await Task.Run(() => 10);
        }
    }
}
