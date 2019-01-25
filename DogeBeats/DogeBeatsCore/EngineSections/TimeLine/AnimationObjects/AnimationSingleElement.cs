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
    public class AnimationSingleElement : ITLEPanelCellElement, INamedElement, IAnimationElement, IGraphicElement
    {
        public AnimationElementShape Shape { get; set; }

        public AnimationRoute Route { get; set; }

        public Placement Placement { get; set; }

        public bool Prediction { get; set; }

        public string Name { get; set; }

        public string GraphicName { get; set; }

        public AnimationSingleElement()
        {
            //GraphicName = GraphicProxy.GenerateElementName(this);
        }

        public void Update(TimeSpan currentStopperTimeRaw, Placement parentPlacement)
        {
            TimeSpan currentStopperTime = VerifyStopperTime(currentStopperTimeRaw);

            var frameCarrier = Route.GetFrameSlider(currentStopperTime);

            //Route.UpdatePlacemenet(Shape.Placement, currentStopperTime);
            Placement = Route.CalculatePlacement(currentStopperTime);
            Placement.X = Route.StartPlacement.X + parentPlacement.X;
            Placement.Y = Route.StartPlacement.Y + parentPlacement.Y;
            //Placement.Width //No support
            //Placement.Height //No support
            //Placement.Rotation = InitPlacement.Rotation + groupPlacement.Rotation;//TODO
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

        public TimeSpan GetDurationTime()
        {
            return Route.CalculateAnimationTime();
        }

        public TimeSpan GetStartTime()
        {
            return Route.AnimationStartTime;
        }

        public void SetStartTime(TimeSpan timeSpan)
        {
            Route.AnimationStartTime = timeSpan;
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
