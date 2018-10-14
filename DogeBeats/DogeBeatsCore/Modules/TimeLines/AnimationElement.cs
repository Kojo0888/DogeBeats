using DogeBeats.Modules.TimeLines;
using DogeBeats.Other;
using DogeBeats.Renderer;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model.GraphicElements;
using Testowy.Model.Shapes;

namespace Testowy.Model
{
    public class AnimationElement : ITLEPanelElement
    {
        public GraphicElementBase Shape { get; set; }

        public AnimationRoute Route { get; set; }

        public Placement InitPlacement { get; set; }

        public bool Prediction { get; set; }

        public AnimationElement()
        {
            GraphicName = GraphicProxy.GenerateElementName(this);
        }

        internal void Update(TimeSpan currentStopperTimeRaw, Placement groupPlacement)
        {
            TimeSpan currentStopperTime = VerifyStopperTime(currentStopperTimeRaw);

            var frameCarrier = Route.GetFrameSlider(currentStopperTime);
            //Route.UpdatePlacemenet(Shape.Placement, currentStopperTime);
            Placement parsedPlacement = Route.CalculatePlacement(currentStopperTime, Shape.Placement);
            Shape.Placement.X = InitPlacement.X + parsedPlacement.X + groupPlacement.X;
            Shape.Placement.Y = InitPlacement.Y + parsedPlacement.Y + groupPlacement.Y;
            Shape.Placement.Width = InitPlacement.Width + parsedPlacement.Width + groupPlacement.Width;
            Shape.Placement.Height = InitPlacement.Height + parsedPlacement.Height + groupPlacement.Height;
            Shape.Placement.Rotation = InitPlacement.Rotation + parsedPlacement.Rotation + groupPlacement.Rotation;
        }

        private TimeSpan VerifyStopperTime(TimeSpan currentStopperTime)
        {
            var ticks = currentStopperTime.Ticks % Route.AnimationTime.Ticks;
            return new TimeSpan(ticks);
        }

        internal void Render()
        {
            //TO DO: Attaching Graphic Engine
            //throw new NotImplementedException();
            GraphicProxy.TranslateObject(Shape.Placement, GraphicName);
        }

        public static AnimationElement Create(NameValueCollection values)
        {
            AnimationElement element = new AnimationElement();

            if (!string.IsNullOrEmpty(values.Get("ElementName")))
                element.GraphicName = values["ElementName"];
            if (!string.IsNullOrEmpty(values.Get("ElementName")))
            {
                switch (values["ElementName"])
                {
                    case "SimpleCircle":
                        element.Shape = new SimpleCircle();
                        break;
                }
            }

            return element;
        }
    }
}
