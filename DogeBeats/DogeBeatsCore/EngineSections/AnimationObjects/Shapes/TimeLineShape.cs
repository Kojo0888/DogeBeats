using DogeBeats.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;

namespace DogeBeats.Modules.TimeLines.Shapes
{
    public class TimeLineShape : IGraphicElement
    {
        public TimeLineShape()
        {

        }

        public TimeLineShape(string name)
        {
            TypeName = name;
        }

        public string TypeName { get; set; }

        public Placement Placement { get; set; }

        public string GraphicName { get; set; }
    }
}
