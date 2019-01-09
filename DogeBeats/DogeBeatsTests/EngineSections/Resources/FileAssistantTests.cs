using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogeBeats.EngineSections.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;
using System.Xml.Linq;

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
        public void SerializeXMLTest()
        {
            var values = new System.Collections.Specialized.NameValueCollection();
            values.Add("Prediction", "False");
            values.Add("ShapeName", "IdkYet");
            var aElem = AnimationElement.Create(values);

            byte[] bytes = fileAssistant.SerializeXML(aElem);
            string xml = new string(Encoding.Unicode.GetChars(bytes));
            if (bytes == null || bytes.Length == 0 && XDocument.Parse(xml) == null)
                Assert.Fail();
        }

        [TestMethod()]
        public void DeserializeXMLTest()
        {
            var values = new System.Collections.Specialized.NameValueCollection();
            values.Add("Prediction", "False");
            values.Add("ShapeName", "IdkYet");
            var aElem = AnimationElement.Create(values);

            byte[] bytes = fileAssistant.SerializeXML(aElem);
            string xml = new string(Encoding.Unicode.GetChars(bytes));
            if (bytes == null || bytes.Length == 0 && XDocument.Parse(xml) == null)
                Assert.Fail();

            var aElem2 = fileAssistant.DeserializeXML<AnimationElement>(bytes);
            if (aElem.Shape.TypeName != aElem2.Shape.TypeName)
                Assert.Fail();
        }
    }
}