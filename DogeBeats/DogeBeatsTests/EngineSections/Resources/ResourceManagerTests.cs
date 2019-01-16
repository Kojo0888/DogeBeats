using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogeBeats.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;
using DogeBeatsTests;
using System.IO;
using DogeBeatsTests.Data;

namespace DogeBeats.Model.Tests
{
    [TestClass()]
    public class ResourceManagerTests
    {
        ResourceManager manager = new ResourceManager();

        [TestInitialize()]
        public void Init()
        {
            FileTestHelper.Init();
            manager.LoadAllResources();
        }

        [TestMethod()]
        public void LoadAllResourcesTest()
        {
            Init();
        }

        [TestMethod()]
        [DataRow("Images", "test")]
        [DataRow("Images", "test.png")]
        [DataRow("images", "test")]
        [DataRow("images", "test.png")]
        [DataRow("images", "tEst.png")]
        [DataRow("images", "Test.png")]
        public void GetResourceTest(string type, string name)
        {
            FileTestHelper.CreateDummyFile(type, name);
            manager.LoadAllResources();

            byte[] bytes = manager.GetResource(type, name);
            if (bytes == null || bytes.Length == 0)
                Assert.Fail();
        }

        [TestMethod()]
        public void SetSerializedObjectTest()
        {
            var values = new System.Collections.Specialized.NameValueCollection();
            values.Add("Prediction", "False");
            values.Add("ShapeName", "IdkYet");
            var aElem = AnimationElement.Create(values);
            manager.SetSerializedObject(aElem, "TestSerialization", "TestSerialization");
        }

        [TestMethod()]
        public void GetSerializedObjectTest()
        {
            var obj = manager.GetSerializedObject<AnimationElement>("TestSerialization", "TestSerialization");
            if (obj == null)
                Assert.Fail();
        }

        [TestMethod()]
        public void SetAllOfSerializedObjectsTest()
        {
            var aElem = MockObjects.GetAnimationElement();
            var aElem2 = MockObjects.GetAnimationElement2();

            var dic = new Dictionary<string, AnimationElement>();
            dic.Add("test AnimationELement1", aElem);
            dic.Add("test AnimationELement2", aElem2);

            manager.SetAllOfSerializedObjects<AnimationElement>("TestSerialization", dic);
        }

        [TestMethod()]
        public void GetAllOfSerializedObjectsTest()
        {
            var aElem = MockObjects.GetAnimationElement();
            var aElem2 = MockObjects.GetAnimationElement2();

            var dic = new Dictionary<string, AnimationElement>();
            dic.Add("test AnimationELement1", aElem);
            dic.Add("test AnimationELement2", aElem2);

            manager.SetAllOfSerializedObjects<AnimationElement>("TestSerialization", dic);

            var dic2 = manager.GetAllOfSerializedObjects<AnimationElement>("TestSerialization");
            if (dic2 == null || dic2.Count == 0)
                Assert.Fail();
        }

        [TestMethod()]
        public void GetResourceNameWithExtensionTest()
        {
            var aElem = MockObjects.GetAnimationElement();
            manager.SetSerializedObject<AnimationElement>(aElem, "TimeLines", aElem.Name);

            var filenameWithExtension = manager.GetResourceNameWithExtension("TimeLines", aElem.Name);
            if (filenameWithExtension != aElem.Name + ".json")
                Assert.Fail();
        }

        [TestMethod()]
        [DataRow("Nested1\\Nested2", "nesttest1")]
        [DataRow("Nested1\\Nested2\\Nested3", "nestTest2")]
        [DataRow("Nested1\\Nested2\\Nested3", "nestTest2.txt")]
        public void NestedFolderResourcesSupport(string type, string name)
        {
            FileTestHelper.CreateDummyFile(type, name);
            manager.LoadAllResources();

            byte[] bytes = manager.GetResource(type, name);
            if (bytes == null || bytes.Length == 0)
                Assert.Fail();
        }

        [TestMethod()]
        public void GetAllResourcesTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SaveAllTest()
        {
            Assert.Fail();
        }
    }
}