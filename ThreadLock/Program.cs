using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadLock
{
    //资源类
    #region Printer helper class
    public class Printer
    {
        /*如果需要锁定公共成员中的一段代码，比较安全（也比较推荐）的方式是声明私有的object
         * 成员来作为锁标识
         */
        // Lock token.锁标识
        //private object threadLock = new object();
        private int num = 0;
        public void PrintNumbers()
        {
            //使用当前对象作为线程标记
            lock (this)//所有在这范围{... }内的代码都是线程安全的！
            {
                Thread.Sleep(new Random().Next(1000));
                num += 1;
                //// Display Thread info.
                Console.WriteLine("-> {0} is executing PrintNumbers()",
                  Thread.CurrentThread.Name);

                //// Print out numbers.
                Console.Write("Your numbers: ");
                for (int i = 0; i < 10; i++)
                {
                    Random r = new Random();
                    Thread.Sleep(100 * r.Next(5));
                    Console.Write("{0}, ", i);
                }
                Console.WriteLine();
                Console.WriteLine(num + Thread.CurrentThread.Name);
            }//end Lock所有在这范围{... }内的代码都是线程安全的！
        }
    }
    #endregion
    //线程类
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("*****Synchronizing Threads *****\n");
            //资源类
            Printer p = new Printer();

            // Make 10 threads that are all pointing to the same method on the same object.
            //使10个线程全部指向同一个对象的同一个方法 
            Thread[] threads = new Thread[10000];
            for (int i = 0; i < 10000; i++)
            {
                threads[i] =
                  new Thread(new ThreadStart(p.PrintNumbers));
                threads[i].Name = string.Format("Worker thread #{0}", i);
            }
            // Now start each one.
            foreach (Thread t in threads)
                t.Start();
            Console.ReadLine();
        }
    }
}
