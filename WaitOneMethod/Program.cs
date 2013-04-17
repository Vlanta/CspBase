using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace WaitOneMethod
{
    /*
     * AutoResetEvent中建议使用WaitOne()而不是Reset()
     * 如，在自定义锁中的Enter方法中就应该使用WaitOne();
     * 因为Reset总是会执行，而WaitOne要等待事件可用才会继续向下执行
     * 
     */
    class Program
    {
        static AutoResetEvent autoEvent = new AutoResetEvent(true);

        static void Main()
        {
            //WaitOneMethod();
            exitContextMethod();
        }
        private static void WaitOneMethod()
        {
            autoEvent.WaitOne();
            Console.WriteLine(autoEvent.SafeWaitHandle.DangerousGetHandle());
        }
        private static void exitContextMethod()
        {
            Console.WriteLine("Main starting.");

            ThreadPool.QueueUserWorkItem(
                new WaitCallback(WorkMethod), autoEvent);
            Thread.Sleep(5000);
            //autoEvent.Set();
            // Wait for work method to signal.
            if (autoEvent.WaitOne(Timeout.Infinite, false))
            {
                Console.WriteLine("Work method signaled.");
            }
            else
            {
                Console.WriteLine("Timed out waiting for work " +
                    "method to signal.");
            }
            Console.WriteLine("Main ending.");
        }

        static void WorkMethod(object stateInfo)
        {
            Console.WriteLine("Work starting.");
            ((AutoResetEvent)stateInfo).WaitOne(); Console.WriteLine("Work WaitOne.");
            //((AutoResetEvent)stateInfo).Reset(); Console.WriteLine("Work Reset.");
            
            // Simulate time spent working.
            Thread.Sleep(new Random().Next(100, 2000));

            // Signal that work is finished.
            Console.WriteLine("Work ending.");
            ((AutoResetEvent)stateInfo).Set();Console.WriteLine("Work Set Event to true");
        }
        /*
         * .net 使用Mutex 同步基元，它只向一个线程授予对共享资源的独占访问权。
         * 如果一个线程获取了互斥体，则要获取该互斥体的第二个线程将被挂起，
         * 直到第一个线程释放该互斥体。
         * 上面话的意思是， 如果你call mutex.WaitOne 之前已经在synchronized context里面
         * （也就是说你已经拥有这个mutex),  
         * 你再call mutex.WaitOne(1000,true) 的话，你的这个thread 会先退出context, 
         * 也就是放弃你目前拥有的mutex, 然后再排队等mutex。这是参数为true的情况，
         * false的话thread如果已经在synchronized context里面， 就直接使用拥有的这个mutex了。
         */
    }
}
