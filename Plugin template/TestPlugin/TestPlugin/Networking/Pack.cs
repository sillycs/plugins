using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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

		public byte[] Pacc()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				IFormatter formatter = new BinaryFormatter();
				formatter.Serialize(memoryStream, new PacketStuff { Objects = Objects });
				return Zip.Compress(memoryStream.ToArray());
			}
		}

		public void Set(string key, byte[] value)
		{
			Objects[key] = value;
		}
	}
}
