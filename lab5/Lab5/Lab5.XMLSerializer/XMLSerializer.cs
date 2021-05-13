using System;
using System.IO;
using System.Xml.Serialization;

namespace lab5
{
    public class XMLSerializer
    {
        public string filePath;
        public XMLSerializer(string filePath)
        {
            this.filePath = filePath;
        }

        public Courses Deserialize()
        {
            XmlSerializer ser = new XmlSerializer(typeof(Courses));
            StreamReader reader = new StreamReader(filePath);
            Courses courses = (Courses)ser.Deserialize(reader);
            reader.Close();

            return courses;
        }

        public void Serialize(Courses courses)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Courses));
            StreamWriter writer = new StreamWriter(filePath);
            ser.Serialize(writer, courses);
            writer.Close();
        }
    }
}
