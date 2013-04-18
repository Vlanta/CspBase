using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventDemo
{
    //定义一个委托类型
    public delegate string deleFun(string word);

    public class Server
    {
       
        //声明一个委托变量
        public deleFun deleSay;
        //声明一个事件变量
        public event deleFun eventSay;

        public void doEventSay(string str)
        {
            if (eventSay != null)
                eventSay(str);
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Server S = new Server();
            S.eventSay += t_say;
            S.deleSay += t_say;
            S.deleSay += t_say2;
            //t.eventSay("eventSay"); 错误 事件不能在外部直接调用
            S.doEventSay("eventSay");//正确 事件只能在声明的内部调用
            string str = S.deleSay("deleSay");//正确 委托可以在外部被调用 当然在内部调用也毫无压力 而且还能有返回值（返回最后一个注册的方法的返回值）
            Console.WriteLine(str);
            Console.Read();
        }


        static string t_say(string word)
        {
            Console.WriteLine(word);

            return "return " + word;
        }

        static string t_say2(string word)
        {
            Console.WriteLine(word);

            return "return " + word + " 2";
        }
    }


}
