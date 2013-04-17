using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArrayDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 100; i++)
            {
                int len = new Random().Next(1, 100);
                int[] array = new int[len];
                Console.WriteLine(array.Length);
            }
        }
    }
}
