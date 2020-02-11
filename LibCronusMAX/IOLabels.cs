namespace LibCronusMAX
{
    public static class IOLabels
    {
        /// <summary>
        ///     Gets an array of strings containing the label for each input/output field
        /// </summary>
        /// <param name="controller">Controller to get the labels for</param>
        /// <returns>Array of strings containing the labels for each input/output field</returns>
        /// <remarks>There may be null strings in the array when there is nothing defined for that particular index</remarks>
        public static string[] GetLabels(IOStatus.ControllerType controller)
        {
            switch (controller)
            {
                case IOStatus.ControllerType.Wii:
                    return GetWii();
                case IOStatus.ControllerType.WiiN:
                    return GetWiiNc();
                case IOStatus.ControllerType.WiiPro:
                    return GetWiiPro();
                case IOStatus.ControllerType.Xb1:
                    return GetXboxOne();
                case IOStatus.ControllerType.Xb360:
                    return GetXbox360();
                case IOStatus.ControllerType.Ps3:
                    return GetPlaystation3();
                case IOStatus.ControllerType.None:
                case IOStatus.ControllerType.Ps4:
                    return GetPlaystation4();
                case IOStatus.ControllerType.G25:
                    return GetG25();
                case IOStatus.ControllerType.G27:
                    return GetG27();
                default:
                    return new string[30];
            }
        }

        /// <summary>
        ///     Gets an array of strings containing the label for each input/output field
        /// </summary>
        /// <param name="console">Console to get the labels for</param>
        /// <returns>Array of strings containing the labels for each input/output field</returns>
        /// <remarks>There may be null strings in the array when there is nothing defined for that particular index</remarks>
        public static string[] GetLabels(IOStatus.ConsoleType console)
        {
            switch (console)
            {
                case IOStatus.ConsoleType.Ps3:
                case IOStatus.ConsoleType.Ps3Wheel:
                    return GetPlaystation3();
                case IOStatus.ConsoleType.Xb360:
                case IOStatus.ConsoleType.Xb360Wheel:
                    return GetXbox360();
                case IOStatus.ConsoleType.None:
                case IOStatus.ConsoleType.Ps4:
                    return GetPlaystation4();
                case IOStatus.ConsoleType.Xb1:
                case IOStatus.ConsoleType.Xb1Wheel:
                    return GetXboxOne();
                case IOStatus.ConsoleType.Ps4Wheel:
                    return GetG29();
                default:
                    return new string[30];
            }
        }

        private static string[] GetXboxOne()
        {
            return new string[30]
            {
                "XB1_XBOX",
                "XB1_VIEW",
                "XB1_MENU",
                "XB1_RB",
                "XB1_RT",
                "XB1_RS",
                "XB1_LB",
                "XB1_LT",
                "XB1_LS",
                "XB1_RX",
                "XB1_RY",
                "XB1_LX",
                "XB1_LY",
                "XB1_UP",
                "XB1_DOWN",
                "XB1_LEFT",
                "XB1_RIGHT",
                "XB1_Y",
                "XB1_B",
                "XB1_A",
                "XB1_X",
                null,
                null,
                null,
                "XB1_PR1",
                "XB1_PR2",
                "XB1_PL1",
                "XB1_PL2",
                null,
                null
            };
        }

        private static string[] GetXbox360()
        {
            return new string[30]
            {
                "XB360_XBOX",
                "XB360_BACK",
                "XB360_START",
                "XB360_RB",
                "XB360_RT",
                "XB360_RS",
                "XB360_LB",
                "XB360_LT",
                "XB360_LS",
                "XB360_RX",
                "XB360_RY",
                "XB360_LX",
                "XB360_LY",
                "XB360_UP",
                "XB360_DOWN",
                "XB360_LEFT",
                "XB360_RIGHT",
                "XB360_Y",
                "XB360_B",
                "XB360_A",
                "XB360_X",
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
            };
        }

        private static string[] GetPlaystation3()
        {
            return new string[30]
            {
                "PS3_PS",
                "PS3_SELECT",
                "PS3_START",
                "PS3_R1",
                "PS3_R2",
                "PS3_R3",
                "PS3_L1",
                "PS3_L2",
                "PS3_L3",
                "PS3_RX",
                "PS3_RY",
                "PS3_LX",
                "PS3_LY",
                "PS3_UP",
                "PS3_DOWN",
                "PS3_LEFT",
                "PS3_RIGHT",
                "PS3_TRIANGLE",
                "PS3_CIRCLE",
                "PS3_CROSS",
                "PS3_SQUARE",
                "PS3_ACCX",
                "PS3_ACCY",
                "PS3_ACCZ",
                "PS3_GYRO",
                null,
                null,
                null,
                null,
                null
            };
        }

        private static string[] GetPlaystation4()
        {
            return new string[30]
            {
                "PS4_PS",
                "PS4_SHARE",
                "PS4_OPTIONS",
                "PS4_R1",
                "PS4_R2",
                "PS4_R3",
                "PS4_L1",
                "PS4_L2",
                "PS4_L3",
                "PS4_RX",
                "PS4_RY",
                "PS4_LX",
                "PS4_LY",
                "PS4_UP",
                "PS4_DOWN",
                "PS4_LEFT",
                "PS4_RIGHT",
                "PS4_TRIANGLE",
                "PS4_CIRCLE",
                "PS4_CROSS",
                "PS4_SQUARE",
                "PS4_ACCX",
                "PS4_ACCY",
                "PS4_ACCZ",
                "PS4_GYROX",
                "PS4_GYROY",
                "PS4_GYROZ",
                "PS4_TOUCH",
                "PS4_TOUCHX",
                "PS4_TOUCHY"
            };
        }

        private static string[] GetWii()
        {
            return new string[30]
            {
                "WII_HOME",
                "WII_MINUS",
                "WII_PLUS",
                null,
                null,
                "WII_ONE",
                null,
                null,
                "WII_TWO",
                null,
                null,
                null,
                null,
                "WII_UP",
                "WII_DOWN",
                "WII_LEFT",
                "WII_RIGHT",
                null,
                "WII_B",
                "WII_A",
                null,
                "WII_ACCX",
                "WII_ACCY",
                "WII_ACCZ",
                null,
                null,
                null,
                null,
                "WII_IRX",
                "WII_IRY"
            };
        }

        private static string[] GetWiiNc()
        {
            string[] wii = GetWii();
            wii[6] = "WII_C";
            wii[7] = "WII_Z";
            wii[11] = "WII_NX";
            wii[12] = "WII_NY";
            wii[25] = "WII_ACCNX";
            wii[26] = "WII_ACCNY";
            wii[27] = "WII_ACCNZ";
            return wii;
        }

        private static string[] GetWiiPro()
        {
            string[] wii = GetWii();
            wii[3] = "WII_RT";
            wii[4] = "WII_ZR";
            wii[6] = "WII_LT";
            wii[7] = "WII_ZL";
            wii[9] = "WII_RX";
            wii[10] = "WII_RY";
            wii[11] = "WII_LX";
            wii[12] = "WII_LY";
            wii[17] = "WII_X";
            wii[20] = "WII_Y";
            return wii;
        }

        private static string[] GetG25()
        {
            return new string[30]
            {
                "G25_PS",
                "G25_SELECT",
                "G25_START",
                "G25_RPADDLE",
                "G25_R2",
                "G25_R3",
                "G25_LPADDLE",
                "G25_L2",
                "G25_L3",
                "G25_GAS",
                "G25_CLUTCH",
                "G25_STEERING",
                "G25_BRAKE",
                "G25_UP",
                "G25_DOWN",
                "G25_LEFT",
                "G25_RIGHT",
                "G25_TRIANGLE",
                "G25_CIRCLE",
                "G25_CROSS",
                "G25_SQUARE",
                "G25_SHIFT",
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
            };
        }

        private static string[] GetG27()
        {
            return new string[30]
            {
                "G27_PS",
                "G27_SELECT",
                "G27_START",
                "G27_RPADDLE",
                "G27_R2",
                "G27_R3",
                "G27_LPADDLE",
                "G27_L2",
                "G27_L3",
                "G27_GAS",
                "G27_CLUTCH",
                "G27_STEERING",
                "G27_BRAKE",
                "G27_UP",
                "G27_DOWN",
                "G27_LEFT",
                "G27_RIGHT",
                "G27_TRIANGLE",
                "G27_CIRCLE",
                "G27_CROSS",
                "G27_SQUARE",
                "G27_SHIFT",
                "G27_L4",
                "G27_L5",
                "G27_R4",
                "G27_R5",
                null,
                null,
                null,
                null
            };
        }

        private static string[] GetG29()
        {
            return new string[30]
            {
                "G29_PS",
                "G29_SELECT",
                "G29_START",
                "G29_RPADDLE",
                "G29_R2",
                "G29_R3",
                "G29_LPADDLE",
                "G29_L2",
                "G29_L3",
                "G29_GAS",
                "G29_CLUTCH",
                "G29_STEERING",
                "G29_BRAKE",
                "G29_UP",
                "G29_DOWN",
                "G29_LEFT",
                "G29_RIGHT",
                "G29_TRIANGLE",
                "G29_CIRCLE",
                "G29_CROSS",
                "G29_SQUARE",
                "G29_SHIFT",
                "G29_UP_ARROW",
                "G29_DOWN_ARROW",
                "G29_DIAL_CCW",
                "G29_DIAL_CW",
                "G29_DIAL",
                null,
                null,
                null
            };
        }
    }
}
