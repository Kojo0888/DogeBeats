﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogeBeats.EngineSections.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;
using System.Xml.Linq;
using DogeBeatsTests;

namespace DogeBeats.EngineSections.Resources.Tests
{
    [TestClass()]
    public class FileAssistantTests
    {
        FileAssistant fileAssistant = new FileAssistant();

        [TestMethod()]
        public void GetFilesFromFolderTest()
        {

            var files = fileAssistant.GetFilesFromFolder(@"Data\Resources\Shapes");
            if (files == null || files.Count == 0)
                Assert.Fail();
        }

        [TestMethod()]
        public void GetFullPathForFolderNameTest()
        {
            var path = fileAssistant.GetFullPathForFolderName("Shapes", "Data");
            if (!(!string.IsNullOrEmpty(path) && path.EndsWith("Shapes") && path.Contains("\\")))
                Assert.Fail();
        }

        [TestMethod()]
        public void GetAllResourceFilesFromFolderTest()
        {
            var dic = fileAssistant.GetAllResourceFilesFromFolder(@"Data\Resources");
            if (dic == null || dic.Count == 0)
                Assert.Fail();
        }

        [TestMethod()]
        public void DeserializeXMLTest()
        {
            fileAssistant = new FileAssistant();
            fileAssistant.SerializationType = "XML";

            var aElem = MockObjects.GetAnimationElement();

            byte[] bytes = fileAssistant.Serialize(aElem);
            string xml = new string(Encoding.Unicode.GetChars(bytes));
            if (bytes == null || bytes.Length == 0 && XDocument.Parse(xml) == null)
                Assert.Fail();

            var aElem2 = fileAssistant.Deserialize<AnimationElement>(bytes);
            if (aElem.Shape.TypeName != aElem2.Shape.TypeName)
                Assert.Fail();
        }

        [TestMethod()]
        public void DeserializeJSONTest()
        {
            fileAssistant = new FileAssistant();
            fileAssistant.SerializationType = "JSON";

            var aElem = MockObjects.GetAnimationElement();

            byte[] bytes = fileAssistant.Serialize(aElem);
            string xml = new string(Encoding.Unicode.GetChars(bytes));
            if (bytes == null || bytes.Length == 0 && XDocument.Parse(xml) == null)
                Assert.Fail();

            var aElem2 = fileAssistant.Deserialize<AnimationElement>(bytes);
            if (aElem.Shape.TypeName != aElem2.Shape.TypeName)
                Assert.Fail();
        }

        [TestMethod()]
        public void SerializeJSONTest()
        {
            fileAssistant = new FileAssistant();
            fileAssistant.SerializationType = "JSON";
            var aElem = MockObjects.GetAnimationElement();

            byte[] bytes = fileAssistant.Serialize(aElem);
            string xml = new string(Encoding.Unicode.GetChars(bytes));
            if (bytes == null || bytes.Length == 0 && XDocument.Parse(xml) == null)
                Assert.Fail();
        }

        [TestMethod()]
        public void SerializeXMLTest()
        {
            fileAssistant = new FileAssistant();
            fileAssistant.SerializationType = "XML";
            var aElem = MockObjects.GetAnimationElement();

            byte[] bytes = fileAssistant.Serialize(aElem);
            string xml = new string(Encoding.Unicode.GetChars(bytes));
            if (bytes == null || bytes.Length == 0 && XDocument.Parse(xml) == null)
                Assert.Fail();
        }
    }
}