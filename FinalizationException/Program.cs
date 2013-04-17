using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FinalizationException
{
    class Program
    {
        static void Main(string[] args)
        {
            A a = new A();
            a = null;
            Console.WriteLine("GC.Collecting........");
            GC.Collect();
            Console.WriteLine("Main Thread sleeping......");
            Thread.Sleep(10000);
            GC.Collect();
            Thread.Sleep(10000);

        }
    }
    public class A
    {
        public A()
        {
            Console.WriteLine(" A()");
        }
        ~A()
        {
            Console.WriteLine("~ A()");
            Console.WriteLine("Throw  Exception in ~A()");
            throw new Exception(" Finalization Exception");
        }
    }
}
