using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncFormSample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = LongCalculations("Blocking call complete");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() => LongCalculations("Task continuation"))
                .ContinueWith(
                    task => label1.Text = task.Result,
                    TaskScheduler.FromCurrentSynchronizationContext()
                );
            
            // InvalidOperationException - trying to update from not UI thread
            //Task.Factory.StartNew(() => LongCalculations("Task continuation"))
            //    .ContinueWith(r => label1.Text = r.Result);
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            label1.Text = await LongCalculationsAsync("Async call complete");
        }

        private string LongCalculations(string message)
        {
            Thread.Sleep(3000);
            return message;
        }

        private Task<string> LongCalculationsAsync(string message)
            => Task.Factory.StartNew(() => LongCalculations(message));  
    }
}
