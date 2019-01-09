using DogeBeats.Other;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DogeBeats.EngineSections.Resources
{
    public class FileAssistant
    {
        public DDictionary<string, byte[]> GetFilesFromFolder(string folderFullPath)
        {
            DDictionary<string, byte[]> toReturn = new DDictionary<string, byte[]>();
            string[] files = Directory.GetFiles(folderFullPath);
            foreach (var file in files)
            {
                string filename = Path.GetFileNameWithoutExtension(file).ToLower();
                byte[] bytes = File.ReadAllBytes(file);
                toReturn[filename] = bytes;
            }
            return toReturn;
        }

        internal DDictionary<string, DDictionary<string, byte[]>> GetAllResourceFilesFromFolder(string folderPath)
        {
            DDictionary<string, DDictionary<string, byte[]>> toReturn = new DDictionary<string, DDictionary<string, byte[]>>();
            string[] directories = Directory.GetDirectories(folderPath);
            foreach (var directory in directories)
            {
                var directoryName = Path.GetFileName(directory).ToLower();

                //toReturn.Add(directoryName, new DDictionary<string, byte[]>());
                var dic = GetFilesFromFolder(directory);
                toReturn.Add(directoryName, dic);
            }
            return toReturn;
        }

        public T DeserializeXML<T>(byte[] resourceBytes) where T : class 
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(resourceBytes))
            {
                T obj = serializer.Deserialize(ms) as T;
                return obj;
            }
        }

        internal string GetFullPathForFolderName(string type, string basePath)
        {
            string[] dirs = Directory.GetDirectories(basePath);
            if(dirs != null && dirs.Length > 0)
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

        public byte[] SerializeXML<T>(T obj) where T : class
        {
            //byte[] resourceBytes = GetResource(type, name);
            //string xmlString = new string(Encoding.UTF8.GetChars(resourceBytes));

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        internal T LoadTimeLine<T>(string file) where T : class
        {
            string data = File.ReadAllText(file);
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (TextReader tr = new StreamReader(file))
            {
                T obj = serializer.Deserialize(tr) as T;
                return obj;
            }
        }
    }
}
