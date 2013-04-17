using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unsafe
{
    class Program
    {
        static void Main(string[] args)
        {
            Base b = new Son();
            b.CallIT();
            Console.WriteLine(((Son)b).i);
        }
    }
    public class Base
    {
        public void CallIT()
        {
            Func();
        }
        public virtual void Func()
        {
            Console.WriteLine("Base Func");
        }
    }

    public class Son : Base
    {
        public int i=0;
        public override void Func()
        {
            i++;
        }
    }
}
