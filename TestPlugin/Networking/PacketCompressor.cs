using System.IO;
using System.IO.Compression;

namespace Stuff
{
    public static class PacketCompressor
    {
        private const int MinSizeToCompress = 256;

        public static byte[] Compress(byte[] data, out bool compressed)
        {
            if (data.Length < MinSizeToCompress)
            {
                compressed = false;
                return data;
            }

            using (var output = new MemoryStream())
            {
                using (var deflate = new DeflateStream(output, CompressionLevel.Fastest, true))
                    deflate.Write(data, 0, data.Length);

                byte[] result = output.ToArray();
                if (result.Length >= data.Length)
                {
                    compressed = false;
                    return data;
                }

                compressed = true;
                return result;
            }
        }

        public static byte[] Decompress(byte[] data, int originalSize)
        {
            byte[] output = new byte[originalSize];
            using (var input = new MemoryStream(data))
            using (var deflate = new DeflateStream(input, CompressionMode.Decompress))
            {
                int read = 0;
                while (read < originalSize)
                {
                    int n = deflate.Read(output, read, originalSize - read);
                    if (n == 0) break;
                    read += n;
                }
            }
            return output;
        }
    }
}