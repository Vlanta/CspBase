using System;
using System.Collections.Generic;
using System.Text;

namespace CharDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Program.TestCharConvert();

        }
        public static void TestCharConvert()
        {
            char a = 'a';
            Console.WriteLine(a);
            /*字符和数字的转换*/
            //错误的做法：这里的强制转换(char)1的结果是码值为1的Unicode字符
            char c1 = (char)1;
            Console.WriteLine(c1);
            char c2 = Convert.ToChar(2);
            Console.WriteLine(c2);
            //正确的做法:
            char c3 =  3.ToString().ToCharArray()[0];
            Console.WriteLine(c3);
            char c4 = (char)(4 + '0');
            Console.WriteLine(c4);
            int int5 = '5' - '0';
            Console.WriteLine(int5);
            //
        }
    }
}
