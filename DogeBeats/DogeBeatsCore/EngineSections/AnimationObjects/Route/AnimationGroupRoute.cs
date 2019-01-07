using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;

namespace DogeBeats.Model.Route
{
    public class AnimationGroupRoute : AnimationRoute
    {
        public TimeSpan AnimationStartTime { get; set; }

        public TimeSpan AnimationEndTime
        {
            get
            {
                return AnimationStartTime.Add(AnimationTime);
            }
        }
    }
}
