using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;

namespace GenericUtil
{
   /*
    * 泛型最漂亮的功能是：可以设置类型T继承只那个类，接口等，
    * 这样就可以在泛型类中使用它的方法，属性等（如这里的,T.Age）
    */
    public class GenericFather<T>where T:People,new()
    { 
        private T t = new T();
        public int  GetValue()
        {
            return t.Age;
        }
    }
    public abstract class GenericFather2<T> where T : People, new()
    {
        protected T t = new T();
        public int GetAge()
        {
            return t.Age;
        }
        public abstract string GetName();
       
    }
    /*泛型继承必须遵守一些规则
     * 1 必须制定类型参数
     */
    public class GenericSon1 : GenericFather<Coder>
    {
    }
   /*
    * 2 如果泛型基类定义了虚拟方法或抽象方法，派生类型必须使用【指定类型参数】重写泛型方法
    */
    public class GenericSon2 : GenericFather2<Coder>
    {
        public override string GetName()
        {
            return base.t.Name;
        }
    }
    /*
    * 3 如果派生类也是泛型，则它能够（可选地）重用类型占位符。
    * 不过要注意派生类型必须遵照基类中的任何约束
    */
    public class GenericSon2<T> : GenericFather2<Coder>
    {
        public override string GetName()
        {
            return base.t.Name;
        }
    }
   
    public class GenericSon4<T> : GenericFather2<T> where T:People,new()
    {
        public override string GetName()
        {
            return base.t.Name;
        }
    }
    /*
     * 泛型约束
     * class（引用类型)或struct(值类型)必须在最前
     * new()必须在最后
     */
    public class GenericConstraint<T,X> where T:Coder,IEnumerable<T> ,new () where X:struct
    {

    }
}
