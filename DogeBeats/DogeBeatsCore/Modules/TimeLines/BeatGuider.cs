using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogeBeats.Modules.TimeLines
{
    public class BeatGuider
    {
        public List<TimedGraphicElement> Beats { get; set; } = new List<TimedGraphicElement>();

        public void RegisterBeat(TimeSpan span)
        {
            Beats.Add(new TimedGraphicElement() { Timestamp = span, Object = new Beat()});
        }
    }
}
