using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProfilerSample
{
    /*http://blog.zhaojie.me/2009/12/profiler-sampling.html
     * 
     */
    class Program
    {
        static void Main(string[] args)
        {
            var array = Enumerable.Range(1, 5).Select(i => new String((char)(i + 97), 5)).ToArray();
            TestConvert(array);
            TestParse(array);
            TestTryParse(array);
        }

        private static List<Int32> TestParse(String[] strings)
        {
            Int32 intValue;
            List<Int32> intList = new List<int>();

            for (int i = 0; i < 100000; i++)
            {
                intList.Clear();

                foreach (String str in strings)
                {
                    try
                    {
                        intValue = Int32.Parse(str);
                        intList.Add(intValue);
                    }

                    catch (System.ArgumentException)
                    { }

                    catch (System.FormatException)
                    { }

                    catch (System.OverflowException)
                    { }
                }

            }

            return intList;
        }

        private static List<Int32> TestTryParse(String[] strings)
        {
            Int32 intValue;
            List<Int32> intList = new List<int>();
            Boolean ret;

            for (int i = 0; i < 100000; i++)
            {
                intList.Clear();

                foreach (String str in strings)
                {
                    ret = Int32.TryParse(str, out intValue);
                    if (ret)
                    {
                        intList.Add(intValue);
                    }
                }
            }

            return intList;
        }

        private static List<Int32> TestConvert(String[] strings)
        {
            Int32 intValue;
            List<Int32> intList = new List<int>();

            for (int i = 0; i < 100000; i++)
            {
                intList.Clear();

                foreach (String str in strings)
                {
                    try
                    {
                        intValue = Convert.ToInt32(str);
                        intList.Add(intValue);
                    }

                    catch (System.FormatException)
                    { }

                    catch (System.OverflowException)
                    { }
                }
            }

            return intList;
        }
    }
}
