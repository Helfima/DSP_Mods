using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DSP_Helmod.Classes;
using DSP_Helmod.Model;
using UnityEngine;

namespace DSP_Helmod.Converter
{
    public class DataModelConverter
    {
        public static void WriteXml(string path, DataModel dataModel)
        {
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            if (Directory.Exists(directory))
            {
                SerializeItem(path, dataModel);
            }
            else
            {
                Debug.LogError($"Unable to write file: {path}");
            }
        }
        public static DataModel ReadXml(string path)
        {
            return DeserializeItem(path);
        }

        internal static void SerializeItem(string fileName, DataModel dataModel)
        {
            // Create an instance of the type and serialize it.
            XmlModel xmlModel = XmlModel.Parse(dataModel);
            FileStream writer = new FileStream(fileName, FileMode.Create);
            XmlSerializer serializer = new XmlSerializer(typeof(XmlModel));
            serializer.Serialize(writer, xmlModel);
            writer.Close();
        }

        internal static DataModel DeserializeItem(string fileName)
        {
            if (File.Exists(fileName))
            {
                FileStream reader = new FileStream(fileName, FileMode.Open);
                XmlSerializer deserializer = new XmlSerializer(typeof(XmlModel));
                XmlModel xmlModel = (XmlModel)deserializer.Deserialize(reader);
                reader.Close();
                HMLogger.Debug($"xmlModel.Version:{xmlModel.Version}");
                return xmlModel.GetObject();
            }
            return null;
        }
    }
}
