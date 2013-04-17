using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StringDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            TestEncode();
        }
        private static void TestEncode()
        {
            string source = @"我是位传海1";
            Console.WriteLine(source.Length);
            //将字符串编码成字节数组
            byte[] b = Encoding.GetEncoding("utf-8").GetBytes(source);
            Console.WriteLine(BitConverter.ToString(b));
            string change = Encoding.GetEncoding("utf-8").GetString(b);
            Console.WriteLine(BitConverter.ToString(b));
            Console.WriteLine(change.Length);
            Console.WriteLine(change.ToCharArray().Length);

        }
        private static void TestInterned()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('b');
            sb.Append('c');
            sb.Append('d');
            string s1 = sb.ToString();
            string str1 = "bcd";
            var sc1 = str1.Substring(1, 2);
            string str2 = "bcd";
            sb.Clear();
            sb.Append('b');
            sb.Append('c');
            string s2 = sb.ToString();
            Console.WriteLine("str1={0},str2={1}=>{2}", str1, str2, Object.ReferenceEquals(str1, str2));
            Console.WriteLine("str1={0},s1={1}=>{2}", str1, s1, Object.ReferenceEquals(str1, s1));
            Console.WriteLine(@"string.IsInterned(str1)={0}", string.IsInterned(str1));
            Console.WriteLine(@"string.IsInterned(sc1)={0}", string.IsInterned(sc1));
            Console.WriteLine(@"string.IsInterned(s2)={0}", string.IsInterned(s2));
            Console.WriteLine(@"string.IsInterned(bcd)={0}", string.IsInterned("bcdefg"));
        }
    }
}
