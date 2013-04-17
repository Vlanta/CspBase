using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.Remoting.Messaging;

namespace AsyncCallbackDelegate
{
    /*注意委托定义的地方，和类是一个级别的
     * 委托类型是一个类型安全的，面向对象的函数指针
     */
    public delegate int BinaryOp(int x, int y);
    public class AsyncCallbackDelegate
    {
        private static bool isDone = false;

        static void Main(string[] args)
        {
            Console.WriteLine("*****  AsyncCallbackDelegate Example *****");
            Console.WriteLine("Main() invoked on thread {0}.",
              Thread.CurrentThread.ManagedThreadId);

            BinaryOp b = new BinaryOp(Add);
            IAsyncResult iftAR = b.BeginInvoke(10, 10,
              new AsyncCallback(AddComplete),//被调用线程完成时的回调方法
              "Main() thanks you for adding these numbers.");//向被调用线程传递的附加信息，类型为Object

            // Assume other work is performed here...
            while (!isDone)
            {
                Thread.Sleep(1000);
                Console.WriteLine("Working....");
        
            }
            Console.ReadLine();
        }

        #region Target for AsyncCallback delegate
        // Don't forget to add a 'using' directive for 
        // System.Runtime.Remoting.Messaging!
        static void AddComplete(IAsyncResult itfAR)
        {
            Console.WriteLine("AddComplete() invoked on thread {0}.",
              Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Your addition is complete");

            // Now get the result.
            AsyncResult ar = (AsyncResult)itfAR;
            //传入的IAsyncResult包含委托的引用
            BinaryOp b = (BinaryOp)ar.AsyncDelegate;
            Console.WriteLine("10 + 10 is {0}.", b.EndInvoke(itfAR));

            //Retrieve the informational object and cast it to string.
            //必须进行强制转换，因为类型是Object的
            string msg = (string)itfAR.AsyncState;
            Console.WriteLine(msg);

            isDone = true;
        }

        #endregion

        #region Target for BinaryOp delegate
        //对应委托的函数
        static int Add(int x, int y)
        {
            Console.WriteLine("Add() invoked on thread {0}.",
              Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(5000);
            return x + y;
        }
        #endregion
    }
}

