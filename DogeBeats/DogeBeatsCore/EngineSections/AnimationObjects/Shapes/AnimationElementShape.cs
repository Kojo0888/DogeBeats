using DogeBeats.EngineSections.Resources;
using DogeBeats.EngineSections.Shared;
using DogeBeats.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;

namespace DogeBeats.Modules.TimeLines.Shapes
{
    public class AnimationElementShape
    {
        public string TypeName { get; set; }

        public AnimationElementShape()
        {

        }

        public AnimationElementShape(string name)
        {
            TypeName = name;
        }

        public ImageItem GetResource()
        {
            return StaticHub.ImageCentre.Get(TypeName);
        }
    }
}
