using System;
using System.IO;

namespace Stuff
{
    public static class PacketFrame
    {
        private const uint Magic = 0xDEADBEEF;

        private const byte FlagCompressed = 0x01;

        public static byte[] Encode(byte[] payload)
        {
            bool compressed;
            byte[] data = PacketCompressor.Compress(payload, out compressed);
            int originalSize = payload.Length;

            byte flags = compressed ? FlagCompressed : (byte)0;

            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                bw.Write(Magic);
                bw.Write(data.Length);
                bw.Write(originalSize);
                bw.Write(flags);
                bw.Write(data);
                uint crc = Crc32.Compute(data);
                bw.Write(crc);
                return ms.ToArray();
            }
        }

        public static byte[] Decode(byte[] frame)
        {
            using (var ms = new MemoryStream(frame))
            using (var br = new BinaryReader(ms))
            {
                uint magic = br.ReadUInt32();
                if (magic != Magic)
                    throw new InvalidDataException("Invalid magic");

                int dataLen = br.ReadInt32();
                int originalSize = br.ReadInt32();
                byte flags = br.ReadByte();
                byte[] data = br.ReadBytes(dataLen);
                uint crc = br.ReadUInt32();

                uint computed = Crc32.Compute(data);
                if (computed != crc)
                    throw new InvalidDataException("CRC mismatch");

                bool compressed = (flags & FlagCompressed) != 0;
                return compressed ? PacketCompressor.Decompress(data, originalSize) : data;
            }
        }
    }
}