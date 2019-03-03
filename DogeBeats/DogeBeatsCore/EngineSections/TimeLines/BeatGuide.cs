using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogeBeats.Modules.TimeLines
{
    public class BeatGuide
    {
        public List<Beat> Beats { get; set; } = new List<Beat>();

        public void RegisterBeat(TimeSpan span)
        {
            Beats.Add(new Beat() { Timestamp = span});
            Beats = Beats.OrderBy(o => o.Timestamp).ToList();
        }

        public void RemoveBeat(TimeSpan span)
        {
            var beat = Beats.FirstOrDefault(f => f.Timestamp <= span && (f.Timestamp + Beat.DurationTime) > span);
            RemoveBeat(beat);
        }

        public void RemoveBeat(Beat beat)
        {
            if (beat != null)
            {
                Beats.Remove(beat);
                Beats = Beats.OrderBy(o => o.Timestamp).ToList();
            }
        }

        public List<ITLEPanelCellElement> GetTLECellElements()
        {
            List<ITLEPanelCellElement> toReturn = new List<ITLEPanelCellElement>();
            foreach (var beat in Beats)
            {
                toReturn.Add(beat as ITLEPanelCellElement);
            }
            return toReturn;
        }
    }
}
