using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class PeopleWithoutDefaultNew
    {
        public PeopleWithoutDefaultNew(int age, string name)
        {
            Age = age;
            Name = name;
        }
        public int Age { get; set; }
        public string Name { get; set; }
    }
}
