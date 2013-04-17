using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsThreads
{
    public partial class Form1 : Form
    {
        private SynchronizationContext SyncContext;
        public Form1()
        {
         
            InitializeComponent();
        }

        private void BtnShowM_Click(object sender, EventArgs e)
        {
            MessageBox.Show("立即弹出对话框");
        }

        private void BtnShowM1S_Click(object sender, EventArgs e)
        {
            Thread.Sleep(3000);
            MessageBox.Show("休眠3秒后才弹出对话框");
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            this.Text = "X:" + e.X + "  Y:" + e.Y;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SyncContext = SynchronizationContext.Current;
            label7.Text = Thread.CurrentThread.ManagedThreadId.ToString();
        }
        private void ShowProsser(ProgressBar pBar)
        {
            pBar.Minimum = 0;
            pBar.Maximum = 100;
            pBar.Value = 0;
            for (int i = 1; i <= pBar.Maximum; i++)
            {
                Thread.Sleep(20000);
                pBar.Value = i;
            }
        }
        private void ContextMethod(Object state)
        {
            SyncContext.Post(ShowProsser1 , state);
        }
        private void ShowProsser1(object bar)
        {
            label4.Text = Thread.CurrentThread.ManagedThreadId.ToString();
            ShowProsser((ProgressBar)bar);
        }
        private void WorkEventHandler(object sender, DoWorkEventArgs e)
        {
            label5.Text = Thread.CurrentThread.ManagedThreadId.ToString();
            ShowProsser((ProgressBar)e.Argument);
        }
        private void ShowProsser3()
        {
            ShowProsser(progressBar3);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //这种写法在运行时是可以的，但在调试时是不行的
         
            //Thread th = new Thread(ShowProsser1);
            Thread th = new Thread(ContextMethod);
            th.Start(progressBar1);
        }

        private void button2_Click(object sender, EventArgs e)
        {

            using (BackgroundWorker bw = new BackgroundWorker())
            {
                bw.DoWork += new DoWorkEventHandler(WorkEventHandler);
                bw.RunWorkerAsync(progressBar2);
            }
        }
    }
}
