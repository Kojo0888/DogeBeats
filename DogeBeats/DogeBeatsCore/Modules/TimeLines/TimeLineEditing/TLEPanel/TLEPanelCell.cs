using DogeBeats.Other;
using DogeBeats.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;

namespace DogeBeats.Modules.TimeLines
{
    public class TLEPanelCell : IGraphicElement
    {
        public Placement Placement { get; set; }
        public string GraphicName { get; set; }
        public TimedTLEPanelElement ReferencingTimedElement { get; set; }

        internal static TLEPanelCell Parse(TimedTLEPanelElement element)
        {
            TLEPanelCell cell = new TLEPanelCell();
            cell.GraphicName = GraphicProxy.GenerateElementName();
            cell.ReferencingTimedElement = element;
            cell.Placement = new Placement();
            return cell;
        }
    }
}
