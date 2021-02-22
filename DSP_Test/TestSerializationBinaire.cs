using DSP_Helmod.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DSP_Test
{
    class TestSerializationBinaire
    {

        public static void Execute()
        {
            // This is the name of the file holding the data. You can use any file extension you like.
            string fileName = "model.data";

            // Use a BinaryFormatter or SoapFormatter.
            IFormatter formatter = new BinaryFormatter();
            //IFormatter formatter = new SoapFormatter();

            TestSerializationBinaire.SerializeItem(fileName, formatter); // Serialize an instance of the class.
            TestSerializationBinaire.DeserializeItem(fileName, formatter); // Deserialize the instance.
            Console.WriteLine("Done");
            Console.ReadLine();
        }

        public static void SerializeItem(string fileName, IFormatter formatter)
        {
            // Create an instance of the type and serialize it.
            DataModel dataModel = new DataModel();

            FileStream stream = new FileStream(fileName, FileMode.Create);
            formatter.Serialize(stream, dataModel);
            stream.Close();
        }

        public static void DeserializeItem(string fileName, IFormatter formatter)
        {
            FileStream stream = new FileStream(fileName, FileMode.Open);
            DataModel dataModel = (DataModel)formatter.Deserialize(stream);
            Console.WriteLine(dataModel.Version);
        }
    }
}
