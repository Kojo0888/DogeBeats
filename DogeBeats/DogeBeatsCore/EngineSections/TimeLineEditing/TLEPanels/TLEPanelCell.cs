using DogeBeats.EngineSections.AnimationObjects;
using DogeBeats.EngineSections.Shared;
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
        public ITLEPanelCellElement ReferenceElement { get; set; }

        public TimeSpan DurationTime
        {
            get
            {
                return ReferenceElement.GetDurationTime();
            }
        }

        public Placement Placement { get; set; }
        public string GraphicName { get; set; }

        //public bool Selected { get; set; }

        public static TLEPanelCell Parse(ITLEPanelCellElement element)
        {
            TLEPanelCell cell = new TLEPanelCell();
            cell.GraphicName = GraphicProxy.GenerateGraphicName();
            cell.ReferenceElement = element;
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

        public static List<TLEPanelCell> Parse<T>(List<T> elements)
        {
            if (typeof(T) == typeof(AnimationRouteFrame))
                return Parse(elements as List<AnimationRouteFrame>);
            else if (typeof(T) == typeof(Beat))
                return Parse(elements as List<Beat>);
            else if (typeof(T) == typeof(ITLEPanelCellElement))
                return Parse(elements as List<ITLEPanelCellElement>);
            else if (typeof(T) == typeof(IAnimationElement))
                return Parse(elements as List<IAnimationElement>);
            else
                throw new NesuException("TLEPanelCell: Parse generic unable to recognise generic type: " + typeof(T));
        }
    }
}
