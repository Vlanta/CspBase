using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadTimer
{
    //资源类，同时也是现场类
    class Program
    {
        //委托对应的方法：void XXX(object obj)
        static void PrintTime(object state)
        {
            Console.WriteLine("Time is: {0}, Param is {1}",
              DateTime.Now.ToLongTimeString(), state.ToString());
        }

        static void Main(string[] args)
        {
            Console.WriteLine("***** Working with Timer type *****\n");

            // Create the delegate for the Timer type.为Timer类型创建委托
            TimerCallback timeCB = new TimerCallback(PrintTime);

            // Establish timer settings.设置Timer类
            Timer t = new Timer(
              timeCB,     // The TimerCallback 委托对象
              "Hello From Main",       // 想传入的对象（null表示没有参数）Any info to pass into the called method (null for no info).
              0,          // 在开始之前，等待多长时间Amount of time to wait before starting.
              1000);      // 每次调用的间隔时间Interval of time between calls (in milliseconds).

            Console.WriteLine("Hit key to terminate...");
            Console.ReadLine();
        }
    }

}
