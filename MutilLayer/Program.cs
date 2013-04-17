using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MutilLayer
{
    class MutilLayer
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-------------虚函数调用---------------");
            Console.WriteLine("【L1 call  L3 instance】");
            L1 l = new L3();
            l.Func();
            Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("【L2 call  L3 instance】");
            ((L2)l).Func();
            Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("【L2 call  L3 instance】");
            L2 ll = new L3();
            ll.Func();
            Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("【L1 call  L2 instance】");
            L1 lll = new L2();
            lll.Func();
            Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("【L1 call  L1 instance】");
            L1 llll = new L1();
            llll.Func();
            Console.WriteLine("-----------------非虚函数调用--------------------");
            Console.Write("s1v2    ");
            L1 s1v2 = new L2();
            s1v2.Function();
            Console.Write("s2v2    ");
            L2 s2v2 = new L2();
            s2v2.Function();
            Console.Write("s1v3    ");
            L1 s1v3 = new L3();
            s1v3.Function();
            Console.Write("s2v3    ");
            L2 s2v3 = new L3();
            s2v3.Function();
            Console.Write("s3v3    ");
            L3 s3v3 = new L3();
            s3v3.Function();
        }

    }
    public class L1
    {
        public void CallIT()
        {
            Func();
        }
        public virtual void Func()
        {
            Console.WriteLine("L1");
        }
        public void Function()
        {
            Console.WriteLine("L1: Function()");
        }
    }
    public class L2:L1
    {
        //重新L1中的虚方法
        public  override void Func()
        {
            Console.WriteLine("L2");
        }
       
        //隐藏父类的同名虚函数，同时定义了一个新的同名虚函数
        /*这会产生有趣的现象
         * 1 L1 l = new L3(); L1.Func();//将调用L1中定义的虚函数
         * 2 L2 l = new L3(); L2.Func();//将调用L3中重新的函数
         */
        //public new virtual void Func()
        //{
        //    Console.WriteLine("L2");
        //}
        //加new 和不加new的区别
        /*在本质上区别不大，因为都将覆盖父类中的同名函数
         * 不加new将产生编译警告
         * 加new代码更加清晰，并且将消除编译警告
         */
        public new void Function()
        {
            Console.WriteLine("L2: Function()");
        }
    }
    public class L3 : L2
    {
        public override void Func()
        {
            Console.WriteLine("-----Begin call base in L3-----");
            base.Func();
            //Console.WriteLine("L3");
            Console.WriteLine("-----End   call base in L3-----");
        }
     
    }
    
}
