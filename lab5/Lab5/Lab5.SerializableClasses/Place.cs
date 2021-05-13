using System;
using System.Xml.Serialization;

namespace lab5
{
    public class Place
    {
        [XmlElement("bldg")]
        public string building;
        public string room;
    }
}
