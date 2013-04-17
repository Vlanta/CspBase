using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ThreadExceptionDemo
{
    /// <summary>
    /// 结论：在某一个线程抛出未处理的异常，将会终止程序
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Main Running......");
            new Thread(ThreadRunFun).Start();
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(1000);
                Console.WriteLine(i+"  in Mian()");
            }
        }
        static void ThreadRunFun()
        {
            Console.WriteLine("Thread Running......");
            Console.WriteLine("Thread Throw Exception");
            throw new Exception("in Thread");
        }
    }
}
