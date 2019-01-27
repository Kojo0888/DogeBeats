using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogeBeats.EngineSections.Shared
{
    public static class ManualUpdaterParser
    {
        public static string ParseString(string value)
        {
            return value;
        }

        public static string Parse(string value, string original)
        {
            if (!string.IsNullOrEmpty(value))
                return ParseString(value);
            else
                return original;
            
        }

        public static int ParseInt(string value)
        {
            int temp = 0;
            if (int.TryParse(value, out temp))
            {
                return temp;
            }
            else
                return -1;
        }

        public static int Parse(string value, int original)
        {
            if (!string.IsNullOrEmpty(value))
                return ParseInt(value);
            else
                return original;
        }

        public static float ParseFloat(string value)
        {
            float temp = 0;
            if (float.TryParse(value, out temp))
            {
                return temp;
            }
            else
                return -1;
        }

        public static float Parse(string value, float original)
        {
            if (!string.IsNullOrEmpty(value))
                return ParseFloat(value);
            else
                return original;
        }

        public static double ParseDouble(string value)
        {
            double temp = 0;
            if (double.TryParse(value, out temp))
            {
                return temp;
            }
            else
                return -1;
        }

        public static double Parse(string value, double original)
        {
            if (!string.IsNullOrEmpty(value))
                return ParseDouble(value);
            else
                return original;
        }

        public static decimal ParseDecimal(string value)
        {
            decimal temp = 0;
            if (decimal.TryParse(value, out temp))
            {
                return temp;
            }
            else
                return -1;
        }

        public static decimal Parse(string value, decimal original)
        {
            if (!string.IsNullOrEmpty(value))
                return ParseDecimal(value);
            else
                return original;
        }

        public static TimeSpan ParseTimeSpan(string value)
        {
            TimeSpan temp = new TimeSpan();
            if (TimeSpan.TryParse(value, out temp))
            {
                return temp;
            }
            else
                return new TimeSpan();
        }

        public static TimeSpan Parse(string value, TimeSpan original)
        {
            if (!string.IsNullOrEmpty(value))
                return ParseTimeSpan(value);
            else
                return original;
        }

        public static bool ParseBoolean(string value)
        {
            bool temp = false;
            if (bool.TryParse(value, out temp))
            {
                return temp;
            }
            else
                return false;
        }

        public static bool Parse(string value, bool original)
        {
            if (!string.IsNullOrEmpty(value))
                return ParseBoolean(value);
            else
                return original;
        }
    }
}
