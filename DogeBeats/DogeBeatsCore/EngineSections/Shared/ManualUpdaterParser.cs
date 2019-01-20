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

    }
}
