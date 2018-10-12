using DogeBeats.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace Testowy.Model
{
    //struct candidate
    public class AnimationRouteFrame : IGraphicElement
    {
        public Placement Placement { get; set; }
        public EasingMode Ease { get; set; }
        public TimeSpan TimeLength { get; set; }
        public string GraphicName { get; set; }
    }
}
