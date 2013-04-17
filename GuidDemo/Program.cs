using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuidDemo
{
    /*全局唯一标识符Guid,为一个128位的整数(数学概念上的)(16字节)
     * 它保证对在同一时空中的所有机器都是唯一的 
     * 生成算法很有意思，用到了以太网卡地址、纳秒级时间、芯片ID码和许多可能的数字。
     * GUID的唯一缺陷在于生成的结果串会比较大。
     * 注：生成的GUID可用于数据库的主键
     */
    class Program
    {
        static void Main(string[] args)
        {
            string a = "1111";
            var b=a.ElementAt(0);

            Console.WriteLine(Guid.NewGuid().ToString());
            Console.WriteLine(Guid.NewGuid().ToString());
            Console.WriteLine(Guid.NewGuid().ToString("D"));
            Console.WriteLine(Guid.NewGuid().ToString("D"));
            Console.WriteLine(Guid.NewGuid().ToString("N"));
            Console.WriteLine(Guid.NewGuid().ToString("N"));         
            Console.WriteLine(Guid.NewGuid().ToString("B"));
            Console.WriteLine(Guid.NewGuid().ToString("B"));
            Console.WriteLine(Guid.NewGuid().ToString("P"));
            Console.WriteLine(Guid.NewGuid().ToString("P"));
        }
    }
}
