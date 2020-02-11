using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace LibCronusMAX
{
    public static class HelperFunctions
    {
        private static readonly double Frequency = 1.0 / (double)Stopwatch.Frequency;

        /// <summary>
        ///     Gets a timestamp
        /// </summary>
        public static double GetTimeStamp()
        {
            return (double)Stopwatch.GetTimestamp() * Frequency;
        }

        internal static string ByteArrayToString(ICollection<byte> ba)
        {
            StringBuilder hex = new StringBuilder(ba.Count * 2);
            foreach (byte b in ba)
            {
                hex.Append($"{b:X2}");
            }
            return hex.ToString();
        }
    }
}
