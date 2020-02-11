using System.Collections.Generic;

namespace LibCronusMAX
{
    public class OutputBuffer
    {
        /// <summary>
        ///     Output values in an array
        /// </summary>
        public readonly int[] Outputs = new int[36];

        /// <summary>
        ///     Gets/Sets the value for Output 0 (PS / Xbox)
        /// </summary>
        public int Output00
        {
            get
            {
                return Outputs[0];
            }
            set
            {
                Outputs[0] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Output 1 (Share / Select / View / Back)
        /// </summary>
        public int Output01
        {
            get
            {
                return Outputs[1];
            }
            set
            {
                Outputs[1] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Output 2 (Options / Start / Menu)
        /// </summary>
        public int Output02
        {
            get
            {
                return Outputs[2];
            }
            set
            {
                Outputs[2] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Output 3 (R1 / RB)
        /// </summary>
        public int Output03
        {
            get
            {
                return Outputs[3];
            }
            set
            {
                Outputs[3] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Output 4 (R2 / RT)
        /// </summary>
        public int Output04
        {
            get
            {
                return Outputs[4];
            }
            set
            {
                Outputs[4] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Output 5 (R3 / RS)
        /// </summary>
        public int Output05
        {
            get
            {
                return Outputs[5];
            }
            set
            {
                Outputs[5] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Output 6 (L1 / LB)
        /// </summary>
        public int Output06
        {
            get
            {
                return Outputs[6];
            }
            set
            {
                Outputs[6] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Output 7 (L2 / LT)
        /// </summary>
        public int Output07
        {
            get
            {
                return Outputs[7];
            }
            set
            {
                Outputs[7] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Output 8 (L3 / LS)
        /// </summary>
        public int Output08
        {
            get
            {
                return Outputs[8];
            }
            set
            {
                Outputs[8] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Output 9 (RX)
        /// </summary>
        public int Output09
        {
            get
            {
                return Outputs[9];
            }
            set
            {
                Outputs[9] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Output 10 (RY)
        /// </summary>
        public int Output10
        {
            get
            {
                return Outputs[10];
            }
            set
            {
                Outputs[10] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Output 11 (LX)
        /// </summary>
        public int Output11
        {
            get
            {
                return Outputs[11];
            }
            set
            {
                Outputs[11] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Output 12 (LY)
        /// </summary>
        public int Output12
        {
            get
            {
                return Outputs[12];
            }
            set
            {
                Outputs[12] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Output 13 (Up)
        /// </summary>
        public int Output13
        {
            get
            {
                return Outputs[13];
            }
            set
            {
                Outputs[13] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Output 14 (Down)
        /// </summary>
        public int Output14
        {
            get
            {
                return Outputs[14];
            }
            set
            {
                Outputs[14] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Output 15 (Left)
        /// </summary>
        public int Output15
        {
            get
            {
                return Outputs[15];
            }
            set
            {
                Outputs[15] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Output 16 (Right)
        /// </summary>
        public int Output16
        {
            get
            {
                return Outputs[16];
            }
            set
            {
                Outputs[16] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Output 17 (Triangle / Y)
        /// </summary>
        public int Output17
        {
            get
            {
                return Outputs[17];
            }
            set
            {
                Outputs[17] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Output 18 (Circle / B)
        /// </summary>
        public int Output18
        {
            get
            {
                return Outputs[18];
            }
            set
            {
                Outputs[18] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Output 19 (Cross / A)
        /// </summary>
        public int Output19
        {
            get
            {
                return Outputs[19];
            }
            set
            {
                Outputs[19] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Output 20 (Square / X)
        /// </summary>
        public int Output20
        {
            get
            {
                return Outputs[20];
            }
            set
            {
                Outputs[20] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Output 21 (ACCY)
        /// </summary>
        public int Output21
        {
            get
            {
                return Outputs[21];
            }
            set
            {
                Outputs[21] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Output 22 (ACCX)
        /// </summary>
        public int Output22
        {
            get
            {
                return Outputs[22];
            }
            set
            {
                Outputs[22] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Output 23 (ACCZ)
        /// </summary>
        public int Output23
        {
            get
            {
                return Outputs[23];
            }
            set
            {
                Outputs[23] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Output 24 (GYRO / GYROX)
        /// </summary>
        public int Output24
        {
            get
            {
                return Outputs[24];
            }
            set
            {
                Outputs[24] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Output 25 (GYROY)
        /// </summary>
        public int Output25
        {
            get
            {
                return Outputs[25];
            }
            set
            {
                Outputs[25] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Output 26 (GYROZ)
        /// </summary>
        public int Output26
        {
            get
            {
                return Outputs[26];
            }
            set
            {
                Outputs[26] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Output 27 (TOUCH)
        /// </summary>
        public int Output27
        {
            get
            {
                return Outputs[27];
            }
            set
            {
                Outputs[27] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Output 28 (TOUCHX)
        /// </summary>
        public int Output28
        {
            get
            {
                return Outputs[28];
            }
            set
            {
                Outputs[28] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value for Output 29 (TOUCHY)
        /// </summary>
        public int Output29
        {
            get
            {
                return Outputs[29];
            }
            set
            {
                Outputs[29] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value of TRACE_1
        /// </summary>
        public int Trace1
        {
            get
            {
                return Outputs[30];
            }
            set
            {
                Outputs[30] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value of TRACE_2
        /// </summary>
        public int Trace2
        {
            get
            {
                return Outputs[31];
            }
            set
            {
                Outputs[31] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value of TRACE_3
        /// </summary>
        public int Trace3
        {
            get
            {
                return Outputs[32];
            }
            set
            {
                Outputs[32] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value of TRACE_4
        /// </summary>
        public int Trace4
        {
            get
            {
                return Outputs[33];
            }
            set
            {
                Outputs[33] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value of TRACE_5
        /// </summary>
        public int Trace5
        {
            get
            {
                return Outputs[34];
            }
            set
            {
                Outputs[34] = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the value of TRACE_6
        /// </summary>
        public int Trace6
        {
            get
            {
                return Outputs[35];
            }
            set
            {
                Outputs[35] = value;
            }
        }

        /// <summary>
        /// Default constructor - Makes a output buffer with the default values for each button (0) and traces (0)
        /// </summary>
        public OutputBuffer()
        {
        }

        /// <summary>
        /// Constructor that makes a output buffer with the specified values for each button and trace value
        /// </summary>
        /// <param name="outputs">Output/Trace values</param>
        public OutputBuffer(IList<int> outputs)
        {
            for (int i = 0; i < outputs.Count && i < Outputs.Length; i++)
            {
                Outputs[i] = outputs[i];
            }
        }

        /// <summary>
        /// Constructor that makes a output buffer with the specified values for each button and trace value
        /// </summary>
        /// <param name="output0">Value of Output0 (PS / Xbox)</param>
        /// <param name="param">Value of all other buttons</param>
        /// <remarks>Params is procssed like an array of ints in the order which they're passed</remarks>
        public OutputBuffer(int output0, params int[] param)
        {
            Outputs[0] = output0;
            for (int i = 1; i < param.Length && i < Outputs.Length; i++)
            {
                Outputs[i] = param[i];
            }
        }
    }
}
