﻿using System.Globalization;

namespace SleepTimer
{
    public static class NumberExtensions
    {
        public const NumberStyles PositiveNumber = NumberStyles.Number ^ NumberStyles.AllowLeadingSign;

        public static bool IsPositiveNumber(this string test)
        {
            return IsNumeric(test, PositiveNumber);
        }

        public static bool IsNumeric(this string test, NumberStyles numberStyles)
        {
            double number;
            var result = double.TryParse(test, numberStyles, CultureInfo.CurrentCulture, out number);
            return result;
        }

        public static bool IsEmpty(this string test)
        {
            return test == string.Empty;
        }
    }
}
