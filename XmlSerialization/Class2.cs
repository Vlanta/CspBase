using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace XmlSerialization
{
    public class Class2
    {
        public static XmlDataDocument GetXmlDataDocument(Classes classes)
        {
             XmlDataDocument x = new XmlDataDocument();
            using (Stream s = new MemoryStream())
            {
                XmlSerializer xmlSerial = new XmlSerializer(typeof(Classes));
                xmlSerial.Serialize(s, classes);
                s.Seek(0, SeekOrigin.Begin);
                x.Load(s);
            }
            return x;
        }
    }
}
