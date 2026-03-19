using System.Collections.Generic;
using System.Text;

namespace Stuff
{
    public class Pack
    {
        private Dictionary<string, byte[]> Objects { get; set; }

        public Pack()
        {
            Objects = new Dictionary<string, byte[]>();
        }

        public void SetString(string key, string value)
        {
            Objects[key] = Encoding.UTF8.GetBytes(value);
        }

        public void SetInt(string key, int value)
        {
            Objects[key] = System.BitConverter.GetBytes(value);
        }

        public void SetLong(string key, long value)
        {
            Objects[key] = System.BitConverter.GetBytes(value);
        }

        public void SetBool(string key, bool value)
        {
            Objects[key] = System.BitConverter.GetBytes(value);
        }

        public void Set(string key, byte[] value)
        {
            Objects[key] = value;
        }

        public byte[] Pacc()
        {
            byte[] serialized = PacketSerializer.Serialize(Objects);
            return PacketFrame.Encode(serialized);
        }
    }
}