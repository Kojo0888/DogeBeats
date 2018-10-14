using DogeBeats.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;

namespace DogeBeats.Modules.TimeLines
{
    public class Beat : ITLEPanelElement
    {
        public string GraphicName { get; set; }
        public TimeSpan Timestamp { get; set; }
    }
}
