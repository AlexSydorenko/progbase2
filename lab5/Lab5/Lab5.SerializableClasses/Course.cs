using System;
using System.Xml.Serialization;

namespace lab5
{
    [Serializable]
    public class Course
    {
        public string footnote;

        [System.ComponentModel.DefaultValue(0)]
        public int sln;
        public string prefix;

        [XmlElement("crs")]
        public int courseCode;
        public string lab;

        [XmlElement("sect")]
        public int sector;
        public string title;
        public string credit;

        [System.ComponentModel.DefaultValue("")]
        public string days;

        [XmlElement("times")]
        public Time times;
        public Place place;
        public string instructor;
        public int limit;
        public int enrolled;

        public Course() { }

        public override string ToString()
        {
            return string.Format($"Footnote: {this.footnote}\n" +
                $"Sln: {this.sln}\n" +
                $"Prefix: {this.prefix}\n" +
                $"Course code: {this.courseCode}\n" +
                $"Lab: {this.lab}\n" +
                $"Sector: {this.sector}\n" +
                $"Title: {this.title}\n" +
                $"Credit: {this.credit}\n" +
                $"Days: {this.days}\n" +
                $"Time:\n" +
                    $" Start: {this.times.startTime}\n" +
                    $" End: {this.times.endTime}\n" +
                $"Place:\n" +
                    $" Building: {this.place.building}\n" +
                    $" Room: {this.place.room}\n" +
                $"Instructor: {this.instructor}\n" +
                $"Limit: {this.limit}\n" +
                $"Enrolled: {this.enrolled}\n");
        }
    }
}
