using System;
using System.Xml.Serialization;

namespace lab5
{
    public class Time
    {
        [XmlElement("start")]
        public string startTime;

        [XmlElement("end")]
        public string endTime;
    }
}
