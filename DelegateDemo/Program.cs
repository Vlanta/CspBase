using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DelegateDemo
{
    class Program
    {
        public Action<string> delegate1;
        static void Main(string[] args)
        {
            Program p = new Program();
            //可将方法重复添加到委托链中
            p.delegate1 += Func;
            p.delegate1 += Func;
            p.delegate1 += Func;      
            Action<string> aa = new Action<string>(Func);
            p.delegate1 += aa; p.delegate1 += aa;
            p.delegate1 += Func2;      
            p.delegate1("test");
            Console.WriteLine("---------------GetInvocationList方法------------------");
            var list = p.delegate1.GetInvocationList();
            Console.WriteLine(list.Count());
            Console.WriteLine(list[0]==list[1]);
            Console.WriteLine(list[0] == list[2]);
            Console.WriteLine(list[0] == list[3]);
            Console.WriteLine(list[0] == list[4]);
            Console.WriteLine(list[0] == list[5]);
            foreach(var item in list)
            {
                Console.WriteLine(item.GetHashCode());
            }

        }
        static void Func(string msg)
        {
            Console.WriteLine("Func({0}) invoking.........",msg);
            Thread.Sleep(500);
        }
        static void Func2(string msg)
        {
            Console.WriteLine("Func2({0}) invoking.........", msg);
            Thread.Sleep(500);
        }
    }
}
