using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogeBeats.EngineSections.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;
using DogeBeatsTests.Data;

namespace DogeBeats.EngineSections.Resources.Tests
{
    [TestClass()]
    public class CentreSerializationBaseTests
    {
        CentreSerializationBase<AnimationSingleElement> centreSerializationBase = new CentreSerializationBase<AnimationSingleElement>();

        [TestInitialize]
        public void Init()
        {
            StaticHub.ResourceManager.LoadAllResources();
            centreSerializationBase.ResourceType = "TestCentreSerializationBase";
        }

        [TestMethod()]
        public void LoadAllTest()
        {
            FileTestHelper.CreateDummyFile(centreSerializationBase.ResourceType, "test1.txt");
            centreSerializationBase.LoadAll();
            var results = centreSerializationBase.GetAll();
            if (results.Count == 0)
                Assert.Fail();
        }

        [TestMethod()]
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
                Assert.Fail();
        }

        [TestMethod()]
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
                Assert.Fail();
        }

        [TestMethod()]
        public void GetTest()
        {
            FileTestHelper.CreateDummyFile(centreSerializationBase.ResourceType, "test1.json", "{\"Shape\":{\"TypeName\":\"IdkYet32\",\"Placement\":null,\"GraphicName\":null},\"Route\":null,\"InitPlacement\":null,\"Prediction\":false,\"ElementName\":null}");
            StaticHub.ResourceManager.LoadAllResources();
            centreSerializationBase.LoadAll();
            var results = centreSerializationBase.Get("test1.json");
            if (results == null)
                Assert.Fail();
        }

        [TestMethod()]
        public void GetAllTest()
        {
            FileTestHelper.CreateDummyFile(centreSerializationBase.ResourceType, "test2.json", "{\"Shape\":{\"TypeName\":\"IdkYet32\",\"Placement\":null,\"GraphicName\":null},\"Route\":null,\"InitPlacement\":null,\"Prediction\":false,\"ElementName\":null}");
            StaticHub.ResourceManager.LoadAllResources();
            centreSerializationBase.LoadAll();

            var results = centreSerializationBase.GetAll();
            if (results.Count == 0)
                Assert.Fail();
        }
    }
}