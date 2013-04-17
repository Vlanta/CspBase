using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XmlSerialization;

namespace ConsoleApplication3
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Person> list = new List<Person>() {
                new Person(){ID=1,Name="haige"},
                new Person(){ID=2,Name="xiaoyao"},
            };
            Classes cl = new Classes() {  ClassName="Tinghua", Persons=list};
            var xml = Class2.GetXmlDataDocument(cl);
            var xmlstring = xml.ToString();
        }
    }
}
