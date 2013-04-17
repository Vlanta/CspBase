using System;
using System.Collections.Generic;
using System.Text;

namespace WhileException
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter While{---}");
           //// bool breaking=false;
            while (true)
            {
                try
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Console.WriteLine(i);
                    }
                    Console.WriteLine("Throw Exception in while innerFunction");
                    fun();
                }
                catch (Exception e)
                {
                    Console.WriteLine(" Exception  :" + e.Message);
                    break;
                    /////breaking = true;
                }
                ////finally
                ////{
                ////    Console.WriteLine("break While in finally");
                ////这样写是错误的，将得到错误提示：控制不能离开finally主体
                ////    break;
                ////}
            }//end while
            Console.WriteLine("End Mian()");
        }
        static void fun()
        {
            throw new Exception("Hebe");
        }
    }
}
