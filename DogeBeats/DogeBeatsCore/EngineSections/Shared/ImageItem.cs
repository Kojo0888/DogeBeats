using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogeBeats.EngineSections.Shared
{
    public class ImageItem : IByteParsable, INamedElement
    {
        private byte[] bytes = new byte[0];

        public string Name { get; set; }

        public byte[] GetBytes()
        {
            return bytes;
        }

        public void LoadBytes(byte[] bytes)
        {
            this.bytes = bytes;
        }
    }
}
