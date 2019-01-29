using DogeBeats.EngineSections.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;
using DogeBeatsTests.Data;
using Xunit;

namespace DogeBeats.EngineSections.Resources.Tests
{
    [Collection("Synchronical")]
    public class CentreSerializationBaseTests
    {
        CentreSerializationBase<AnimationSingleElement> centreSerializationBase = new CentreSerializationBase<AnimationSingleElement>("TestCentreSerializationBase");

        public CentreSerializationBaseTests()
        {
            StaticHub.ResourceManager.LoadAllResources();
            //centreSerializationBase.ResourceType = "TestCentreSerializationBase";
        }

        [Fact]
        public void LoadAllTest()
        {
            FileTestHelper.CreateDummyFile(centreSerializationBase.ResourceType, "test1.txt");
            centreSerializationBase.LoadAll();
            var results = centreSerializationBase.GetAll();
            if (results.Count == 0)
                throw new Exception("Assert Fails");
        }

        [Fact]
        public void SaveAllTest()
        {
            FileTestHelper.CreateDummyFile(centreSerializationBase.ResourceType, "test4.json", "{\"Shape\":{\"TypeName\":\"IdkYet32\",\"Placement\":null,\"GraphicName\":null},\"Route\":null,\"InitPlacement\":null,\"Prediction\":false,\"ElementName\":null}");
            StaticHub.ResourceManager.LoadAllResources();
            centreSerializationBase.LoadAll();
            var element = centreSerializationBase.Get("test4.json");
            element.Prediction = true;
            element.Name = "test4";
            centreSerializationBase.SaveAll();

            StaticHub.ResourceManager.LoadAllResources();
            centreSerializationBase.LoadAll();
            var element2 = centreSerializationBase.Get("test4.json");
            if (!element2.Prediction)
                throw new Exception("Assert Fails");
        }

        [Fact]
        public void SaveTest()
        {
            FileTestHelper.CreateDummyFile(centreSerializationBase.ResourceType, "test3.json", "{\"Shape\":{\"TypeName\":\"IdkYet32\",\"Placement\":null,\"GraphicName\":null},\"Route\":null,\"InitPlacement\":null,\"Prediction\":false,\"ElementName\":null}");
            StaticHub.ResourceManager.LoadAllResources();
            centreSerializationBase.LoadAll();
            var element = centreSerializationBase.Get("test3.json");
            element.Prediction = true;
            element.Name = "test3";
            centreSerializationBase.Save(element);

            StaticHub.ResourceManager.LoadAllResources();
            centreSerializationBase.LoadAll();
            var element2 = centreSerializationBase.Get("test3.json");
            if (!element2.Prediction)
                throw new Exception("Assert Fails");
        }

        [Fact]
        public void GetTest()
        {
            FileTestHelper.CreateDummyFile(centreSerializationBase.ResourceType, "test1.json", "{\"Shape\":{\"TypeName\":\"IdkYet32\",\"Placement\":null,\"GraphicName\":null},\"Route\":null,\"InitPlacement\":null,\"Prediction\":false,\"ElementName\":null}");
            StaticHub.ResourceManager.LoadAllResources();
            centreSerializationBase.LoadAll();
            var results = centreSerializationBase.Get("test1.json");
            if (results == null)
                throw new Exception("Assert Fails");
        }

        [Fact]
        public void GetAllTest()
        {
            FileTestHelper.CreateDummyFile(centreSerializationBase.ResourceType, "test2.json", "{\"Shape\":{\"TypeName\":\"IdkYet32\",\"Placement\":null,\"GraphicName\":null},\"Route\":null,\"InitPlacement\":null,\"Prediction\":false,\"ElementName\":null}");
            StaticHub.ResourceManager.LoadAllResources();
            centreSerializationBase.LoadAll();

            var results = centreSerializationBase.GetAll();
            if (results.Count == 0)
                throw new Exception("Assert Fails");
        }

        [Fact]
        public void CreateElementTest()
        {
            string name = "TEst636asd3635";
            this.centreSerializationBase.CreateElement(name);
            if (centreSerializationBase.CentreElements.Count() == 0)
                throw new Exception("Assert Fails");
            var element = centreSerializationBase.Get(name);
            if (centreSerializationBase.CentreElements.ElementAt(0).Key != name || element.Name != name)
                throw new Exception("Assert Fails");
        }

        [Fact]
        public void RenameElementTest()
        {
            string name = "TEst6363635";
            this.centreSerializationBase.CreateElement(name);
            if (centreSerializationBase.CentreElements.Count() == 0)
                throw new Exception("Assert Fails");
            var element = centreSerializationBase.Get(name);
            if (centreSerializationBase.CentreElements.ElementAt(0).Key != name || element.Name != name)
                throw new Exception("Assert Fails");

            string newName = "TEST2323";
            centreSerializationBase.RenameElement(element, name, newName);
            element = centreSerializationBase.Get(newName);
            if (centreSerializationBase.CentreElements.ElementAt(0).Key != newName || element.Name != newName)
                throw new Exception("Assert Fails");
        }
    }
}