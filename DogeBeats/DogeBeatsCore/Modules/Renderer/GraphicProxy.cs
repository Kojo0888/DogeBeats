using DogeBeats.MockUps;
using DogeBeats.Model;
using DogeBeats.Modules.Renderer.RendererModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;

namespace DogeBeats.Renderer
{
    public static class GraphicProxy
    {
        public static GraphicProxyTimeLineEditor TimeLineEditor { get; set; } = new GraphicProxyTimeLineEditor();

        public static int GraphicElementNameIndex { get; set; }

        public static Dictionary<string, GameObject> ShapesOnScreen = new Dictionary<string, GameObject>();

        public static void TranslateObject(Placement placement, string elementName)
        {
            //var shape = ShapesOnScreen.FirstOrDefault(f => f.Name == elementName);
            if (ShapesOnScreen.ContainsKey(elementName))
            {
                //attach to the gameObejct of Unity
            }
        }

        internal static string GenerateElementName(AnimationElement animationElement)
        {
            string name = animationElement.Shape.GetType().Name.ToString() + "_" + (GraphicElementNameIndex++);
            if (animationElement.Prediction)
                return "Prediction_EAE_" + name;
            else
                return "EAE_" + name;
        }

        internal static string GenerateElementName(AnimationGroupElement animationElement)
        {
            string name = animationElement.GetType().Name.ToString() + "_" + (GraphicElementNameIndex++);
            return "GE_" + name;
        }

        internal static string GenerateElementName()
        {
            return "Object_" + (GraphicElementNameIndex++);
        }

        public static string CreateGraphicElement(AnimationElement element)
        {
            string shapeName = element.GetType().Name;
            byte[] resourceBytes = CenterResource.GetResource(shapeName);
            string gameObjectName = GenerateElementName(element);

            GameObject gameObject = PrepareGameObject(gameObjectName);
            ShapesOnScreen.Add(gameObjectName, gameObject);

            return gameObjectName;
        }

        public static GameObject PrepareGameObject(string gameObjectName)
        {
            return new GameObject();
            //add this to the parent object etc
        }

        internal static void UpdatePlacement(string graphicName, Placement placement)
        {
            if (ShapesOnScreen.ContainsKey(graphicName))
            {
                //Update placement
            }
        }
    }
}
