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
            ob.Fun();
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
}
