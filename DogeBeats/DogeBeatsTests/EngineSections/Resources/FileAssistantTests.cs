using DogeBeats.EngineSections.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;
using System.Xml.Linq;
using DogeBeatsTests;
using DogeBeatsTests.Data;
using System.IO;
using Xunit;

namespace DogeBeats.EngineSections.Resources.Tests
{
    [Collection("Synchronical")]
    public class FileAssistantTests
    {
        FileAssistant fileAssistant = new FileAssistant();

        public FileAssistantTests()
        {
            FileTestHelper.Init();
        }

        [Fact]
        public void GetFilesFromFolderTest()
        {
            FileTestHelper.CreateDummyFile("Shapes", "whatever");

            var files = fileAssistant.GetFilesFromFolder(@"Data\Resources\Shapes");
            if (files == null || files.Count == 0)
                throw new Exception("Assert Fails");
        }

        [Fact]
        public void GetFullPathForFolderNameTest()
        {
            var path = fileAssistant.GetFullPathForFolderName("Shapes", "Data");
            if (!(!string.IsNullOrEmpty(path) && path.EndsWith("Shapes") && path.Contains("\\")))
                throw new Exception("Assert Fails");
        }

        [Fact]
        public void GetAllResourceFilesFromFolderTest()
        {
            var dic = fileAssistant.GetAllResourceFilesFromFolder(@"Data\Resources");
            if (dic == null || dic.Count == 0)
                throw new Exception("Assert Fails");
        }

        [Fact]
        public void DeserializeXMLTest()
        {
            fileAssistant = new FileAssistant();
            fileAssistant.SerializationType = "XML";

            var aElem = MockObjects.GetAnimationElement();

            byte[] bytes = fileAssistant.Serialize(aElem);
            string xml = new string(Encoding.Unicode.GetChars(bytes));
            if (bytes == null || bytes.Length == 0 && XDocument.Parse(xml) == null)
                throw new Exception("Assert Fails");

            var aElem2 = fileAssistant.Deserialize<AnimationSingleElement>(bytes);
            if (aElem.Shape.TypeName != aElem2.Shape.TypeName)
                throw new Exception("Assert Fails");
        }

        [Fact]
        public void DeserializeJSONTest()
        {
            fileAssistant = new FileAssistant();
            fileAssistant.SerializationType = "JSON";

            var aElem = MockObjects.GetAnimationElement();

            byte[] bytes = fileAssistant.Serialize(aElem);
            string xml = new string(Encoding.Unicode.GetChars(bytes));
            if (bytes == null || bytes.Length == 0 && XDocument.Parse(xml) == null)
                throw new Exception("Assert Fails");

            var aElem2 = fileAssistant.Deserialize<AnimationSingleElement>(bytes);
            if (aElem.Shape.TypeName != aElem2.Shape.TypeName)
                throw new Exception("Assert Fails");
        }

        [Fact]
        public void SerializeJSONTest()
        {
            fileAssistant = new FileAssistant();
            fileAssistant.SerializationType = "JSON";
            var aElem = MockObjects.GetAnimationElement();

            byte[] bytes = fileAssistant.Serialize(aElem);
            string xml = new string(Encoding.Unicode.GetChars(bytes));
            if (bytes == null || bytes.Length == 0 && XDocument.Parse(xml) == null)
                throw new Exception("Assert Fails");
        }

        [Fact]
        public void SerializeXMLTest()
        {
            fileAssistant = new FileAssistant();
            fileAssistant.SerializationType = "XML";
            var aElem = MockObjects.GetAnimationElement();

            byte[] bytes = fileAssistant.Serialize(aElem);
            string xml = new string(Encoding.Unicode.GetChars(bytes));
            if (bytes == null || bytes.Length == 0 && XDocument.Parse(xml) == null)
                throw new Exception("Assert Fails");
        }

        [Fact]
        public void SaveFileTest()
        {
            //FileTestHelper.CreateDummyFile("Test\\TEst2323","asd.txt");
            byte[] bytes = new byte[] { 1, 3, 4, 5, 33, 2, 32, 32 };
            string path = "Data\\testSave.txt";
            fileAssistant.SaveFile(path, bytes);
            byte[] bytes2 = File.ReadAllBytes(path);
            Assert.Equal(bytes, bytes2);
        }
    }
}