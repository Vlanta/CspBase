using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirturalMethod
{
    public class Phone
    {
        public static void Main(string[] args)
        {
            Phone p = new BetterPhone();
            p.Dial();
            Console.WriteLine("-------------------------------------------------------");
            BetterPhone bp = new BetterPhone();
            bp.Dial();
 
        }
        public void Dial()
        {
            Console.WriteLine("Phone.Dial");
            EstablishConnection();      
        }

        protected virtual void EstablishConnection()
        {
            Console.WriteLine("Phone.EstablishConnection");     
        }
    }

    public class BetterPhone : Phone
    {

        new public void Dial()
        {
            Console.WriteLine("BetterPhone.Dial");
            EstablishConnection();
            base.Dial();
        }
         protected override void EstablishConnection()
        //new protected virtual void EstablishConnection()
        {
            Console.WriteLine("BetterPhone.EstablishConnection");

        }
    }
}
