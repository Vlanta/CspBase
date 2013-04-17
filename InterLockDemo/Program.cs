using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace InterLockDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            int val = 10;
            //以原子操作的形式，将 32 位有符号整数设置为指定的值并返回原始值
            Console.WriteLine(Interlocked.Exchange(ref val, 11)+"    "+val);
            Console.WriteLine(Interlocked.Add(ref val, 2));
            //CompareExchange(ref int location1, int value, int comparand);
            //比较两个 32 位有符号整数是否相等，如果相等，则替换其中一个值
            //location1: 其值将与 comparand 进行比较并且可能被替换的目标
            //value: 比较结果相等时替换目标值的值
            //comparand: 与位于 location1 处的值进行比较的值
            //返回结果: location1 中的原始值。
            Console.WriteLine(Interlocked.CompareExchange(ref val,11,10));
            
        }
    }
}
