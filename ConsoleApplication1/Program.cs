using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            IList<int> list1 = new List<int>() { 1, 2, 3, 2 };
            IList<int> list2 = new List<int>() { 2, 3, 4 };
            //连接（合并）两个序列（不去重）
            //var list3 = list1.Concat(list2);
            //去重
            EqualityComparer<int> comparer = new EqualityComparer<int>();
            var list3 = list1.Distinct(comparer);
            foreach (var item in list3)
            {
                Console.WriteLine(item);
            }
        }
    }
    public class EqualityComparer<T> : IEqualityComparer<T>
    {
        public bool Equals(T x, T y)
        {
            return true;         
        }

        public int GetHashCode(T obj)
        {
            throw new NotImplementedException();
        }
    }

    public class GenericFather<T> where T:People,new()
    {
        private T t = new T();
        public int  GetValue()
        {
            return t.Age;
        }

    }
    public class People
    {
        public People(int age, string name)
        {
            Age = age;
            Name = name;
        }
        public int Age{get;set;}
        public string Name { get; set; }
    }
}
