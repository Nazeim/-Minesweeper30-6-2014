using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TcpCommunications
{
    public class ByteArrayConverter
    {
        private static int PositiveToInt32(byte[] arrayOfBytes)
        {
            int result = 0;
            int weight = 1;

            for (int i = 0; i < arrayOfBytes.Length; i++)
            {
                result += weight * arrayOfBytes[i];
                weight *= 256;
            }

            return result;
        }
        private static byte[] PositiveToArrayOfBytes(int number)
        {
            byte[] result = new byte[4];
            int counter = 0;

            //stop when the whole number has been converted
            while (number > 0)
            {
                result[counter] = Convert.ToByte(number % 256);
                number /= 256;

                counter++;
            }

            return result;
        }
        private static void SecondComplement(byte[] original)
        {
            FirstComplement(original);
            Increment(original);
        }
        private static void FirstComplement(byte[] original)
        {
            for (int i = 0; i < original.Length; i++)
            {
                original[i] = (byte)(~original[i]);
            }
        }
        private static void Increment(byte[] original)
        {
            bool repeatNext = true;
            int index = 0;

            while (repeatNext && index < original.Length)
            {
                checked
                {
                    try
                    {
                        original[index]++;
                        repeatNext = false;
                    }
                    catch
                    {
                        original[index] = (byte)0;
                    }
                    finally
                    {
                        index++;
                    }
                }
            }
        }

        //returns A SIGNED integer
        public static int ToInt32(byte[] arrayOfBytes)
        {
            if (arrayOfBytes.Length > 4)
                throw new ArgumentOutOfRangeException("array of bytes should contain 4 bytes maximumly");

            if (arrayOfBytes.Length < 4)
                return PositiveToInt32(arrayOfBytes);

            bool isNeg = (arrayOfBytes[arrayOfBytes.Length - 1] & 8) == 8;

            if (!isNeg)
                return PositiveToInt32(arrayOfBytes);

            SecondComplement(arrayOfBytes);

            return PositiveToInt32(arrayOfBytes) * -1;
        }

        //converts A SIGNED integer to an array of bytes
        public static byte[] ToArrayOfBytes(int number, int numberOfBytes)
        {
            byte[] resultFull;

            if (number >= 0)
            {
                resultFull = PositiveToArrayOfBytes(number);
            }
            else
            {
                if (number == Int32.MinValue)
                    resultFull = new byte[] { 0x00, 0x00, 0x00, 0x80 };
                else
                    resultFull = PositiveToArrayOfBytes(-1 * number);

                SecondComplement(resultFull);
            }

            if (numberOfBytes >= 4)
            {
                return resultFull;
            }

            byte[] result = new byte[numberOfBytes];
            Array.Copy(resultFull, result, numberOfBytes);

            return result;
        }

    }
}
