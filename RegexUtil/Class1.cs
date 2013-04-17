using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RegexUtil
{
    public class Class1
    {
        public static void Main(string[] args)
        {
            //正则表达式的一般用法
            Regex regex = new Regex(@"\w+=\w+");
            Match match = regex.Match("          @@@@123=123=");
            if (match.Success)
            {
                Console.WriteLine(match.Groups[0].Value);
            }

            //int[] groupnums = regex.GetGroupNumbers();     
            //while(match.Success)
            //{
            //    Console.WriteLine(match.Groups[0].Value);
            //    match = match.NextMatch();
            //}
        }


    }
}
