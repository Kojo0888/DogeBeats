using DogeBeats.EngineSections.Resources;
using DogeBeats.Other;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DogeBeats.Model
{
    public class ResourceManager
    {
        public DDictionary<string, DDictionary<string, byte[]>> Resources = new DDictionary<string, DDictionary<string, byte[]>>();
        
        public readonly string RESOURCE_PATH = @"Data\Resources";

        private FileAssistant _fileAssistant = new FileAssistant();

        public void LoadAllResources()
        {
            Resources = _fileAssistant.GetAllResourceFilesFromFolder(RESOURCE_PATH);
        }

        public byte[] GetResource(string type, string name)
        {
            if (name.Contains('.'))
                name = Path.GetFileNameWithoutExtension(name);

            name = name.ToLower();
            type = type.ToLower();

            if (Resources != null && Resources.ContainsKey(type))
                if (Resources[type].ContainsKey(name))
                    return Resources[type]?[name];
            return new byte[0];
        }

        public T GetSerializedObject<T>(string type, string name) where T : class
        {
            byte[] resourceBytes = GetResource(type, name);
            //string xmlString = new string(Encoding.UTF8.GetChars(resourceBytes));
            type = type.ToLower();
            name = name.ToLower();

            return _fileAssistant.DeserializeXML<T>(resourceBytes, type, name);
        }

        public void SetSerializedObject<T>(T obj, string type, string name) where T : class
        {
            var bytes = _fileAssistant.SerializeXML(obj, type, name);
            string path = _fileAssistant.GetFullPathForFolderName(type, RESOURCE_PATH);

            if (!name.EndsWith(".xml"))
                name += ".xml";

            type = type.ToLower();

            string fullPath = Path.Combine(path, name);
            File.WriteAllBytes(fullPath, bytes);
        }
    }
}
