using DogeBeats.EngineSections.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogeBeats.Modules.Music
{
    public class SoundItem : INamedElement, IByteParsable
    {
        public TimeSpan TimeOffset { get; set; }//for now not needed

        public object SoundObject { get; set; }//choose music engine and set object

        public string Name { get; set; }

        public byte[] Bytes { get; set; }

        public byte[] GetBytes()
        {
            return Bytes;
        }

        public void LoadBytes(byte[] bytes)
        {
            Bytes = bytes;
        }

        public void Play()
        {
            
        }
    }
}
