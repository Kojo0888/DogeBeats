using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogeBeats.Modules.TimeLines
{
    public class BeatGuider
    {
        public List<Beat> Beats { get; set; } = new List<Beat>();

        public void RegisterBeat(TimeSpan span)
        {
            Beats.Add(new Beat() { Timestamp = span});
        }
    }
}
