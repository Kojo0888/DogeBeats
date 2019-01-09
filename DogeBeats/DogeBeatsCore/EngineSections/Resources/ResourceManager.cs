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

        public void SetAllOfSerializedObjects<T>(string type, Dictionary<string, T> timeLines) where T : class
        {
            type = type.ToLower();

            foreach (var timeLinePair in timeLines)
            {
                SetSerializedObject(timeLinePair.Value, type, timeLinePair.Key);
            }
        }

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

            return _fileAssistant.DeserializeXML<T>(resourceBytes);
        }

        public void SetSerializedObject<T>(T obj, string type, string name) where T : class
        {
            var bytes = _fileAssistant.SerializeXML(obj);
            string path = _fileAssistant.GetFullPathForFolderName(type, RESOURCE_PATH);

            if (!name.EndsWith(".xml"))
                name += ".xml";

            type = type.ToLower();

            string fullPath = Path.Combine(path, name);
            File.WriteAllBytes(fullPath, bytes);
        }

        public Dictionary<string, T> GetAllOfSerializedObjects<T>(string type) where T : class
        {
            Dictionary<string, T> toReturn = new Dictionary<string, T>();
            type = type.ToLower();

            if (Resources.ContainsKey(type)){
                Dictionary<string, byte[]> resourcesWithType = Resources[type];

                foreach (var resourceWithType in resourcesWithType)
                {
                    var obj = _fileAssistant.DeserializeXML<T>(resourceWithType.Value);
                    toReturn.Add(resourceWithType.Key, obj);
                }
            }
            return toReturn;
        }
    }
}
