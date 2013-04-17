using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;

namespace CollectionsUtil
{
    public class ListDemo
    {
        private static List<int> list1 = new List<int>() { 1, 2, 3 };
        private static List<int> list2 = new List<int>() { 3, 3, 3 };
        private static List<int> list3 = new List<int>() { 4, 5, 6 };
        private static List<People> peopleList = new List<People>();
        //private static SortedList<int, People> sortedList = new SortedList<int>();

        public bool TestSort()
        {
            peopleList.Add(new People() { Age = 1, Name = "first" });
            peopleList.Add(new People() { Age = 3, Name = "third" });
            peopleList.Add(new People() { Age = 2, Name = "second" });
            //before sort
            for (int i = 0; i < peopleList.Count; i++)
            {
                Console.WriteLine(peopleList[i].Age);
            }
            //after sort
            /*运用传入的比较器*/
            //peopleList.Sort(new PeopleComparer());
            /*运用对象自己的比较函数，如果对象没有会抛出异常*/
            peopleList.Sort();
            for (int i = 0; i < peopleList.Count; i++)
            {
                Console.WriteLine(peopleList[i].Age);
            }
            if (peopleList[1].Age == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

   /// <summary>
   /// 自定义比较器
   /// </summary>
    public class PeopleComparer : IComparer<People>
    {
        public int Compare(People x, People y)
        {
            if (x.Age > y.Age)
                return 1;
            else if (x.Age < y.Age)
                return -1;
            else
                return 0;
        }
    }
}
