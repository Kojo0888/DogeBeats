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
        public AnimationElementShape()
        {

        }

        public AnimationElementShape(string name)
        {
            TypeName = name;
        }

        public string TypeName { get; set; }
    }
}
