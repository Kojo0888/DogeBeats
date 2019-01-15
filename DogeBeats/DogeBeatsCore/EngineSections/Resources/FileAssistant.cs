﻿using DogeBeats.Other;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DogeBeats.EngineSections.Resources
{
    public class FileAssistant
    {
        public string SerializationType = "JSON";

        public DDictionary<string, byte[]> GetFilesFromFolder(string folderFullPath)
        {
            DDictionary<string, byte[]> toReturn = new DDictionary<string, byte[]>();
            string[] files = Directory.GetFiles(folderFullPath);
            foreach (var file in files)
            {
                string filename = Path.GetFileName(file);
                byte[] bytes = File.ReadAllBytes(file);
                toReturn[filename] = bytes;
            }
            return toReturn;
        }

        public DDictionary<string, DDictionary<string, byte[]>> GetAllResourceFilesFromFolder(string folderPath)
        {
            DDictionary<string, DDictionary<string, byte[]>> toReturn = new DDictionary<string, DDictionary<string, byte[]>>();
            string[] directories = Directory.GetDirectories(folderPath);
            foreach (var directory in directories)
            {
                var directoryName = Path.GetFileName(directory).ToLower();

                var dic = GetFilesFromFolder(directory);
                if(dic.Count > 0)
                    toReturn.Add(directoryName, dic);

                var childDic = GetAllResourceFilesFromFolder(directory);
                if(childDic.Count > 0)
                {
                    foreach (var childDicPair in childDic)
                    {
                        toReturn.Add(Path.Combine(directoryName, childDicPair.Key), childDicPair.Value);
                    }
                }
            }
            return toReturn;
        }

        public string GetFullPathForFolderName(string type, string basePath)
        {
            string[] dirs = Directory.GetDirectories(basePath);
            if (dirs != null && dirs.Length > 0)
            {
                foreach (var dir in dirs)
                {
                    if (Path.GetFileName(dir).ToLower() == type.ToLower())
                        return dir;
                    else
                    {
                        string path = GetFullPathForFolderName(type, dir);
                        if (!string.IsNullOrEmpty(path))
                            return path;
                    }

                }
            }
            return string.Empty;
        }

        #region serialiation

        public T Deserialize<T>(byte[] resourceBytes) where T : class 
        {
            if (SerializationType == "JSON")
                return DeserializeJSON<T>(resourceBytes);
            else if (SerializationType == "XML")
                return DeserializeXML<T>(resourceBytes);
            else
                throw new Exception("Serialization Type: " + SerializationType + " is not supported");
        }

        private T DeserializeXML<T>(byte[] resourceBytes) where T : class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(resourceBytes))
            {
                T obj = serializer.Deserialize(ms) as T;
                return obj;
            }
        }

        private T DeserializeJSON<T>(byte[] resourceBytes) where T : class
        {
            string json = new string (Encoding.UTF8.GetChars(resourceBytes));
            return JsonConvert.DeserializeObject<T>(json);
        }

        public byte[] Serialize<T>(T obj) where T : class
        {
            if (SerializationType == "JSON")
                return SerializeJSON<T>(obj);
            else if (SerializationType == "XML")
                return SerializeXML<T>(obj);
            else
                throw new Exception("Serialization Type: " + SerializationType + " is not supported");
        }

        private byte[] SerializeJSON<T>(T obj) where T : class
        {
            string json = JsonConvert.SerializeObject(obj);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            return bytes;
        }

        private byte[] SerializeXML<T>(T obj) where T : class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        #endregion
    }
}
