using DogeBeats.Other;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogeBeats.Model
{
    public static class CenterResource
    {
        public static NDictionary<string, NDictionary<string, byte[]>> Resources = new NDictionary<string, NDictionary<string, byte[]>>();
        
        public static readonly string RESOURCE_PATH = @"Data\Resources";

        public static void LoadAllResources()
        {
            LoadResourcesFromFolder(RESOURCE_PATH);

            string[] directories = Directory.GetDirectories(RESOURCE_PATH);
            foreach (var directory in directories)
            {
                Resources.Add(directory, new NDictionary<string, byte[]>());
                LoadResourcesFromFolder(directory);
            }
        }

        private static void LoadResourcesFromFolder(string folder)
        {
            string[] files = Directory.GetFiles(RESOURCE_PATH);
            foreach (var file in files)
            {
                string filename = Path.GetFileNameWithoutExtension(file);
                byte[] bytes = File.ReadAllBytes(file);
                Resources[folder].Add(filename, bytes);
            }
        }

        public static byte[] GetResource(string type, string name)
        {
            if (Resources != null && Resources.ContainsKey(name))
                return Resources[type]?[name];
            return new byte[0];
        }
    }
}
