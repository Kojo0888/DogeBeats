using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogeBeats.Modules.Music
{
    public class SoundItem
    {
        public TimeSpan TimeOffset { get; set; }//for now not needed

        public string TrackName { get; set; }

        public object SoundObject { get; set; }//choose music engine and set object

        public void Play()
        {

        }
    }
}
