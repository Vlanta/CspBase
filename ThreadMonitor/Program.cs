using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.Remoting.Contexts;

namespace ThreadMonitor
{
    //资源类
    #region Printer helper class
    public class Printer
    {
        /*如果需要锁定公共成员中的一段代码，比较安全（也比较推荐）的方式是声明私有的object
         * 成员来作为锁标识
         */
        // Lock token.锁标识
        private object threadLock = new object();
        private int num = 0;
        public void PrintNumbers()
        {
            /*可以认为Lock是Monitor的简化版本
             * 而Monitor意味着更好的控制能力，如
             * 1 使用Monitor.Wait()指示活动的线程等待一段时间
             * 2 当现场完成操作时，使用Monitor.Pulse()或Monitor.PulseAll()通知等待中的线程
             */
            Monitor.Enter(threadLock);
            try     //所有在这范围try{... }内的代码都是线程安全的！
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
            }//end try{}所有在这范围{... }内的代码都是线程安全的！
            finally
            {
                //finally子句保证了无论出现什么运行错误，线程标记都能被释放(通过Monitor.Exit()方法)
                Monitor.Exit(threadLock);
            }
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


    /*其他同步原语
     * 1 Interlocked 同步单个值
     * 2 使用[Synchronization]特性进行同步
     */
    public class InterlockedClassDemo
    {
        private long intVal;
        InterlockedClassDemo(long val)
        {
            this.intVal = val;
        }
        public long  AddOne()
        {
            return Interlocked.Increment(ref this.intVal);
        }
    }
    /*这个特性只能用在类上，则类中的全部方法都是线程安全的
     * 缺点就是：即使一个方法没有使用线程敏感数据，CLR仍会锁定对该方法的调用
     * 这会全面降低性能
     */
     [Synchronization]
    public class SynClassDemo
    {      
        public int Age { get; set; }
    }
}
