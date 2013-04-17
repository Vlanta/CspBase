using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class People : IComparable<People>
    {
        public int Age { get; set; }
        public string Name { get; set; }
        public int CompareTo(People other)
        {
            if (this.Age > other.Age)
                return 1;
            else if (this.Age < other.Age)
                return -1;
            else
                return 0;
        }
    }
}
