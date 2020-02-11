using System.Collections.Generic;

namespace LibCronusMAX
{
    /// <summary>
    /// A Class used to represent controller inputs
    /// </summary>
    public class InputBuffer
    {
        /// <summary>
        ///     Input values in an array
        /// </summary>
        public readonly int[] Inputs = new int[30];

        /// <summary>
        ///     Gets/Sets the value for Input 0 (PS / Xbox)
        /// </summary>
        public int Input00
        {
            get
            {
                return Inputs[0];
            }
            set
            {
                Inputs[0] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Input 1 (Share / Select / View / Back)
        /// </summary>
        public int Input01
        {
            get
            {
                return Inputs[1];
            }
            set
            {
                Inputs[1] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Input 2 (Options / Start / Menu)
        /// </summary>
        public int Input02
        {
            get
            {
                return Inputs[2];
            }
            set
            {
                Inputs[2] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Input 3 (R1 / RB)
        /// </summary>
        public int Input03
        {
            get
            {
                return Inputs[3];
            }
            set
            {
                Inputs[3] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Input 4 (R2 / RT)
        /// </summary>
        public int Input04
        {
            get
            {
                return Inputs[4];
            }
            set
            {
                Inputs[4] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Input 5 (R3 / RS)
        /// </summary>
        public int Input05
        {
            get
            {
                return Inputs[5];
            }
            set
            {
                Inputs[5] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Input 6 (L1 / LB)
        /// </summary>
        public int Input06
        {
            get
            {
                return Inputs[6];
            }
            set
            {
                Inputs[6] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Input 7 (L2 / LT)
        /// </summary>
        public int Input07
        {
            get
            {
                return Inputs[7];
            }
            set
            {
                Inputs[7] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Input 8 (L3 / LS)
        /// </summary>
        public int Input08
        {
            get
            {
                return Inputs[8];
            }
            set
            {
                Inputs[8] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Input 9 (RX)
        /// </summary>
        public int Input09
        {
            get
            {
                return Inputs[9];
            }
            set
            {
                Inputs[9] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Input 10 (RY)
        /// </summary>
        public int Input10
        {
            get
            {
                return Inputs[10];
            }
            set
            {
                Inputs[10] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Input 11 (LX)
        /// </summary>
        public int Input11
        {
            get
            {
                return Inputs[11];
            }
            set
            {
                Inputs[11] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Input 12 (LY)
        /// </summary>
        public int Input12
        {
            get
            {
                return Inputs[12];
            }
            set
            {
                Inputs[12] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Input 13 (Up)
        /// </summary>
        public int Input13
        {
            get
            {
                return Inputs[13];
            }
            set
            {
                Inputs[13] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Input 14 (Down)
        /// </summary>
        public int Input14
        {
            get
            {
                return Inputs[14];
            }
            set
            {
                Inputs[14] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Input 15 (Left)
        /// </summary>
        public int Input15
        {
            get
            {
                return Inputs[15];
            }
            set
            {
                Inputs[15] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Input 16 (Right)
        /// </summary>
        public int Input16
        {
            get
            {
                return Inputs[16];
            }
            set
            {
                Inputs[16] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Input 17 (Triangle / Y)
        /// </summary>
        public int Input17
        {
            get
            {
                return Inputs[17];
            }
            set
            {
                Inputs[17] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Input 18 (Circle / B)
        /// </summary>
        public int Input18
        {
            get
            {
                return Inputs[18];
            }
            set
            {
                Inputs[18] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Input 19 (Cross / A)
        /// </summary>
        public int Input19
        {
            get
            {
                return Inputs[19];
            }
            set
            {
                Inputs[19] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Input 20 (Square / X)
        /// </summary>
        public int Input20
        {
            get
            {
                return Inputs[20];
            }
            set
            {
                Inputs[20] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Input 21 (ACCY)
        /// </summary>
        public int Input21
        {
            get
            {
                return Inputs[21];
            }
            set
            {
                Inputs[21] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Input 22 (ACCX)
        /// </summary>
        public int Input22
        {
            get
            {
                return Inputs[22];
            }
            set
            {
                Inputs[22] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Input 23 (ACCZ)
        /// </summary>
        public int Input23
        {
            get
            {
                return Inputs[23];
            }
            set
            {
                Inputs[23] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Input 24 (GYRO / GYROX)
        /// </summary>
        public int Input24
        {
            get
            {
                return Inputs[24];
            }
            set
            {
                Inputs[24] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Input 25 (GYROY)
        /// </summary>
        public int Input25
        {
            get
            {
                return Inputs[25];
            }
            set
            {
                Inputs[25] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Input 26 (GYROZ)
        /// </summary>
        public int Input26
        {
            get
            {
                return Inputs[26];
            }
            set
            {
                Inputs[26] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Input 27 (TOUCH)
        /// </summary>
        public int Input27
        {
            get
            {
                return Inputs[27];
            }
            set
            {
                Inputs[27] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Input 28 (TOUCHX)
        /// </summary>
        public int Input28
        {
            get
            {
                return Inputs[28];
            }
            set
            {
                Inputs[28] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Input 29 (TOUCHY)
        /// </summary>
        public int Input29
        {
            get
            {
                return Inputs[29];
            }
            set
            {
                Inputs[29] = value;
            }
        }

        /// <summary>
        ///     Default constructor - Makes a input buffer with the default values for each button (0)
        /// </summary>
        public InputBuffer()
        {
        }

        /// <summary>
        ///     Constructor that makes a input buffer with the specified values for each button
        /// </summary>
        /// <param name="inputs">Input values</param>
        public InputBuffer(IList<int> inputs)
        {
            for (int i = 0; i < inputs.Count && i < Inputs.Length; i++)
            {
                Inputs[i] = inputs[i];
            }
        }

        /// <summary>
        ///     Constructor that makes a input buffer with the specified values for each button
        /// </summary>
        /// <param name="input0">Value of Input0 (PS / Xbox)</param>
        /// <param name="param">Value of all other buttons</param>
        /// <remarks>Params is procssed like an array of ints in the order which they're passed</remarks>
        public InputBuffer(int input0, params int[] param)
        {
            Inputs[0] = input0;
            for (int i = 1; i < param.Length && i < Inputs.Length; i++)
            {
                Inputs[i] = param[i];
            }
        }
    }
}
