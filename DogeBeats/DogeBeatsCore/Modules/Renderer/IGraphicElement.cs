using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;

namespace DogeBeats.Other
{
    public interface IGraphicElement
    {
        string GraphicName { get; set; }
        Placement Placement { get; set; }
    }
}
