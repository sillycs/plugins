using System;
using System.IO;
using System.IO.Compression;

namespace Stuff
{
	public class Zip
	{
		public static byte[] Decompress(byte[] input)
		{
			using (MemoryStream memoryStream = new MemoryStream(input))
			{
				byte[] lengthBuffer = new byte[4];
				memoryStream.Read(lengthBuffer, 0, 4);
				int length = BitConverter.ToInt32(lengthBuffer, 0);

				using (GZipStream gZipStream =
					new GZipStream(memoryStream, CompressionMode.Decompress))
				{
					byte[] output = new byte[length];
					gZipStream.Read(output, 0, length);
					return output;
				}
			}
		}

		public static byte[] Compress(byte[] input)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				byte[] lengthBytes = BitConverter.GetBytes(input.Length);
				memoryStream.Write(lengthBytes, 0, 4);

				using (GZipStream gZipStream =
					new GZipStream(memoryStream, CompressionMode.Compress))
				{
					gZipStream.Write(input, 0, input.Length);
					gZipStream.Flush();
				}

				return memoryStream.ToArray();
			}
		}
	}
}
