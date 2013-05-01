/*
====================================================================================
This file is part of ListPlayers, the open-source S.T.A.L.K.E.R. multiplayer
statistics organizing tool for game server administrators.
Copyright (C) 2013 Pavel Kovalenko.

You should have received a copy of the MIT License along with ListPlayers sources.
If not, see <http://www.opensource.org/licenses/mit-license.php>.

For support and more information about ListPlayers,
visit <http://mpnetworks.ru> or <https://github.com/nitrocaster/ListPlayers>
====================================================================================
*/

using System.IO;
using System.Text;

namespace FileSystem
{
    /// <summary>
    ///     Represents a scanner that uses the Boyer-Moore algorithm to find an arbitrary sequence in the Stream.
    /// </summary>
    public static class StreamScanner
    {
        private const int defaultBufferSize = 1024;

        private static int[] BuildShiftArray(byte[] needle)
        {
            var shifts = new int[needle.Length];
            var shiftCount = 0;
            var end = needle[needle.Length - 1];
            var index = needle.Length - 1;
            var shift = 1;
            while (--index >= 0)
            {
                if (needle[index] == end)
                {
                    shifts[shiftCount++] = shift;
                    shift = 1;
                }
                else
                {
                    ++shift;
                }
            }
            var ret = new int[shiftCount];
            for (var i = 0; i < shiftCount; ++i)
            {
                ret[i] = shifts[i];
            }
            return ret;
        }

        private static byte[] FlushBuffer(byte[] buffer, int keepSize)
        {
            var newBuffer = new byte[buffer.Length];
            for (var i = 0; i < keepSize; ++i)
            {
                newBuffer[i] = buffer[buffer.Length - keepSize + i];
            }
            return newBuffer;
        }

        private static int FindBytes(byte[] haystack, int haystackSize, byte[] needle, int[] shiftArray)
        {
            var currentShiftIndex = 0;
            var shiftFlag = false;
            var index = needle.Length;
            while (true)
            {
                var needleIndex = needle.Length - 1;
                while (true)
                {
                    if (index >= haystackSize)
                    {
                        return -1;
                    }
                    if (haystack[index] == needle[needleIndex])
                    {
                        break;
                    }
                    ++index;
                }
                var searchIndex = index;
                needleIndex = needle.Length - 1;
                while (needleIndex >= 0 && haystack[searchIndex] == needle[needleIndex])
                {
                    --searchIndex;
                    --needleIndex;
                }
                if (needleIndex < 0)
                {
                    return index - needle.Length + 1;
                }
                if (shiftFlag)
                {
                    shiftFlag = false;
                    index += shiftArray[0];
                    currentShiftIndex = 1;
                }
                else if (currentShiftIndex > 0 && currentShiftIndex >= shiftArray.Length)
                {
                    shiftFlag = true;
                    ++index;
                }
                else
                {
                    if (shiftArray.Length == 0)
                    {
                        ++index;
                    }
                    else
                    {
                        index += shiftArray[currentShiftIndex++];
                    }
                }
            }
        }

        public static int FindString(Stream stream, Encoding encoding, string needle, int bufferSize = defaultBufferSize)
        {
            var bytes = encoding.GetBytes(needle);
            return FindBytes(stream, bytes, bufferSize);
        }

        public static int FindBytes(Stream stream, byte[] needle, int bufferSize = defaultBufferSize)
        {
            if (needle.Length > bufferSize)
            {
                bufferSize = needle.Length;
            }
            var buffer = new byte[bufferSize];
            var shiftArray = BuildShiftArray(needle);
            var offset = 0;
            var init = needle.Length;
            while (true)
            {
                var bytesRead = stream.Read(buffer, needle.Length - init, buffer.Length - needle.Length + init);
                if (bytesRead == 0)
                {
                    return -1;
                }
                var val = FindBytes(buffer, bytesRead + needle.Length - init, needle, shiftArray);
                if (val != -1)
                {
                    return val + offset;
                }
                buffer = FlushBuffer(buffer, needle.Length);
                offset += bytesRead - init;
                init = 0;
            }
        }
    }
}
