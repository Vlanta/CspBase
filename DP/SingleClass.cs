using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DP
{
    //我的模式
    public class SingleClass
    {
        private static SingleClass Instance = new SingleClass();
        private SingleClass()
        { }
        public static SingleClass GetInstance()
        {
            return Instance;
        }
    }
    //懒汉模式
    public sealed class SingleClassReadonly
    {
        private static readonly SingleClassReadonly Instance = new SingleClassReadonly();
        private SingleClassReadonly()
        { }
        public static SingleClassReadonly GetInstance()
        {
            return Instance;
        }
    }
    //Lazy 模式
    public class Singleton
    {
        private static Singleton instance;
        private static object _lock = new object();

        private Singleton()
        {
        }

        public static Singleton GetInstance()
        {
            if (instance == null)
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new Singleton();
                    }
                }
            }
            return instance;
        }
    }

}
