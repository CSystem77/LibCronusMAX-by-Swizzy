using System;
using System.Collections.Generic;
using System.Linq;

namespace LibCronusMAX
{
    /// <summary>
    ///     Class used to send outputs to the CronusMAX Plus device (which then sends it to the connected console)
    /// </summary>
    public class CmCommand : OutputBuffer
    {
        /// <summary>
        ///     Default constructor that makes a default entry with all buttons set to their default value (0)
        /// </summary>
        public CmCommand()
        {
        }

        /// <summary>
        ///     Constructor that makes a CmCommand object with the specified button values
        /// </summary>
        /// <param name="outputs">Button values to use</param>
        public CmCommand(IList<int> outputs)
            : base(outputs)
        {
        }

        /// <summary>
        ///     Constructor that makes a CmCommand object with the specified button values
        /// </summary>
        /// <param name="output0">Value of button 0 (PS/Xbox)</param>
        /// <param name="param">Value of whatever buttons you specify</param>
        /// <remarks>param is treated as if you passed in a array of buttons</remarks>
        public CmCommand(int output0, params int[] param)
            : base(output0, param)
        {
        }

        internal byte[] ToByteArray()
        {
            return Outputs.Select((int t) => (byte)(sbyte)Math.Max(Math.Min(t, 100), -100)).ToArray();
        }
    }
}
