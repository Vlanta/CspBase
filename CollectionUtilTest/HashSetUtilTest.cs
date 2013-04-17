using CollectionsUtil;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace CollectionUtilTest
{
    
    
    /// <summary>
    ///这是 HashSetUtilTest 的测试类，旨在
    ///包含所有 HashSetUtilTest 单元测试
    ///</summary>
    [TestClass()]
    public class HashSetUtilTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        // 
        //编写测试时，还可使用以下特性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///setExcept 的测试
        ///</summary>
        [TestMethod()]
        public void setExceptTest()
        {
            HashSet<int> first = new HashSet<int>() { 1,2,3}; // TODO: 初始化为适当的值
            //first.Add(1);
            //first.Add(2);
            //first.Add(3);
          
            HashSet<int> second = new HashSet<int>() { 2, 4, 5 };// TODO: 初始化为适当的值
            //first.Add(2);
            //first.Add(4);
            //first.Add(5);
            IList<int> expected = new List<int>(){1,3}; // TODO: 初始化为适当的值
            IList<int> actual;
            actual = HashSetUtil.setExcept(first, second);
            Assert.AreEqual(expected.Count, actual.Count);
            Assert.AreNotEqual(actual.Count,first.Count);
            Assert.AreNotEqual(actual.Count, second.Count);
       

        }
    }
}
