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

            return _fileAssistant.Deserialize<T>(resourceBytes);
        }

        public void SetSerializedObject<T>(T obj, string type, string name) where T : class
        {
            var bytes = _fileAssistant.Serialize(obj);
            string path = _fileAssistant.GetFullPathForFolderName(type, RESOURCE_PATH);

            type = type.ToLower();

            AddResourceToList(name, type, bytes);//pieroli sie tu

            string extension = "." + _fileAssistant.SerializationType.ToLower();
            if (!name.EndsWith(extension))
                name += extension;

            string fullPath = Path.Combine(path, name);
            File.WriteAllBytes(fullPath, bytes);
        }

        private void AddResourceToList(string name, string type, byte[] bytes)
        {
            if (!Resources.ContainsKey(type))
                Resources[type] = new DDictionary<string, byte[]>();

            //if (!Resources[type].ContainsKey(name))
            Resources[type][name] = bytes;
        }

        public DDictionary<string, T> GetAllOfSerializedObjects<T>(string type) where T : class
        {
            DDictionary<string, T> toReturn = new DDictionary<string, T>();
            type = type.ToLower();

            if (Resources.ContainsKey(type)){
                DDictionary<string, byte[]> resourcesWithType = Resources[type];

                foreach (var resourceWithType in resourcesWithType)
                {
                    var fileNameWithoutExt = Path.GetFileNameWithoutExtension(resourceWithType.Key);
                    var obj = _fileAssistant.Deserialize<T>(resourceWithType.Value);
                    toReturn.Add(fileNameWithoutExt, obj);
                }
            }
            return toReturn;
        }
    }
}
