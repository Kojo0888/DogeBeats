using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogeBeats.EngineSections.Shared
{
    public interface IByteParsable
    {
        void LoadBytes(byte[] bytes);

        byte[] GetBytes();
    }
}
