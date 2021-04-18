using System.Xml.Serialization;

namespace ConsoleProject
{
    public class Course
    {
        [XmlElement("reg_num")]
        public int regNum;
        public string subj;
        [XmlElement("crse")]
        public int course;
        public string sect;
        public string title;
        public double units;
        public string instructor;
        public string days;
        public Time time;
        public Place place;
    }
}
