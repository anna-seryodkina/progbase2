using System.Xml.Serialization;

namespace ConsoleProject
{
    public class Time
    {
        [XmlElement("start_time")]
        public string startTime;
        [XmlElement("end_time")]
        public string endTime;
    }
}
