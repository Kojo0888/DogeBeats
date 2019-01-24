using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogeBeats.EngineSections.Resources.Centres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogeBeats.Modules.Music;
using DogeBeatsTests.Data;

namespace DogeBeats.EngineSections.Resources.Centres.Tests
{
    [TestClass()]
    public class CentreFileBaseTests
    {
        CentreFileBase<SoundItem> centreFileBase = new CentreFileBase<SoundItem>("FileCentreTest");

        [TestInitialize]
        public void Init()
        {
            //centreFileBase.ResourceType = "FileCentreTest";
            FileTestHelper.CreateFolder("Data\\Resources\\"+centreFileBase.ResourceType);
            StaticHub.ResourceManager.LoadAllResources();
        }

        [TestMethod()]
        public void LoadAllTest()
        {
            FileTestHelper.CreateDummyFile(centreFileBase.ResourceType, "test1.txt");
            centreFileBase.LoadAll();
            var results = centreFileBase.GetAll();
            if (results.Count == 0)
                Assert.Fail();
        }

        [TestMethod()]
        public void SaveAllTest()
        {
            FileTestHelper.CreateDummyFile(centreFileBase.ResourceType, "test2.txt");
            centreFileBase.LoadAll();
            var results = centreFileBase.GetAll();
            if (results.Count == 0)
                Assert.Fail();

            var bytes = new byte[] { 1, 3, 2, 2, 3, 2 };
            string key = results.Keys.FirstOrDefault();
            results[key].LoadBytes(bytes);

            centreFileBase.SaveAll();
            centreFileBase.LoadAll();

            var results2 = centreFileBase.GetAll();
            if (results2.Count == 0)
                Assert.Fail();

            var bytes2 = results2[key].GetBytes();

            CollectionAssert.AreEqual(bytes2, bytes);
        }

        [TestMethod()]
        public void SaveTest()
        {
            FileTestHelper.CreateDummyFile(centreFileBase.ResourceType, "test3.txt");
            StaticHub.ResourceManager.LoadAllResources();
            centreFileBase.LoadAll();
            var results = centreFileBase.GetAll();
            if (results.Count == 0)
                Assert.Fail();

            var bytes = new byte[] { 1, 3, 2, 2, 3, 2 };
            string key = "test3.txt";
            //results[key].LoadBytes(bytes);

            centreFileBase.Save(key, bytes);
            centreFileBase.LoadAll();

            var results2 = centreFileBase.GetAll();
            if (results2.Count == 0)
                Assert.Fail();

            var bytes2 = results2[key].GetBytes();

            CollectionAssert.AreEqual(bytes2, bytes);
        }

        [TestMethod()]
        public void GetTest()
        {
            FileTestHelper.CreateDummyFile(centreFileBase.ResourceType, "test3.txt");
            centreFileBase.LoadAll();
            var results = centreFileBase.Get("test3.txt");//TO NIE ZADZIALA (samo: test3)
            if (results == null)
                Assert.Fail();
        }

        [TestMethod()]
        public void GetAllTest()
        {
            FileTestHelper.CreateDummyFile(centreFileBase.ResourceType, "test3.txt");
            centreFileBase.LoadAll();
            var results = centreFileBase.GetAll();
            if (results.Count == 0)
                Assert.Fail();
        }
    }
}