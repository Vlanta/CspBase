using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**
 * 在C#中调用抽象方法是运用callvirt指令
 * 在实现类的类型对象中存储了它继承的虚方法和抽象方法
 * 所以，如果一个类C实现了接口I(其中有一个方法Func())，那么它的类型对象的方法列表中就存在
 * 两个Func()，这两个Func()指向同样的代码（如果没有显示实现接口的话）
 */
namespace Abstractor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("---------------------调用抽象类/接口----------------------");
            IFather a = new Son();
            //Father a = new Son();
            a.show();
            Console.WriteLine("-------------------调用实现类-------------------");
            Son SON = new Son();
            SON.show();
            Console.WriteLine("----------------------------------------------------");
            IFather B = new BaseClass();
            AbstractIFather C = new BaseClass();
            B.show();
            C.show();
        }
    }
    //-------------------------------实现抽象类与接口的区别-----------------------------------------
    /*
     * 
     */

    public class Son : IFather
    //public class Son : Father
    {
        //重写抽象方法
        //public   override void show()
        //{
        //    Console.WriteLine("重写抽象方法");
        //}
        //实现接口
        public void show()
        {
            Console.WriteLine("子类的方法");
        }

        //显式实现接口    
        void IFather.show()
        {
            Console.WriteLine("显式实现接口");
        }
    }
    public abstract class Father
    {
        public abstract void show();
    }
    public interface IFather
    {
        void show();
    }
    //---------------------------------抽象类实现接口-----------------------------------------------------------------------
    public abstract class AbstractIFather : IFather
    {
        //如果抽象类实现接口，则可以把接口中方法映射到抽象类中作为【抽象方法】而不必实现，
        //而在抽象类的子类中实现接口中方法.
        public abstract void show();
    }
    public class BaseClass : AbstractIFather
    {
        public override void show()
        {
            Console.WriteLine("BaseClass : AbstractIFather : IFather");
        }
    }
}
