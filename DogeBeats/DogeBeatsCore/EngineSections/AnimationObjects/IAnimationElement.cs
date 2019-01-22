using DogeBeats.Modules.TimeLines.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;

namespace DogeBeats.EngineSections.AnimationObjects
{
    public interface IAnimationElement
    {
        AnimationRoute Route { get; set; }

        Placement InitPlacement { get; set; }

        bool Prediction { get; set; }

        string Name { get; set; }

        void Update(TimeSpan currentStopperTimeRaw);

        void Render();
    }
}
