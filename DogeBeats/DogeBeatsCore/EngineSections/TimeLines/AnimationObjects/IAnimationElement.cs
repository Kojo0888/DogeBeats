using DogeBeats.EngineSections.Shared;
using DogeBeats.Modules.TimeLines;
using DogeBeats.Modules.TimeLines.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;

namespace DogeBeats.EngineSections.AnimationObjects
{
    public interface IAnimationElement : ITLEPanelCellElement/*temporarly*/, INamedElement
    {
        AnimationRoute Route { get; set; }

        bool Prediction { get; set; }

        void Update(TimeSpan currentStopperTimeRaw, Placement parentPalacement);

        void Render();
    }
}
