using DogeBeats.EngineSections.AnimationObjects;
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
    public class AnimationSingleElement : ITLEPanelElement, INamedElement, IAnimationElement, IGraphicElement
    {
        public AnimationElementShape Shape { get; set; }

        public AnimationRoute Route { get; set; }

        public Placement Placement { get; set; }

        public Placement InitPlacement { get; set; }

        public bool Prediction { get; set; }

        public string Name { get; set; }

        public string GraphicName { get; set; }

        public AnimationSingleElement()
        {
            //GraphicName = GraphicProxy.GenerateElementName(this);
        }

        public void Update(TimeSpan currentStopperTimeRaw, Placement groupPlacement)
        {
            TimeSpan currentStopperTime = VerifyStopperTime(currentStopperTimeRaw);

            var frameCarrier = Route.GetFrameSlider(currentStopperTime);

            //Route.UpdatePlacemenet(Shape.Placement, currentStopperTime);
            Placement = Route.CalculatePlacement(currentStopperTime, Placement);
            //Placement.X = InitPlacement.X + parsedPlacement.X + groupPlacement.X;
            //Placement.Y = InitPlacement.Y + parsedPlacement.Y + groupPlacement.Y;
            //Placement.Width = InitPlacement.Width + parsedPlacement.Width + groupPlacement.Width;
            //Placement.Height = InitPlacement.Height + parsedPlacement.Height + groupPlacement.Height;
            //Placement.Rotation = InitPlacement.Rotation + parsedPlacement.Rotation + groupPlacement.Rotation;
        }

        private TimeSpan VerifyStopperTime(TimeSpan currentStopperTime)
        {
            var ticks = currentStopperTime.Ticks % Route.CalculateAnimationTime().Ticks;
            return new TimeSpan(ticks);
        }

        public void Render()
        {
            //TODO: Attaching Graphic Engine
            //throw new NotImplementedException();
            GraphicProxy.TranslateObject(Placement);
        }

        public void Update(TimeSpan currentStopperTimeRaw)
        {
            throw new NotImplementedException();
        }

        public TimeSpan GetDurationTime()
        {
            return Route.CalculateAnimationTime();
        }

        public void UpdateManual(NameValueCollection values)
        {
            if (!string.IsNullOrEmpty(values["Prediction"].ToString()))
                Prediction = values["Prediction"].ToString().ToLower() == "true" ? true : false;
            if (!string.IsNullOrEmpty(values["ShapeTypeName"].ToString()))
                Shape = new AnimationElementShape(values["ShapeTypeName"]);
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
