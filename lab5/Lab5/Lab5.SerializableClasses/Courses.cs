using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace lab5
{
    [Serializable, XmlRoot("root")]
    public class Courses
    {
        [XmlElement("course")]
        public List<Course> listCourses;

        public Courses() { }
    }
}
