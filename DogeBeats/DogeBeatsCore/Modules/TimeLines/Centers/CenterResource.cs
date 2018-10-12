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
        public static Dictionary<string, byte[]> Resources = new Dictionary<string, byte[]>();

        public static readonly string RESOURCE_PATH = @"Data\Resources";

        public static void LoadAllResources()
        {
            string[] files = Directory.GetFiles(RESOURCE_PATH);

            foreach (var file in files)
            {
                LoadResource(file);
            }
        }

        private static void LoadResource(string file)
        {
            byte[] resource = File.ReadAllBytes(file);
            Resources.Add(Path.GetFileNameWithoutExtension(file), resource);
        }

        public static byte[] GetResource(string name)
        {
            if (Resources != null && Resources.ContainsKey(name))
                return Resources[name];
            return new byte[0];
        }
    }
}
