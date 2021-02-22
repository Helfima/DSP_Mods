using DSP_Helmod.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace DSP_Test
{
    class TestSerializationJson
    {

        public static void Execute()
        {
            // This is the name of the file holding the data. You can use any file extension you like.
            string fileName = "model.data";

            SerializeItem(fileName);
            DeserializeItem(fileName);

            Console.WriteLine("Done");
            Console.ReadLine();
        }

        public static void SerializeItem(string fileName)
        {
            // Create an instance of the type and serialize it.
            DataModel dataModel = new DataModel();

            FileStream writer = new FileStream(fileName, FileMode.Create);
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(DataModel));
            serializer.WriteObject(writer, dataModel);
            writer.Close();
        }

        public static void DeserializeItem(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open);
            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(DataModel));
            DataModel dataModel = (DataModel)deserializer.ReadObject(fs);
            Console.WriteLine(dataModel.Version);
        }
    }
}
