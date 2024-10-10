using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace TamsApi.Core
{
    public static class CloneExtensions
    {
        public static T DeepClone<T>(this T a)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                DataContractSerializer dcs = new DataContractSerializer(typeof(T));
                dcs.WriteObject(stream, a);
                stream.Position = 0;
                return (T)dcs.ReadObject(stream);
            }
        }
        public static T Clone<T>(this T obj)
        {
            byte[] buffer = BinarySerialize(obj);
            return (T)BinaryDeserialize(buffer);
        }

        public static byte[] BinarySerialize(object obj)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                return stream.ToArray();
            }
        }

        public static object BinaryDeserialize(byte[] buffer)
        {
            using (var stream = new MemoryStream(buffer))
            {
                var formatter = new BinaryFormatter();
                return formatter.Deserialize(stream);

            }
        }
    }
}
