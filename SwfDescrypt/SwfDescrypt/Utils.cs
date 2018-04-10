using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwfDescrypt
{
    class Utils
    {
        public static void HexDump(byte[] bytes, string typ, int bytesPerLine = 16)
        {
            if (bytes != null) {

                int bytesLength = bytes.Length;

                char[] HexChars = "0123456789ABCDEF".ToCharArray();

                int firstHexColumn =
                      8                   // 8 characters for the address
                    + 3;                  // 3 spaces

                int firstCharColumn = firstHexColumn
                    + bytesPerLine * 3       // - 2 digit for the hexadecimal value and 1 space
                    + (bytesPerLine - 1) / 8 // - 1 extra space every 8 characters from the 9th
                    + 2;                  // 2 spaces 

                int lineLength = firstCharColumn
                    + bytesPerLine           // - characters to show the ascii value
                    + Environment.NewLine.Length; // Carriage return and line feed (should normally be 2)

                char[] line = (new String(' ', lineLength - 2) + Environment.NewLine).ToCharArray();
                int expectedLines = (bytesLength + bytesPerLine - 1) / bytesPerLine;
                StringBuilder result = new StringBuilder(expectedLines * lineLength);

                for (int i = 0; i < bytesLength; i += bytesPerLine)
                {
                    line[0] = HexChars[(i >> 28) & 0xF];
                    line[1] = HexChars[(i >> 24) & 0xF];
                    line[2] = HexChars[(i >> 20) & 0xF];
                    line[3] = HexChars[(i >> 16) & 0xF];
                    line[4] = HexChars[(i >> 12) & 0xF];
                    line[5] = HexChars[(i >> 8) & 0xF];
                    line[6] = HexChars[(i >> 4) & 0xF];
                    line[7] = HexChars[(i >> 0) & 0xF];

                    int hexColumn = firstHexColumn;
                    int charColumn = firstCharColumn;

                    for (int j = 0; j < bytesPerLine; j++)
                    {
                        if (j > 0 && (j & 7) == 0) hexColumn++;
                        if (i + j >= bytesLength)
                        {
                            line[hexColumn] = ' ';
                            line[hexColumn + 1] = ' ';
                            line[charColumn] = ' ';
                        }
                        else
                        {
                            byte b = bytes[i + j];
                            line[hexColumn] = HexChars[(b >> 4) & 0xF];
                            line[hexColumn + 1] = HexChars[b & 0xF];
                            line[charColumn] = (b < 32 ? '·' : (char)b);
                        }
                        hexColumn += 3;
                        charColumn++;
                    }
                    result.Append(line);
                }
                //return result.ToString();
                Console.WriteLine(result.ToString());
            }
        }
        public static byte[] GetBytes(long s)
        {
            byte[] buffer = new byte[8];
            ulong num = (ulong)s;
            for (int i = 1; i <= 8; i++)
            {
                buffer[i - 1] = (byte)(num >> (0x40 - (i * 8)));
            }
            return buffer;
        }

        public static byte[] GetBytes(string s)
        {
            byte[] buffer = new byte[s.Length];
            int num = 0;
            foreach (char ch in s)
            {
                buffer[num++] = (byte)ch;
            }
            return buffer;
        }

        public static byte[] GetBytes(ushort u)
        {
            byte[] buffer = new byte[2];
            for (int i = 1; i <= 2; i++)
            {
                buffer[i - 1] = (byte)(u >> (0x10 - (i * 8)));
            }
            return buffer;
        }

        public static byte[] GetBytes(uint t)
        {
            byte[] buffer = new byte[4];
            for (int i = 1; i <= 4; i++)
            {
                buffer[i - 1] = (byte)(t >> (0x20 - (i * 8)));
            }
            return buffer;
        }
    }
}
