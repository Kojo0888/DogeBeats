using DogeBeats.EngineSections.AnimationObjects;
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
        public ITLEPanelCellElement AnimationElement { get; set; }

        public TimeSpan DurationTime
        {
            get
            {
                return AnimationElement.GetDurationTime();
            }
        }

        public Placement Placement { get; set; }
        public string GraphicName { get; set; }

        //public bool Selected { get; set; }

        internal static TLEPanelCell Parse(ITLEPanelCellElement element)
        {
            TLEPanelCell cell = new TLEPanelCell();
            cell.GraphicName = GraphicProxy.GenerateGraphicName();
            cell.AnimationElement = element;
            cell.Placement = new Placement();
            return cell;
        }

        public static List<TLEPanelCell> Parse(List<ITLEPanelCellElement> elements)
        {
            List<TLEPanelCell> timedElements = new List<TLEPanelCell>();
            foreach (var element in elements)
            {
                TLEPanelCell cell = Parse(element);
                timedElements.Add(cell);
            }
            return timedElements;
        }

        public static List<TLEPanelCell> Parse(List<IAnimationElement> elements)
        {
            List<TLEPanelCell> timedElements = new List<TLEPanelCell>();
            foreach (var element in elements)
            {
                TLEPanelCell cell = Parse(element);
                timedElements.Add(cell);
            }
            return timedElements;
        }

        public static List<TLEPanelCell> Parse(List<Beat> elements)
        {
            List<TLEPanelCell> timedElements = new List<TLEPanelCell>();
            foreach (var element in elements)
            {
                TLEPanelCell cell = Parse(element);
                timedElements.Add(cell);
            }
            return timedElements;
        }

        public static List<TLEPanelCell> Parse(List<AnimationRouteFrame> elements)
        {
            List<TLEPanelCell> timedElements = new List<TLEPanelCell>();
            foreach (var element in elements)
            {
                TLEPanelCell cell = Parse(element);
                timedElements.Add(cell);
            }
            return timedElements;
        }
    }
}
