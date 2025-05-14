using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HumbleCpuMonitor.Config
{
    internal static class SerializationExt
    {
        internal static void SerializeToFile(this object obj, string filename)
        {
            string saveData = null;
            XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, obj);
                saveData = textWriter.ToString();
            }
            if (saveData == null) return;

            FileInfo fi = new FileInfo(filename);
            if (!fi.Directory.Exists) fi.Directory.Create();
            fi.Directory.Refresh();
            if (!fi.Directory.Exists) return;

            if (fi.Exists) fi.Delete();
            File.WriteAllText(filename, saveData);
        }

        internal static T DeserializeFromFile<T>(string filename)
        {
            FileInfo file = new FileInfo(filename);
            if (!file.Exists) return default(T);

            string serializedData = File.ReadAllText(filename);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            StringReader textWriter = new StringReader(serializedData);
            T retVal = (T) xmlSerializer.Deserialize(textWriter);
            return retVal;
        }
    }
}
