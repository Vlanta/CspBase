using System;
using System.Collections.Generic;
using System.Text;

namespace SomeFun
{
    class Program
    {
        static void Main(string[] args)
        {
            A ob = new C();
            //输出:B.V()
            ob.Fun();

            A ob2 = new B1();
            //输出:B.V()
            ob2.Fun();
        }
    }
    public class A
    {
        public void Fun()
        {
            this.V();
        }
        public virtual void V()
        {
            Console.WriteLine("A .V()");
        }
    }
    public class B:A
    {
        public override void V()
        {
            Console.WriteLine("B .V()");
        }
    }
    public class C : B
    {
        public new void V()
        {
            Console.WriteLine("C .V()");
        }
    }
    public class B1 : A
    {
        public new void V()
        {
            Console.WriteLine("B1 .V()");
        }
 
    }
}
