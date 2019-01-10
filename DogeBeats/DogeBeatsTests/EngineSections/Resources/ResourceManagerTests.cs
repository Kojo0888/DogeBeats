using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogeBeats.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;
using DogeBeatsTests;

namespace DogeBeats.Model.Tests
{
    [TestClass()]
    public class ResourceManagerTests
    {
        ResourceManager manager = new ResourceManager();

        [TestInitialize()]
        public void Init()
        {
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
        public void GetResourceTest(string type, string name)
        {
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
            manager.SetSerializedObject(aElem, "Shapes", "TestSerialization");
        }

        [TestMethod()]
        public void GetSerializedObjectTest()
        {
            var obj = manager.GetSerializedObject<AnimationElement>("Shapes", "TestSerialization");
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

            manager.SetAllOfSerializedObjects<AnimationElement>("Shapes", dic);
        }

        [TestMethod()]
        public void GetAllOfSerializedObjectsTest()
        {
            var aElem = MockObjects.GetAnimationElement();
            var aElem2 = MockObjects.GetAnimationElement2();

            var dic = new Dictionary<string, AnimationElement>();
            dic.Add("test AnimationELement1", aElem);
            dic.Add("test AnimationELement2", aElem2);

            manager.SetAllOfSerializedObjects<AnimationElement>("Shapes", dic);

            var dic2 = manager.GetAllOfSerializedObjects<AnimationElement>("Shapes");
            if (dic2 == null || dic2.Count == 0)
                Assert.Fail();
        }

    }
}