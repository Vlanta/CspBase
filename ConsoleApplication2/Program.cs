using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            BaseClass a = new SonClass();
             //IL_0008:  callvirt   instance void ConsoleApplication2.BaseClass::func()
            a.func();
            SonClass s = new SonClass();
            //IL_0015:  callvirt   instance void ConsoleApplication2.BaseClass::func()
            s.func();
        }
    }
    public class BaseClass
    {
        public virtual void func()
        {
 
        }
    }
    public class SonClass : BaseClass
    {
        public override void func()
        {
           
        }
    }
}
