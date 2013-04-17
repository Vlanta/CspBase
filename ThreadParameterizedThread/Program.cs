using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadParameterizedThread
{
    //资源类
    #region The AddParams class
    class AddParams
    {
        public int a, b;

        public AddParams(int numb1, int numb2)
        {
            a = numb1;
            b = numb2;
        }
    }
    #endregion
    //线程类
    public class Program
    {
        private static AutoResetEvent waitHandle = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            Console.WriteLine("***** Adding with Thread objects *****");
            Console.WriteLine("ID of thread in Main(): {0}",
              Thread.CurrentThread.ManagedThreadId);
            //建立AddParams对象（资源类），将其传给次线程
            AddParams ap = new AddParams(10, 10);
            Thread t = new Thread(new ParameterizedThreadStart(Add));
            t.Start(ap);

            // Wait here until you are notified等待，直到收到通知         
            waitHandle.WaitOne();

            Console.WriteLine("Other thread is done!");
            Console.ReadLine();
        }

        static void Add(object data)
        {
            if (data is AddParams)
            {
                Console.WriteLine("ID of thread in Add(): {0}",
                  Thread.CurrentThread.ManagedThreadId);
              
                AddParams ap = (AddParams)data;
                Console.WriteLine("{0} + {1} is {2}",
                  ap.a, ap.b, ap.a + ap.b);

                // Tell other thread we are done.通知其他线程，该线程已结束
                waitHandle.Set();
            }
        }
    }
}
