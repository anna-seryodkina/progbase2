using System.Collections.Generic;
using System.Xml.Serialization;

namespace ConsoleProject
{
    [XmlRoot("root")]
    public class Root
    {
        [XmlElement("course")]
        public List<Course> courses;
    }
}
