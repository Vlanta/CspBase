using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinallyTest
{
    class Program
    {
        /*
         * 对于未处理的异常,CLR会终止程序
         * 并且，“异常栈”中所有finally块都不会执行
         * 注：asp.net（WinForm也是）对于未抛出的异常进行了特殊处理(具体怎么处理不知道)，使得进程不会终结，并把异常传回了客户的
         * asp.net提供了三级的异常处理机制，1页面 2...3...
         * WinForm会弹出一个提示框，让你选择终止还是继续应用程序
         * 
         * Finally永远会执行
         * 1 Finally主要用于释放非托管资源
         * 2 不要再Finally中操作返回变量
         *   因为的它的执行顺序是先暂存返回变量，然后执行finally块，最后把暂存变量返回
         *   另，在Finally不能写返回语句，因为没有意思
         */
        static void Main(string[] args)
        {
            try
            {
                throw new Exception("Exception Demo");
            }
            finally
            {
                Console.WriteLine("finally excuted....");
            }
        }
        static int TestFinallyReturn()
        {
            int n = 10;
            try
            {
                throw new Exception("Exception Test");

            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                n = 20;
                //编译器报错：不能在这里写返回语句
                //return n;
            }
        }
    }
}
