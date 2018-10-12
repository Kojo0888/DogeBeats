using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DogeBeats
{
    public static class Assumptions
    {
        public static double ScreenWidth { get; set; }
        public static double ScreenHeight { get; set; }

        static Assumptions()
        {
            ScreenWidth = SystemParameters.PrimaryScreenWidth;
            ScreenHeight = SystemParameters.PrimaryScreenHeight;
        }
    }
}
