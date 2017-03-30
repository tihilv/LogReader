namespace LogReader
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class BaseRuleSerializer<T>
    {
        public string Serialize(IEnumerable<T> rules)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T[]));
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.Serialize(ms, rules.ToArray());
                ms.Position = 0;

                using (StreamReader reader = new StreamReader(ms, Encoding.UTF8))
                    return reader.ReadToEnd();
            }
        }

        public IEnumerable<T> Deserialize(string serialized)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T[]));
            using (MemoryStream ms = new MemoryStream())
            using (StreamWriter writer = new StreamWriter(ms, Encoding.UTF8))
            {
                writer.Write(serialized);
                writer.Flush();
                ms.Position = 0;
                return (T[])serializer.Deserialize(ms);
            }
        }
    }
}