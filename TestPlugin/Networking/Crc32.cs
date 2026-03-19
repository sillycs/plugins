using System;

namespace Stuff
{
    public static class Crc32
    {
        private static readonly uint[] Table = BuildTable();

        private static uint[] BuildTable()
        {
            uint[] table = new uint[256];
            for (uint i = 0; i < 256; i++)
            {
                uint crc = i;
                for (int j = 0; j < 8; j++)
                    crc = (crc & 1) == 1 ? (crc >> 1) ^ 0xEDB88320u : crc >> 1;
                table[i] = crc;
            }
            return table;
        }

        public static uint Compute(byte[] data, int offset, int length)
        {
            uint crc = 0xFFFFFFFFu;
            for (int i = offset; i < offset + length; i++)
                crc = (crc >> 8) ^ Table[(crc ^ data[i]) & 0xFF];
            return crc ^ 0xFFFFFFFFu;
        }

        public static uint Compute(byte[] data)
        {
            return Compute(data, 0, data.Length);
        }
    }
}