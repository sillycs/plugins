using System.Collections.Generic;
using System.Text;

namespace Stuff
{
    public class Unpack
    {
        private Dictionary<string, byte[]> Objects { get; set; }

        public void Unpacc(byte[] packedData)
        {
            Objects = PacketSerializer.Deserialize(packedData);
        }

        public bool Has(string key)
        {
            return Objects != null && Objects.ContainsKey(key);
        }

        public byte[] GetRaw(string key)
        {
            if (!Has(key)) return null;
            return Objects[key];
        }

        public string GetAsString(string key)
        {
            if (!Has(key)) return null;
            return Encoding.UTF8.GetString(Objects[key]);
        }

        public bool GetAsBool(string key)
        {
            if (!Has(key)) return false;
            return System.BitConverter.ToBoolean(Objects[key], 0);
        }

        public short GetAsShort(string key)
        {
            if (!Has(key)) return 0;
            return System.BitConverter.ToInt16(Objects[key], 0);
        }

        public int GetAsInteger(string key)
        {
            if (!Has(key)) return 0;
            return System.BitConverter.ToInt32(Objects[key], 0);
        }

        public long GetAsLong(string key)
        {
            if (!Has(key)) return 0;
            return System.BitConverter.ToInt64(Objects[key], 0);
        }

        public double GetAsDouble(string key)
        {
            if (!Has(key)) return 0;
            return System.BitConverter.ToDouble(Objects[key], 0);
        }

        public byte[] GetAsByteArray(string key)
        {
            return GetRaw(key);
        }

        public Dictionary<string, byte[]> GetAll()
        {
            return Objects;
        }
    }
}