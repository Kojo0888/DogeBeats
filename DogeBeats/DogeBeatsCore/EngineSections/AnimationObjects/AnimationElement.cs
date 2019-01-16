using DogeBeats.EngineSections.Shared;
using DogeBeats.Modules.TimeLines;
using DogeBeats.Modules.TimeLines.Shapes;
using DogeBeats.Other;
using DogeBeats.Renderer;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testowy.Model
{
    public class AnimationElement : ITLEPanelElement, INamedElement
    {
        public TimeLineShape Shape { get; set; }

        public AnimationRoute Route { get; set; }

        public Placement InitPlacement { get; set; }

        public bool Prediction { get; set; }

        public string Name { get; set; }

        public AnimationElement()
        {
            //GraphicName = GraphicProxy.GenerateElementName(this);
        }

        public void Update(TimeSpan currentStopperTimeRaw, Placement groupPlacement)
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

        public void Render()
        {
            //TODO: Attaching Graphic Engine
            //throw new NotImplementedException();
            GraphicProxy.TranslateObject(Shape.Placement);
        }

        public static AnimationElement Create(NameValueCollection values)
        {
            AnimationElement element = new AnimationElement();
            if (!string.IsNullOrEmpty(values.Get("ShapeTypeName")))
                element.Shape = new TimeLineShape(values["ShapeTypeName"]);
            if (!string.IsNullOrEmpty(values.Get("Name")))
                element.Name = values["Name"];

            return element;
        }

        public void UpdateManual(NameValueCollection values)
        {
            if (!string.IsNullOrEmpty(values["Prediction"].ToString()))
                Prediction = values["Prediction"].ToString().ToLower() == "true" ? true : false;
            if (!string.IsNullOrEmpty(values["ShapeTypeName"].ToString()))
                Shape = new TimeLineShape(values["ShapeTypeName"]);
        }

        public static IEnumerable<string> GetKeysManualUpdate()
        {
            List<string> keys = new List<string>();
            keys.Add("Prediction");
            keys.Add("ShapeTypeName");
            return keys;
        }
    }
}
