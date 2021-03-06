/*

The contents of this file are subject to the Mozilla Public License
Version 1.1 (the "License"); you may not use this file except in
compliance with the License. You may obtain a copy of the License at
http://www.mozilla.org/MPL/

Software distributed under the License is distributed on an "AS IS"
basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
License for the specific language governing rights and limitations
under the License.

The Original Code is OpenFAST.

The Initial Developer of the Original Code is The LaSalle Technology
Group, LLC.  Portions created by Shariq Muhammad
are Copyright (C) Shariq Muhammad. All Rights Reserved.

Contributor(s): Shariq Muhammad <shariq.muhammad@gmail.com>
                Yuri Astrakhan <FirstName><LastName>@gmail.com
*/
using System;
using System.IO;
using System.Text;

namespace OpenFAST
{
    public static class ByteUtil
    {
        public static byte[] ConvertBitStringToFastByteArray(string bitString)
        {
            if (bitString.Length == 0)
                return EmptyByteArray;

            string[] bitStrings = bitString.Split(' ');
            var bytes = new byte[bitStrings.Length];

            for (int i = 0; i < bitStrings.Length; i++)
            {
                bytes[i] = (byte) Convert.ToInt32(bitStrings[i], 2);
            }

            return bytes;
        }

        public static byte[] ConvertHexStringToByteArray(string hexString)
        {
            if (hexString == null)
                return EmptyByteArray;

            hexString = hexString.Replace(" ", "");
            var bytes = new byte[hexString.Length/2];

            for (int i = 0; i < hexString.Length; i += 2)
            {
                bytes[i/2] = (byte) Convert.ToInt32(hexString.Substring(i, (i + 2) - (i)), 16);
            }

            return bytes;
        }

        public static string ConvertByteArrayToBitString(byte[] bytes)
        {
            return ConvertByteArrayToBitString(bytes, bytes.Length);
        }

        public static string ConvertByteArrayToBitString(byte[] bytes, int length)
        {
            if (bytes.Length == 0)
                return "";

            var builder = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                string bits = Convert.ToString(bytes[i] & 0xFF, 2);

                for (int j = 0; j < (8 - bits.Length); j++)
                    builder.Append('0');

                builder.Append(bits).Append(' ');
            }

            if (builder.Length > 0)
                builder.Remove(builder.Length - 1, 1);

            return builder.ToString();
        }

        public static Stream CreateByteStream(string bitString)
        {
            return new MemoryStream(ConvertBitStringToFastByteArray(bitString));
        }

        public static Stream CreateByteStreamFromHexBytes(string hexString)
        {
            return new MemoryStream(ConvertHexStringToByteArray(hexString));
        }

        public static byte[] Combine(byte[] first, byte[] second)
        {
            var result = new byte[first.Length + second.Length];
            Array.Copy(first, 0, result, 0, first.Length);
            Array.Copy(second, 0, result, first.Length, second.Length);
            return result;
        }

        public static bool IsEmpty(byte[] bytes, int count)
        {
            for (int i = 0; i < count; i++)
                if ((bytes[i] & 0x7f) != 0)
                    return false;
            return true;
        }

        public static byte[] Combine(byte[] first, int firstOffset, int firstLength, byte[] second, int secondOffset,
                                     int secondLength)
        {
            int fLen = Math.Min(firstLength, first.Length - firstOffset);
            int sLen = Math.Min(secondLength, second.Length - secondOffset);
            int totalLength = fLen + sLen;
            var result = new byte[totalLength];
            Array.Copy(first, firstOffset, result, 0, fLen);
            Array.Copy(second, secondOffset, result, fLen, sLen);
            return result;
        }

        public static readonly byte[] EmptyByteArray = new byte[0];
    }
}