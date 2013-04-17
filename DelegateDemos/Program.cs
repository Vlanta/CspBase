using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace DelegateDemos
{
    class Program
    {
        public int i = 0;
        //DelegateA是个类型
        public delegate void DelegateA();
        //这个是DelegateA委托类型的变量
        public DelegateA da;

        public static void Func()
        {
            Console.WriteLine("Func()s");
            throw new Exception();
        }
        public static void CallBack(IAsyncResult ar)
        {
            Console.WriteLine("CallBack() called");
            var bb = (AsyncResult)ar;
            var delegateClass = (DelegateA)bb.AsyncDelegate;
           // delegateClass.EndInvoke(ar);
 
        }
        static void Main(string[] args)
        {
            Program p = new Program();
         
            p.da = new DelegateA(Func);
            //如果只运行BeginInvoke,则函数A会运行
            //就算AsyncCallback参数不为null,也会运行
            //调用EndInvoke只是取得相应的结果值才
            //如果委托Func抛出异常，如果不调用EndInvoke异常不会抛出的
           var xxx =  p.da.BeginInvoke(new AsyncCallback(CallBack),null);
           Thread.Sleep(5000);
        }
    }
}
