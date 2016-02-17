using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace WebUtility
{
    public class XmlSerialize
    {
        public static String Serialize(Object response, Type type)
        {
            var mySerializer = new XmlSerializer(type);
            var myStream = new MemoryStream();
            var xmlns = new XmlSerializerNamespaces();
            xmlns.Add(String.Empty, String.Empty);
            mySerializer.Serialize(myStream, response, xmlns);
            string xml = Encoding.UTF8.GetString(myStream.GetBuffer());
            myStream.Close();
            myStream.Dispose();
            return xml.TrimEnd('\0');
        }

        public static Object Deserialize(string xmlDoc, Type type)
        {
            Object ob = null;
            XmlSerializer xml = null;
            try
            {
                xml = new XmlSerializer(type);
                var myFileStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlDoc));
                ob = xml.Deserialize(myFileStream);
            }
            catch
            {
            }
            return ob;
        }
    }
}