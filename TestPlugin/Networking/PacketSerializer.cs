using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Stuff
{
    public static class PacketSerializer
    {
        internal static byte[] Serialize(Dictionary<string, byte[]> objects)
        {
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms, Encoding.UTF8))
            {
                bw.Write(objects.Count);
                foreach (var kv in objects)
                {
                    byte[] keyBytes = Encoding.UTF8.GetBytes(kv.Key);
                    bw.Write(keyBytes.Length);
                    bw.Write(keyBytes);
                    bw.Write(kv.Value.Length);
                    bw.Write(kv.Value);
                }
                return ms.ToArray();
            }
        }

        internal static Dictionary<string, byte[]> Deserialize(byte[] data)
        {
            var result = new Dictionary<string, byte[]>();
            using (var ms = new MemoryStream(data))
            using (var br = new BinaryReader(ms, Encoding.UTF8))
            {
                int count = br.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    int keyLen = br.ReadInt32();
                    string key = Encoding.UTF8.GetString(br.ReadBytes(keyLen));
                    int valLen = br.ReadInt32();
                    byte[] val = br.ReadBytes(valLen);
                    result[key] = val;
                }
            }
            return result;
        }
    }
}