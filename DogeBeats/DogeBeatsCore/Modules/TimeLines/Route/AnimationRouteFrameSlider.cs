using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testowy.Model
{
    public class AnimationRouteFrameSlider
    {
        public AnimationRouteFrame PreviousFrame { get; set; }

        public AnimationRouteFrame CurrentFrame { get; set; }

        public AnimationRouteFrame NextFrame { get; set; }
    }
}
