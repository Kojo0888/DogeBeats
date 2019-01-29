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
using Xunit;

namespace DogeBeats.Model.Tests
{
    [Collection("Synchronical")]
    public class ResourceManagerTests
    {
        ResourceManager manager = new ResourceManager();

        public ResourceManagerTests()
        {
            FileTestHelper.Init();
            manager.LoadAllResources();
        }

        [Fact]
        public void LoadAllResourcesTest()
        {

        }

        [Theory]
        [InlineData("Images", "test")]
        [InlineData("Images", "test.png")]
        [InlineData("images", "test")]
        [InlineData("images", "test.png")]
        [InlineData("images", "tEst.png")]
        [InlineData("images", "Test.png")]
        public void GetResourceTest(string type, string name)
        {
            FileTestHelper.CreateDummyFile(type, name);
            manager.LoadAllResources();

            byte[] bytes = manager.GetResource(type, name);
            if (bytes == null || bytes.Length == 0)
                throw new Exception("Assert Fails");
        }

        [Fact]
        public void SetSerializedObjectTest()
        {
            var values = new System.Collections.Specialized.NameValueCollection();
            values.Add("Prediction", "False");
            values.Add("ShapeName", "IdkYet");
            var aElem = new AnimationSingleElement();
            aElem.UpdateManual(values);
            manager.SaveSerializedObject(aElem, "TestSerialization", "TestSerialization");
        }

        [Fact]
        public void GetSerializedObjectTest()
        {
            var obj = manager.GetSerializedObject<AnimationSingleElement>("TestSerialization", "TestSerialization");
            if (obj == null)
                throw new Exception("Assert Fails");
        }

        [Fact]
        public void SetAllOfSerializedObjectsTest()
        {
            var aElem = MockObjects.GetAnimationElement();
            var aElem2 = MockObjects.GetAnimationElement2();

            var dic = new Dictionary<string, AnimationSingleElement>();
            dic.Add("test AnimationELement1", aElem);
            dic.Add("test AnimationELement2", aElem2);

            manager.SaveAllOfSerializedObjects<AnimationSingleElement>("TestSerialization", dic);
        }

        [Fact]
        public void GetAllOfSerializedObjectsTest()
        {
            var aElem = MockObjects.GetAnimationElement();
            var aElem2 = MockObjects.GetAnimationElement2();

            var dic = new Dictionary<string, AnimationSingleElement>();
            dic.Add("test AnimationELement1", aElem);
            dic.Add("test AnimationELement2", aElem2);

            manager.SaveAllOfSerializedObjects<AnimationSingleElement>("TestSerialization", dic);

            var dic2 = manager.GetAllOfSerializedObjects<AnimationSingleElement>("TestSerialization");
            if (dic2 == null || dic2.Count == 0)
                throw new Exception("Assert Fails");
        }

        [Fact]
        public void GetResourceNameWithExtensionTest()
        {
            var aElem = MockObjects.GetAnimationElement();
            manager.SaveSerializedObject<AnimationSingleElement>(aElem, "TestSerialization", aElem.Name);

            var filenameWithExtension = manager.GetResourceNameWithExtension("TestSerialization", aElem.Name);
            if (filenameWithExtension != aElem.Name + ".json")
                throw new Exception("Assert Fails");
        }

        [Theory]
        [InlineData("Nested1\\Nested2", "nesttest1")]
        [InlineData("Nested1\\Nested2\\Nested3", "nestTest2")]
        [InlineData("Nested1\\Nested2\\Nested3", "nestTest2.txt")]
        public void NestedFolderResourcesSupport(string type, string name)
        {
            FileTestHelper.CreateDummyFile(type, name);
            manager.LoadAllResources();

            byte[] bytes = manager.GetResource(type, name);
            if (bytes == null || bytes.Length == 0)
                throw new Exception("Assert Fails");
        }

        [Fact]
        public void GetAllResourcesTest()
        {
            FileTestHelper.CreateDummyFile("Test\\Test232", "Test.txt");
            manager.LoadAllResources();
            var results = manager.GetAllResources("Test\\Test232");
            if (results.Count != 1)
                throw new Exception("Assert Fails");
        }

        [Theory]
        [InlineData("Test\\Test2322", "Test331.txt")]
        public void SaveAllTest(string type, string name)
        {
            FileTestHelper.CreateDummyFile(type, name);
            manager.LoadAllResources();
            var resource = manager.GetAllResources(type).FirstOrDefault();

            byte[] bytes = new byte[] { 1, 1, 1, 1, 1, 1 };

            //resource.Value = bytes;
            manager.AddOrUpdateResource(type, name, bytes);

            manager.SaveAll();
            manager.LoadAllResources();

            var resource2 = manager.GetResource(type, name);
            Assert.Equal(bytes, resource2);
        }
    }
}