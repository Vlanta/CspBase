using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CollectionsUtil
{
    public class HashSetUtil
    {
        /// <summary>
        /// 求两个Hash集合的差集（不改变原集合）
        /// 将返回其不在 second 中的元素
        /// 注释：Except方法是IEnumrable的方法，也可用于List等
        /// </summary>
        /// <param name="first"></param>
        /// <param name="compararSet"></param>
        /// <returns></returns>
        public static IList<int> setExcept(HashSet<int> first, HashSet<int> second)
        {
          return first.Except<int>(second).ToList<int>();    
        }
    }
}
