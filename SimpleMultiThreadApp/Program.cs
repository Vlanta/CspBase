using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace SimpleMultiThreadApp
{

    //一个类，可以被产生线程的类调用，可以称之为“资源类”
    //同步机制主要是用在这里,而不是产生线程的类中（如，Lock）
    #region The Printer class
    public class Printer
    {
        public void PrintNumbers()
        {
            // Display Thread info.
            Console.WriteLine("-> {0} is executing PrintNumbers()",
              Thread.CurrentThread.Name);

            // Print out numbers.
            Console.Write("Your numbers: ");
            for (int i = 0; i < 10; i++)
            {
                Console.Write("{0}, ", i);
                Thread.Sleep(2000);
            }
            Console.WriteLine();
        }
    }
    #endregion
    //可以把它看成线程类，可以称之为“线程类”
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("***** The Amazing Thread App *****\n");
            Console.Write("Do you want [1] or [2] threads? ");
            string threadCount = Console.ReadLine();

            // Name the current thread.
            Thread primaryThread = Thread.CurrentThread;
            primaryThread.Name = "Primary";

            // Display Thread info.
            Console.WriteLine("-> {0} is executing Main()",
            Thread.CurrentThread.Name);

            //用于线程操作的“资源类”
            Printer p = new Printer();

            // Make worker class.产生次线程
            switch (threadCount)
            {
                case "2":
                    // Now make the thread.
                    /*Thread对象接受一个ParameterizedThreadStart(带参数)或ThreadStart(无参数)委托作为构造函数的参数
                     */
                    Thread backgroundThread =
                      new Thread(new ThreadStart(p.PrintNumbers));//ThreadStart委托:函数形式为void xxx()，无返回值，无参数
                    backgroundThread.Name = "Secondary";
                    backgroundThread.Start();
                    break;
                case "1":
                    p.PrintNumbers();
                    break;
                default:
                    Console.WriteLine("I don't know what you want...you get 1 thread.");
                    goto case "1";
            }

            // Do some additional work.
            MessageBox.Show("I'm busy!", "Work on main thread...");
            Console.ReadLine();
        }

    }
}

