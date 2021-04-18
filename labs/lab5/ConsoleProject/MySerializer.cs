using System;
using System.IO;
using System.Xml.Serialization;

namespace ConsoleProject
{
    static class MySerializer
    {
        public static void Serialize(string filename, Root root)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Root));
            //
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineHandling = System.Xml.NewLineHandling.Entitize;
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(filename, settings);
            //
            ser.Serialize(writer, root);
            writer.Close();
        }
        public static Root Deserialize(string filename)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Root));
            StreamReader reader = new StreamReader(filename);
            Root value = (Root)ser.Deserialize(reader);
            reader.Close();
            return value;
        }
    }
}
