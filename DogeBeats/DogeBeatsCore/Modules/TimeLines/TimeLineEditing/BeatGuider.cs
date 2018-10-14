using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogeBeats.Modules.TimeLines
{
    public class BeatGuider
    {
        public List<TimedTLEPanelElement> Beats { get; set; } = new List<TimedTLEPanelElement>();

        public void RegisterBeat(TimeSpan span)
        {
            Beats.Add(new TimedTLEPanelElement() { Timestamp = span, Object = new Beat()});
        }
    }
}
