using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSerialization
{
    public class Person
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    public class Classes
    {
        public string ClassName { get; set; }
        public List<Person> Persons { get; set; }
    }
}
