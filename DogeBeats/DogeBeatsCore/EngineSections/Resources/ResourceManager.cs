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
        public readonly string RESOURCE_PATH = @"Data\Resources";

        private DDictionary<string, DDictionary<string, byte[]>> Resources = new DDictionary<string, DDictionary<string, byte[]>>();

        private FileAssistant _fileAssistant = new FileAssistant();

        public void SaveAllOfSerializedObjects<T>(string type, Dictionary<string, T> timeLines) where T : class
        {
            foreach (var timeLinePair in timeLines)
            {
                SaveSerializedObject(timeLinePair.Value, type, timeLinePair.Key);
            }
        }

        public void LoadAllResources()
        {
            Resources = _fileAssistant.GetAllResourceFilesFromFolder(RESOURCE_PATH);
        }

        private string GetPossibleType(string type)
        {
            string possibleType = Resources.Keys.FirstOrDefault(f => f.ToLower() == type.ToLower());
            if (possibleType != null && Resources.ContainsKey(possibleType))
            {
                return possibleType;
            }
            else return type;
        }

        private string GetPossibleName(string possibleType, string name)
        {
            if (!name.Contains('.'))
                name = GetResourceNameWithExtension(possibleType, name);

            string possibleName = Resources[possibleType].Keys.FirstOrDefault(f => f.ToLower() == name.ToLower());

            if (Resources[possibleType].ContainsKey(possibleName))
                return possibleName;
            else
                return name;
        }

        public byte[] GetResource(string type, string name)
        {
            string possibleType = GetPossibleType(type);
            string possibleName = GetPossibleName(possibleType, name);

            if (Resources.ContainsKey(possibleType) && Resources[possibleType].ContainsKey(possibleName))
                return Resources[possibleType]?[possibleName];
            
            return new byte[0];
        }

        public DDictionary<string, byte[]> GetAllResources(string type)
        {
            string possibleType = GetPossibleType(type);
            if (!string.IsNullOrEmpty(possibleType) && Resources.ContainsKey(possibleType))
            {
                return Resources[type];
            }
            return new DDictionary<string, byte[]>();
        }

        public void AddOrUpdateResource(string type, string name, byte[] bytes)
        {
            string possibleType = GetPossibleType(type);
            string possibleName = GetPossibleName(possibleType, name);

            if (Resources.ContainsKey(possibleType) && Resources[possibleType].ContainsKey(possibleName))
                Resources[possibleType][possibleName] = bytes;
        }

        public void Save(string type, Dictionary<string, byte[]> resources)
        {
            foreach (var resource in resources)
            {
                AddResourceToList(resource.Key, type, resource.Value);
                _fileAssistant.SaveFile(resource.Key, resource.Value);
            }

            //SaveAll();
        }

        public void SaveAll()
        {
            foreach (var resourceTypePair in Resources)
            {
                foreach (var resource in resourceTypePair.Value)
                {
                    string path = Path.Combine(RESOURCE_PATH, resourceTypePair.Key, resource.Key);
                    _fileAssistant.SaveFile(path, resource.Value);
                }
            }
        }

        public string GetResourceNameWithExtension(string type, string name)
        {

            string key = Resources[type].Keys.FirstOrDefault(f => f.ToLower().StartsWith(name.ToLower()));
            if (!string.IsNullOrEmpty(key))
                return key;
            return string.Empty;
            //key = Resources[type].Keys.FirstOrDefault(f => f.ToLower().StartsWith(name.ToLower()));
        }

        public T GetSerializedObject<T>(string type, string name) where T : class
        {
            byte[] resourceBytes = GetResource(type, name);
            //string xmlString = new string(Encoding.UTF8.GetChars(resourceBytes));

            name = Path.GetFileNameWithoutExtension(name).ToLower();

            return _fileAssistant.Deserialize<T>(resourceBytes);
        }

        public void SaveSerializedObject<T>(T obj, string type, string name) where T : class
        {
            var bytes = _fileAssistant.Serialize(obj);
            string path = _fileAssistant.GetFullPathForFolderName(type, RESOURCE_PATH);

            if (!name.Contains("."))
                name += "." + _fileAssistant.SerializationType.ToLower();

            AddResourceToList(name, type, bytes);

            string fullPath = Path.Combine(path, name);
            File.WriteAllBytes(fullPath, bytes);
        }

        public DDictionary<string, T> GetAllOfSerializedObjects<T>(string type) where T : class
        {
            DDictionary<string, T> toReturn = new DDictionary<string, T>();

            if (Resources.ContainsKey(type))
            {
                DDictionary<string, byte[]> resourcesWithType = Resources[type];

                foreach (var resourceWithType in resourcesWithType)
                {
                    var fileNamewithExtension = resourceWithType.Key;
                    var obj = _fileAssistant.Deserialize<T>(fileNamewithExtension, resourceWithType.Value);
                    toReturn.Add(fileNamewithExtension, obj);
                }
            }
            return toReturn;
        }

        private void AddResourceToList(string name, string type, byte[] bytes)
        {
            if (!Resources.ContainsKey(type))
                Resources[type] = new DDictionary<string, byte[]>();
            if (!name.Contains("."))
                name += "." + _fileAssistant.SerializationType.ToLower();

            //if (!Resources[type].ContainsKey(name))
            Resources[type][name] = bytes;
        }
    }
}
