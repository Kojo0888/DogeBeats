using DogeBeats.EngineSections.TimeLineEditing;
using DogeBeats.Model;
using DogeBeats.Modules.Centers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogeBeats.EngineSections.Resources
{
    public static class StaticHub
    {
        public static TrigonometricCache TrigonometricCache { get; set; } = new TrigonometricCache("sin", 0.0001);

        public static ResourceManager ResourceManager { get; set; } = new ResourceManager();

        public static ManualUpdater ManualUpdater { get; set; } = new ManualUpdater();
    }
}
