using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace RandomDemo
{
    /*
     * Random类是一个产生伪随机数字的类
     * 它的构造函数有两种，一个是直接New Random()，另外一个是New Random(Int32)
     * 前者是根据触发那刻的系统时间做为种子，来产生一个随机数字
     * 后者可以自己设定触发的种子，一般都是用UnCheck((Int)DateTime.Now.Ticks)做为参数种子
     * -----------------------------------------------------------------------------------------------------------------------------
     * 伪随机数是以相同的概率从一组有限的数字中选取的。所选数字并不具有完全的随机性，但是从实用的角度而言，其随机程度已足够了。
     * 伪随机数的选择是从随机种子开始的，所以为了保证每次得到的伪随机数都足够地“随机”，随机种子的选择就显得非常重要。
     * 如果随机种子一样，那么同一个随机数发生器产生的随机数也会一样。
     * 一般地，我们使用同系统时间有关的参数作为随机种子，这也是.net Framework中的随机数发生器默认采用的方法。
     */
    class Program
    {
        static void Main(string[] args)
        {
            long longtick = DateTime.Now.Ticks;
            int seed = (int)(longtick & 0xffffffffL) | (int)(longtick >> 32);
            Console.WriteLine("-----------------------------随机数种子一样------------------------------");
            var r1 = new Random(seed);
            for (int i = 0; i < 10; i++)
            {
                Console.Write(r1.Next(20) + "  ");
            }
            Console.Write("\n");
            var r2 = new Random(seed);
            for (int i = 0; i < 10; i++)
            {
                Console.Write(r2.Next(20) + "  ");
            }
            Console.Write("\n");

            Console.WriteLine("-----------------------------随机数去重---------------------------------------");
            Removeduplicate(seed);
            MyRemoveduplicate(seed);
            Console.WriteLine("--------------------------------------------------------------------");
        }
        static void Removeduplicate(int seed)
        {
            //Dictionary<int, int> dic = new Dictionary<int, int>();其实用字典更好，应该字典可以保证没有重复的值
            Hashtable hashtable = new Hashtable();
            Random rm = new Random(seed);
            int RmNum = 10;
            for (int i = 0; hashtable.Count < RmNum; i++)
            {
                int nValue = rm.Next(20);
                if (!hashtable.ContainsValue(nValue) && nValue != 0)
                {
                    hashtable.Add(nValue, nValue);
                    Console.Write(nValue+"  ");
                }
            }
            Console.Write("\n");
        }
        static void MyRemoveduplicate(int seed)
        {
            //其实用字典更好，应该字典可以保证没有重复的值
            //.NET如果添加重复的键会抛异常(Fuck)
            //JAVA会覆盖具有相同键的值
            Dictionary<int, int> dic = new Dictionary<int, int>();
          
            Random rm = new Random(seed);
            int RmNum = 10;
            while (RmNum > dic.Count)
            {
                int nValue = rm.Next(20);
                if (dic.ContainsKey(nValue))
                    continue;
                dic.Add(nValue, nValue);
            }
            for (int i = 0; i < dic.Count; i++)
            {
                Console.Write(dic.ElementAt(i).Value+"  ");
            }
            Console.Write("\n");
        }
        static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
    }
    /*
     * Random快速连续产生相同随机数的解决方案:
     * 1、延时的办法。可以采用for循环的办法，也可以采用Thread.Sleep(100)
     * 2、提高随机数不重复概率的种子生成方法,
     *        如上面的new Random( GetRandomSeed( ))或new Random(new Guid().GetHashCode());
     */
}
